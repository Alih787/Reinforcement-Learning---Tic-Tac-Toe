using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    static class NodeSystem
    {
        public static string tparentnode = "---------";
        ///<summary>
        ///<para>storage of model</para>
        ///</summary>
        public static Noding.Model MModel = new Noding.Model();
        ///<summary>
        ///<para>storage of parent node</para>
        ///</summary>
        public static Noding.Node parentnode = new Noding.Node();

        //node manipulation
        ///<summary>
        ///<para>node manipulation, structure model, serialisation</para>
        ///</summary>
        public static class Noding
        {
            public static class Serialilse
            {
                ///<summary>
                ///<para>Serialises Model To a File</para>
                ///</summary>
                public static void Serial(object data, string path)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    FileStream fs = File.Open(path, FileMode.Create);
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(fs, data);
                    fs.Close();
                }
                ///<summary>
                ///<para>Deserialises Model From a File</para>
                ///</summary>
                public static Model Deserial(string path)
                {
                    if (File.Exists(path))
                    {
                        try
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                            Model obj = (Model)binaryFormatter.Deserialize(fs);
                            fs.Close();
                            return obj;
                        }
                        catch (SerializationException)
                        {
                            MessageBox.Show("Bad File","'Model.bin' is Corrupt. Please Delete All Model Files",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            Link.Main.Close();
                            throw new Exception("Corrupt File");
                        }
                    }
                    else
                    {
                        throw new Exception("No Such File Exists");
                    }
                }
            }

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
            
            ///<summary>
            ///<para>Returns Current Node</para>
            ///</summary>
            public static Node ReturnCurrentNode()
            {
                Node node = new Node
                {
                    Current = ReturnCurrentBoard().ToList(),
                    Moves = CurrentMoves().ToList(),
                    ParentNodes = new List<Node>
                    {
                    parentnode
                    }
                };
                if (Grid.CheckWinX())
                {
                    node.reward = 1;
                }
                else if (Grid.CheckWinO())
                {
                    node.reward = -1;
                }
                else if (Grid.CheckDraw())
                {
                    node.reward = 0;
                }
                else
                {
                    node.reward = null;
                }
                return node;
            }
            ///<summary>
            ///<para>Finds Matching Node In Model</para>
            ///<para>Does turn-1 To Translate To Model</para>
            ///</summary>
            public static Node FindCurrentNode(int turn, Node Lnode)
            {
                Node node = new Node();
                Parallel.ForEach(MModel.Turns[turn - 1].MovesDone, md =>
                   {
                       if (md.movesdone.SequenceEqual(Lnode.Moves))
                       {
                           Parallel.ForEach(md.Nodes, nodes =>
                           {
                               if (nodes.Current.SequenceEqual(Lnode.Current))
                               {
                                   node = nodes;
                               }
                           });
                       }
                   });
                return node;
            }
            ///<summary>
            ///<para>Returns All Nodes</para>
            ///<para>Does Turn-1 To Translate To Model</para>
            ///</summary>
            public static List<Node> GetNodes(int turn, Node node)
            {
                MovesDone movesdone = new MovesDone();
                List<MovesDone> mvs = MModel.Turns[turn - 1].MovesDone;
                Parallel.ForEach(mvs, mv =>
                 {
                     if (mv.movesdone.SequenceEqual(node.Moves))
                     {
                         movesdone = mv;
                     }
                 });
                return movesdone.Nodes;
            }
            public static void foldermake(int turn, Node node)
            {
                bool truth = false;
                truth = MModel.Turns[turn - 1].MovesDone.AsParallel().Any(mv => mv.movesdone.SequenceEqual(node.Moves));
                if (!truth)
                {
                    MModel.Turns[turn - 1].MovesDone.Add(new MovesDone()
                    {
                        movesdone = node.Moves
                    });
                }
            }
        }

        ///<summary>
        ///<para>resets parentnode to Base Node</para>
        ///</summary>
        public static void ResetParentNode()
        {
            parentnode = new Noding.Node()
            {
                Current = "---------".ToList(),
                Moves = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }
            };
        }

        //making node
        ///<summary>
        ///<para>Makes New Node or Updates Node</para>
        ///</summary>
        public static void Intialisenode()
        {
            
            //actual
            Thread actual = new Thread(() =>
            {
                Noding.Node PresentNode = Noding.ReturnCurrentNode();
                if (!Noding.GetNodes(Link.Main.turntally, PresentNode).Any(n => n.Current.SequenceEqual(PresentNode.Current)))
                {
                    Noding.foldermake(Link.Main.turntally, PresentNode);
                    Noding.Node current = Noding.ReturnCurrentNode();
                    Noding.GetNodes(Link.Main.turntally, current).Add(current);
                    parentnode = current;
                }
                else
                {
                    Noding.Node cnode = Noding.FindCurrentNode(Link.Main.turntally, PresentNode);
                    if (cnode.Current != null)
                    {
                        AttachParent(cnode);
                    }
                    parentnode = cnode;
                }
            });

            //for textside
            string path = $@".\Model\{Link.Main.turntally}\{ReturnCurrentBoard()}.txt";
            Thread text = new Thread(() =>
            {
                //folders
                if (!File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine($"Available Moves : {string.Join(",", CurrentMoves())}");
                    sw.WriteLine($"Parent Node : {tparentnode}");
                    if (Grid.CheckWinO())//where X is maximising reward
                    {
                        sw.WriteLine("Reward : -1");
                        tparentnode = "---------";
                    }
                    else if (Grid.CheckWinX())
                    {
                        sw.WriteLine("Reward : 1");
                        tparentnode = "---------";
                    }
                    else if (Grid.CheckDraw())
                    {
                        sw.WriteLine("Reward : 0");
                        tparentnode = "---------";
                    }
                    else
                    {
                        sw.WriteLine("\n");
                    }
                    sw.Close();
                    tparentnode = ReturnCurrentBoard();
                }
                else
                {
                    Tattachparent(path);
                    tparentnode = ReturnCurrentBoard();
                }
            });

            actual.Start();
            text.Start();

            actual.Join();
            text.Join();

        }

        //finding best node
        ///<summary>
        ///<para>finds best move</para>
        ///</summary>
        public static Noding.Node BestMove(bool X)
        {
            Random random = new Random();
            Noding.Node bestmv = new Noding.Node();
            Noding.Node[] possiblenodes = FindChilds(Link.Main.turntally, Noding.ReturnCurrentNode());
            int[] rewards = new int[possiblenodes.Length];
            Parallel.For(0, possiblenodes.Length, j =>
              {
                  Link.Logs.Input($"Bot : Thread for '{string.Join("", possiblenodes[j].Current)}' board started");
                  lock (rewards)
                  {
                      rewards[j] = ReturnReward(FindEndNodesChilds(possiblenodes[j], Link.Main.turntally + 1), possiblenodes[j]);
                  }
                  Link.Logs.Input($"Bot : Thread for '{string.Join("", possiblenodes[j].Current)}' board exited with reward");
                  Link.Logs.UpdateLog();
              });
            if (X)
            {
                int tempbestreward = 0;
                for (int i = 0; i < possiblenodes.Length; i++)
                {
                    if (i == 0)
                    {
                        bestmv = possiblenodes[i];
                        tempbestreward = rewards[i];
                    }
                    if (rewards[i] > tempbestreward)
                    {
                        bestmv = possiblenodes[i];
                        tempbestreward = rewards[i];
                    }
                }
                Link.Logs.Input($"Bot : '{string.Join("", bestmv.Current)}' was decided");
                Link.Logs.UpdateLog();
            }
            else
            {
                int tempbestreward = 0;
                for (int i = 0; i < possiblenodes.Length; i++)
                {
                    if (i == 0)
                    {
                        bestmv = possiblenodes[i];
                        tempbestreward = rewards[i];
                    }
                    if (rewards[i] < tempbestreward)
                    {
                        bestmv = possiblenodes[i];
                        tempbestreward = rewards[i];
                    }
                }
                Link.Logs.Input($"Bot : Thread {string.Join("", bestmv.Current)} was decided");
                Link.Logs.UpdateLog();
            }
            Link.Logs.Input("");
            Link.Logs.UpdateLog();
            return bestmv;
        }

        //node/game value return
        ///<summary>
        ///<para>Returns reward of endnodes</para>
        ///</summary>
        public static int ReturnReward(Noding.Node[] children,Noding.Node ParentedNode)
        {
            int reward = 0;
            if (children.Length > 0)
            {
                Parallel.For(0, children.Length, (i, state) =>
                {
                    reward += (int)children[i].reward;
                });
            }
            Link.Logs.IInput($"Bot : Found Total Reward {reward} For {string.Join("",ParentedNode.Current)}");
            Link.Logs.UpdateLog();
            return reward;
        }
        ///<summary>
        ///<para>Returns Board as a String</para>
        ///</summary>
        public static string ReturnCurrentBoard()
        {
            string statis = string.Empty;
            for (int i = 0; i < Grid.grid.Length; i++)
            {
                if (Grid.grid[i] == string.Empty)
                {
                    statis += "-";
                }
                else
                {
                    statis += Grid.grid[i];
                }
            }
            return statis;
        }
        ///<summary>
        ///<para>returns availiable moves from node</para>
        ///</summary>
        public static int[] ReturnMoves(Noding.Node node)
        {
            return node.Moves.ToArray();
        }
        ///<summary>
        ///<para>returns moves that can be done</para>
        ///</summary>
        public static int[] CurrentMoves()
        {
            List<int> moves = new List<int>();

            Parallel.For(0, Grid.grid.Length, i =>
            {
                if (Grid.grid[i] == string.Empty)
                {
                    lock (moves)
                    {
                        moves.Add(i);
                    }
                }
            });

            return (moves.ToArray());
        }
        ///<summary>
        ///<para>returns diffrence in moves between the to lists</para>
        ///</summary>
        public static int MoveDiff(int[] Blist, int[] Slist)
        {
            return (Blist.AsParallel().Except(Slist.AsParallel())).First();
        }

        //searching
        ///<summary>
        ///<para>finds child of node provided</para>
        ///</summary>
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
                          Link.Logs.IInput($"Bot : Found Child Nodes '{string.Join("", node.Current)}' for '{string.Join("", ParentNode.Current)}'");
                          lock (children)
                          {
                              children.Add(node);
                          }
                      }
                  });
             });
            return children.ToArray();
        }
        ///<summary>
        ///<para>finds end nodes of node provided</para>
        ///</summary>
        public static Noding.Node[] FindEndNodesChilds(Noding.Node parent, int turn)
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
                        Link.Logs.IInput($"Bot : Found End Node '{string.Join("", nodes[i].Current)}' with reward : {nodes[i].reward} for parent '{string.Join("", parentnode.Current)}'");
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
                 Noding.Node[] nodestoadd = FindEndNodesChilds(childs, turn + 1);
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

        //node editing
        ///<summary>
        ///<para>attaches parent to node (text edit)</para>
        ///</summary>
        public static void Tattachparent(string path)
        {
            string[] ncontent = File.ReadAllLines(path);
            for (int i = 0; i < ncontent.Length; i++)
            {
                if (ncontent[i].Contains("Parent Node : "))
                {
                    string Eparentnodes = ncontent[i].Replace("Parent Node : ", "");
                    List<string> parents = new List<string>(Eparentnodes.Split(','));
                    bool alreadycontains = false;
                    for (int j = 0; j < parents.Count; j++)
                    {
                        if (parents[j] == tparentnode)
                        {
                            alreadycontains = true;
                        }
                    }
                    if (!alreadycontains)
                    {
                        parents.Add(tparentnode);
                    }
                    ncontent[i] = ($"Parent Node : {string.Join(",", parents)}");

                }
            }
            File.WriteAllLines(path, ncontent);
        }
        ///<summary>
        ///<para>attaches parent to node (model edit)</para>
        ///</summary>
        public static void AttachParent(Noding.Node node)
        {
            if (!node.ParentNodes.AsParallel().Any(n => n.Current.SequenceEqual(parentnode.Current)))
            {
                node.ParentNodes.Add(parentnode);
            }
        }

        //creation funtions
        ///<summary>
        ///<para>makes folder structure</para>
        ///</summary>
        public static void MakeFolderStructure()
        {
            if (!File.Exists(@".\Model.bin"))
            {
                Noding.Model model = ConstructEmptyModel();
                Noding.Serialilse.Serial(model, @".\Model.bin");
            }
            if (!Directory.Exists(@".\Model"))
            {
                Directory.CreateDirectory($@".\Model\1\");
                Directory.CreateDirectory($@".\Model\2\");
                Directory.CreateDirectory($@".\Model\3\");
                Directory.CreateDirectory($@".\Model\4\");
                Directory.CreateDirectory($@".\Model\5\");
                Directory.CreateDirectory($@".\Model\6\");
                Directory.CreateDirectory($@".\Model\7\");
                Directory.CreateDirectory($@".\Model\8\");
                Directory.CreateDirectory($@".\Model\9\");
            }
        }
        ///<summary>
        ///<para>makes empty model</para>
        ///</summary>
        public static Noding.Model ConstructEmptyModel()
        {
            Noding.Model model = new Noding.Model();
            for (int i = 0; i < model.Turns.Capacity; i++)
            {
                model.Turns[i] = new Noding.Turn();
            }
            return model;
        }
    }
}
