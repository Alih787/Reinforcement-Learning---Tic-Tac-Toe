# Code Breakdown  
the 
```c#
//in main.cs
private void Start(object sender, EventArgs e)
{
    ingame = true;
    NodeSystem.ResetParentNode();
    Link.Logs.Input("Games : Game Started\n");
    turntally = 0;
    Updatetturn();
    if (MPlayer.Checked)
    {
        player1.XorO = "X";
        player2.XorO = "O";
        player1.name = "Player One";
        player2.name = "Player Two";
        turn = "X";
    }
    else if (bots.Checked)
    {
        Agent.XorO = "X";
        turn = "X";
        while (!Grid.CheckEnd())
        {
            PlayerPlace(sender, e);
        }

    }
    else if (GoFirst.Checked)
    {
        player1.XorO = "X";
        player1.name = "Player";
        Agent.XorO = "O";
        turn = "X";
    }
    else if (!GoFirst.Checked)
    {
        player1.XorO = "O";
        Agent.XorO = "X";
        player1.name = "Player";
        turn = "X";
        PlayerPlace(sender, e);
    }
    Ingame(true);
    Link.Logs.UpdateLog();
}
```
# Pages  
[Home](index.md)  
[Usage Guide](guide.md)  
