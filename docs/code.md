# Code Breakdown  
## Start Button
the start button that allow you to play the game and also sets up the game depending on if your playing PvP/BvB/Player vs Bot.  
```c#
//in main.cs
private void Start(object sender, EventArgs e)
{
    ingame = true;
    NodeSystem.ResetParentNode();//resets what the parent node is
    Link.Logs.Input("Games : Game Started\n");
    turntally = 0;//reset what turn it is
    Updatetturn();
    if (MPlayer.Checked)//2player
    {
        player1.XorO = "X";//sets player 1 to X
        player2.XorO = "O";//sets player 2 to O
        player1.name = "Player One";//sets player 1 name to player one
        player2.name = "Player Two";//sets player 1 name to player two
        turn = "X";//starting turn is X
    }
    else if (bots.Checked)/bot vs bot
    {
        Agent.XorO = "X";
        turn = "X";
        while (!Grid.CheckEnd())//loops until there won/lost/draw
        {
            PlayerPlace(sender, e);//bot move
        }

    }
    else if (GoFirst.Checked)//player vs bot (player goes first)
    {
        player1.XorO = "X";
        player1.name = "Player";
        Agent.XorO = "O";
        turn = "X";
    }
    else if (!GoFirst.Checked)//player vs bot (bot goes first)
    {
        player1.XorO = "O";
        Agent.XorO = "X";
        player1.name = "Player";
        turn = "X";
        PlayerPlace(sender, e);
    }
    Ingame(true);//changes buttons to unclickable whilst in game
    Link.Logs.UpdateLog();
}
```  
## Player/Bot Move
in the game the player or bot does a move and this function is called :
```c#  
//in main.cs
private void PlayerPlace(object sender, EventArgs e)//when a player or bot place a move
{
    if (!ingame)
    {
        return;
    }
    bool donemove = false;

    if (MPlayer.Checked && !Grid.CheckEnd())//if multiplayer
    {
        if (player1.XorO == turn)
        {
            player1.PlayerPlace(sender);//place X/O for player
            donemove = true;
        }
        else if (player2.XorO == turn)
        {
            player2.PlayerPlace(sender);
            donemove = true;
        }
    }
    else// if singleplayer
    {
        if (player1.XorO == turn && !Grid.CheckEnd())
        {
            player1.PlayerPlace(sender);
            donemove = true;
        }
    }
    if (donemove)//if player move is done
    {
        turntally++;
        Updatetturn();
        if (!MPlayer.Checked)
        {
            NodeSystem.Intialisenode();//makes a new node for the board state
        }
        Endcheck();
        Switchturn();//changes turn over
    }
    if (turn == Agent.XorO && !MPlayer.Checked && !Grid.CheckEnd())//if playing against agent
    {
        Agent.AImove();//the agent does a move
        turntally++;
        Updatetturn();
        NodeSystem.Intialisenode();
        Endcheck();
        Switchturn();

    }
}
```  
## How it is Modelled
each structure in the model that that allow the nodes to be quickly searched. this allows the program to skip 
```c#
//in node.cs (in noding class)
//structure of model
[Serializable]
public class Model
{
    public List<Turn> Turns = new List<Turn>(new Turn[9]);
}
[Serializable]
public class Turn
{
    public List<MovesDone> MovesDone = new List<MovesDone>();
}
[Serializable]
public class MovesDone
{
    public List<Node> Nodes = new List<Node>();
    public List<int> movesdone = new List<int>();
}
[Serializable]
public class Node
{
    public List<char> Current = new List<char>();
    public List<int> Moves = new List<int>();
    public List<Node> ParentNodes = new List<Node>();
    public Nullable<int> reward;
}
```
## Agent Decision  
the Agent.AImove() in the PlayerPlace() that makes the move for bot  
```c#
//in bot.cs
public static void AImove()
{
    Random rnd = new Random();
    try
    {
        if ((rnd.Next(0, 100) + 1) > ExploreP)//chance the bot does a random move rather than one based on memory
        {
            NodeSystem.Noding.Node stored = new NodeSystem.Noding.Node();
            bool agentx;

            if (XorO == "X")
            {
                agentx = true;
            }
            else if (XorO == "O")
            {
                agentx = false;
            }
            else
            {
                return;
            }

            Thread BestMV = new Thread(() =>
            {
                stored = NodeSystem.BestMove(agentx);//finding best move possible
            });
            BestMV.Start();
            BestMV.Join();

            if (stored != new NodeSystem.Noding.Node())
            {
                Valuenter(NodeSystem.MoveDiff(NodeSystem.CurrentMoves(), NodeSystem.ReturnMoves(stored)));//the move that is entered by comparing two nodes
                Link.Logs.Input("Bot : Reinforced Move");
            }
            else
            {
                RandomMove();//does a random move.
            }
        }
        else
        {
            RandomMove();
        }
    }
    catch (Exception)
    {
        RandomMove();
    }

    Grid.Updategrid();

    if (Link.Botters.Checked)
    {
        SwapTurn();
    }
    Link.Logs.UpdateLog();
}
``` 
## Finding Best Move Possible  
function gets the child nodes of current possition and then finds which one is the to do base on the amount of reward it has.  
```c#
//in node.cs
public static Noding.Node BestMove(bool X)
{
    Random random = new Random();
    Noding.Node bestmv = new Noding.Node();
    Noding.Node[] possiblenodes = FindChilds(Link.Main.turntally, Noding.ReturnCurrentNode());//finding child node
    opennodes = possiblenodes;
    double[] rewards = new double[possiblenodes.Length];
    int ThreadDecided = -1;
    Parallel.For(0, possiblenodes.Length, j =>
      {
          if (possiblenodes[j].reward == null)
          {
              Link.Logs.Input($"Bot : Thread [{j}] for '{string.Join("", possiblenodes[j].Current)}' board started");
              lock (rewards)
              {
                  rewards[j] = ReturnReward(FindEndNodesChilds(possiblenodes[j], j, possiblenodes[j], Link.Main.turntally + 1));//gets total reward for one child node
              }
              Link.Logs.Input($"Bot : Thread [{j}] for '{string.Join("", possiblenodes[j].Current)}' board exited with reward {Math.Round(rewards[j],4)}");
              Link.Logs.UpdateLog();
          }
          else
          {
              rewards[j] = (int)possiblenodes[j].reward;
              Link.Logs.Input($"Bot : Thread [{j}] for '{string.Join("", possiblenodes[j].Current)}' board exited with reward {possiblenodes[j].reward}");
              Link.Logs.UpdateLog();
          }
      });
    if (X)//comparing each child node
    {
        double tempbestreward = 0;
        for (int i = 0; i < possiblenodes.Length; i++)
        {
            if (i == 0)
            {
                bestmv = possiblenodes[i];
                tempbestreward = rewards[i];
            }
            if (rewards[i] > tempbestreward)//finding which reward is better (greater reward is better)
            {
                bestmv = possiblenodes[i];
                tempbestreward = rewards[i];
            }
        }
        Link.Logs.Input($"Bot : Thread [{ThreadDecided}] '{string.Join("", bestmv.Current)}' was decided");
        Link.Logs.UpdateLog();
    }
    else
    {
        double tempbestreward = 0;
        for (int i = 0; i < possiblenodes.Length; i++)
        {
            if (i == 0)
            {
                bestmv = possiblenodes[i];
                tempbestreward = rewards[i];
                ThreadDecided = i;
            }
            if (rewards[i] < tempbestreward)//finding which reward is better (lower reward is better)
            {
                bestmv = possiblenodes[i];
                tempbestreward = rewards[i];
                ThreadDecided = i;
            }
        }
        Link.Logs.Input($"Bot : Thread [{ThreadDecided}] '{string.Join("", bestmv.Current)}' was decided");
        Link.Logs.UpdateLog();
    }
    Link.Logs.Input("");
    Link.Logs.UpdateLog();
    return bestmv;
}
```
### Finding Child Nodes  
the function in bestmove that finds the child node
```c#
public static Noding.Node[] FindChilds(int turn, Noding.Node ParentNode)
{
    List<Noding.MovesDone> movesDones = MModel.Turns[turn].MovesDone;
    List<Noding.Node> possible = new List<Noding.Node>();
    List<Noding.Node> children = new List<Noding.Node>();
    Parallel.ForEach(movesDones, mv =>
    {
        if (ParentNode.Moves.Except(mv.movesdone).Count() == 1)
        {

            lock (possible)
            {
                possible.AddRange(mv.Nodes);
            }
        }
    });
    Parallel.ForEach(possible, node =>
     {
         Parallel.ForEach(node.ParentNodes, pnode =>
          {
              if (pnode.Current.SequenceEqual(ParentNode.Current))
              {
                  //Link.Logs.IInput($"Bot : Found Child Node '{string.Join("", node.Current)}' for '{string.Join("", ParentNode.Current)}'");
                  lock (children)
                  {
                      children.Add(node);
                  }
              }
          });
     });
    return children.ToArray();
}
```
### Finding End Nodes  
the function in bestmove that searches every branch to find node with rewards to return.
```c#
public static Noding.Node[] FindEndNodesChilds(Noding.Node rootnode,int j,Noding.Node parent, int turn)
{
List<Noding.Node> endnode = new List<Noding.Node>();
List<Thread> threads = new List<Thread>();
Noding.Node[] nodes = FindChilds(turn, parent);
List<Noding.Node> Childstolookup = new List<Noding.Node>();
if (nodes.Length > 0)
{
    Parallel.For(0, nodes.Length, (i, state) =>
    {
        bool isendnode = nodes[i].reward.HasValue;
        if (isendnode)
        {
            Link.Logs.IInput($"Bot : Found End Node '{string.Join("", nodes[i].Current)}' For Thread [{j}] '{string.Join("", rootnode.Current)}', reward {nodes[i].reward}");
            lock (endnode)
            {
                endnode.Add(nodes[i]);
            }
        }
        else if (!isendnode)
        {
            lock (Childstolookup)
            {

                Childstolookup.Add(nodes[i]);
            }
        }
    });
}
Parallel.ForEach(Childstolookup, childs =>
 {
     Noding.Node[] nodestoadd = FindEndNodesChilds(rootnode,j,childs, turn + 1);
     if (nodestoadd.Length != 0)
     {
         lock (endnode)
         {
             endnode.AddRange(nodestoadd);
         }
     }
 });
return endnode.ToArray();
}
```
### Return Reward  
find total reward of the combined children node from current node.
```c#
public static double ReturnReward(Noding.Node[] children)
{
    double reward = 0;
    if (children.Length > 0)
    {
        Parallel.For(0, children.Length, (i, state) =>
        {
        reward += (int)(children[i].reward) * (1.0f / (1.0f + (float)Math.Exp((9-(children[i].Current.Count(possibledash => possibledash == '-'))))));
        });
    }
    return reward;
}
```
# Pages  
[Home](index.md)  
[Usage Guide](guide.md)  
