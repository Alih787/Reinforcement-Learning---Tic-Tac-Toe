using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Main : Form
    {
        bool ingame = false;
        Player player1 = new Player();
        Player player2 = new Player();
        public int turntally;
        string turn;
        List<object> buttons = new List<object>();
        public bool Training = false;

        public Main()
        {
            InitializeComponent();
            trackBar1.Value = Agent.ExploreP / 5;
            BvBRepeater.Text = string.Empty;
            percentan.Text = $"{Agent.ExploreP}%";
            object[] button = { A1, A2, A3, B1, B2, B3, C1, C2, C3 };
            buttons.AddRange(button);
            Grid.buttons = button;
            Grid.Reset();
            Link.Botters = bots;
            Thread thread = new Thread(() => new Loggers().ShowDialog());
            thread.Start();
            Link.Main = ((Main)this.FindForm());
            NodeSystem.MakeFolderStructure();
            NodeSystem.MModel = NodeSystem.Noding.BinConvert.Deserial(@".\Model.bin");
        }

        ///<summary>
        ///<para>updates turn count</para>
        ///</summary>
        void Updatetturn()
        {
            if (Training)
            {
                return;
            }
            if (turntally == 0)
            {
                tall.Text = string.Empty;
            }
            else
            {
                tall.Text = $"Turn : {turntally}";
            }
        }
        ///<summary>
        ///<para>changes to allow game input</para>
        ///</summary>
        void Ingame(bool ingame)
        {
            if (Training)
            {
                MPlayer.Enabled = false;
                GoFirst.Enabled = false;
                StartB.Enabled = false;
                bots.Enabled = false;
                BvBRepeater.ReadOnly = true;
                return;
            }
            if (ingame)
            {
                MPlayer.Enabled = false;
                GoFirst.Enabled = false;
                StartB.Enabled = false;
                bots.Enabled = false;
                BvBRepeater.ReadOnly = true;
            }
            else
            {
                MPlayer.Enabled = true;
                GoFirst.Enabled = true;
                StartB.Enabled = true;
                bots.Enabled = true;
                BvBRepeater.ReadOnly = false;
            }
        }
        ///<summary>
        ///<para>switches turns over</para>
        ///</summary>
        void Switchturn()
        {
            if (turn == "X")
            {
                turn = "O";
            }
            else if (turn == "O")
            {
                turn = "X";
            }
        }
        ///<summary>
        ///<para>checks to see if game is won</para>
        ///</summary>
        void Endcheck()
        {
            bool gamedone = false;
            if (MPlayer.Checked)
            {
                if (Grid.CheckWinO())
                {
                    if (player1.XorO == "O")
                    {
                        MessageBox.Show($"{player1.name} Win");
                    }
                    else
                    {
                        MessageBox.Show($"{player2.name} Win");
                    }
                    gamedone = true;
                }
                else if (Grid.CheckWinX())
                {
                    if (player1.XorO == "X")
                    {
                        MessageBox.Show($"{player1.name} Win");
                    }
                    else
                    {
                        MessageBox.Show($"{player2.name} Win");
                    }
                    gamedone = true;
                }
                else if (Grid.CheckDraw())
                {
                    MessageBox.Show($"Draw");
                    gamedone = true;
                }
            }
            else if (bots.Checked)
            {
                if (Training)
                {
                    gamedone = true;
                }
                else if (Grid.CheckWinO())
                {
                    MessageBox.Show($"Computer O Win");
                    gamedone = true;
                }
                else if (Grid.CheckWinX())
                {
                    MessageBox.Show($"Computer X Win");
                    gamedone = true;
                }
                else if (Grid.CheckDraw())
                {
                    MessageBox.Show($"Draw");
                    gamedone = true;
                }
            }
            else
            {
                if (Grid.CheckWinO())
                {

                    if (player1.XorO == "O")
                    {
                        MessageBox.Show($"{player1.name} Win");
                    }
                    else
                    {
                        MessageBox.Show($"Computer Win");
                    }
                    gamedone = true;
                }
                else if (Grid.CheckWinX())
                {
                    if (player1.XorO == "X")
                    {
                        MessageBox.Show($"{player1.name} Win");
                    }
                    else
                    {
                        MessageBox.Show($"Computer Win");
                    }
                    gamedone = true;
                }
                else if (Grid.CheckDraw())
                {
                    MessageBox.Show($"Draw");
                    gamedone = true;
                }
            }
            if (gamedone)
            {
                NodeSystem.Noding.BinConvert.Serial(NodeSystem.MModel, @".\Model.bin");
                if (!Training)
                {
                    if (Grid.CheckWinX())
                    {
                        Link.Logs.Input("X Wins");
                    }
                    if (Grid.CheckWinO())
                    {
                        Link.Logs.Input("O Wins");
                    }
                    if (Grid.CheckDraw())
                    {
                        Link.Logs.Input("Draw");
                    }
                    Link.Logs.Input("Game Ended\n");
                    foreach (object obj in buttons)
                    {
                        ((Button)obj).Enabled = false;
                    }
                    ingame = false;
                }
            }
        }
        ///<summary>
        ///<para>place X or O on board</para>
        ///</summary>
        private void PlayerPlace(object sender, EventArgs e)
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
                    player1.PlayerPlace(sender);
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
                    NodeSystem.Intialisenode();
                }
                Endcheck();
                Switchturn();
            }


            if (turn == Agent.XorO && !MPlayer.Checked && !Grid.CheckEnd())//if playing against agent
            {
                Agent.AImove();
                turntally++;
                Updatetturn();
                NodeSystem.Intialisenode();
                Endcheck();
                Switchturn();

            }


        }
        ///<summary>
        ///<para>start button</para>
        ///</summary>
        private void Start(object sender, EventArgs e)
        {
            ingame = true;
            NodeSystem.ResetParentNode();
            Link.Logs.Input("Game Started\n");
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

        }
        ///<summary>
        ///<para>resets board</para>
        ///</summary>
        private void Reset(object sender, EventArgs e)
        {
            if (!Training)
            {
                foreach (object item in buttons)
                {
                    ((Button)item).Enabled = true;
                }
            }
            turntally = 0;
            Link.Logs.Clear();
            Agent.XorO = "Null";
            NodeSystem.tparentnode = "---------";
            NodeSystem.parentnode = new NodeSystem.Noding.Node()
            {
                Current = "---------".ToList()
            };
            player1 = new Player();
            player2 = new Player();
            Grid.Reset();
            Grid.Updategrid();
            tall.Text = string.Empty;
            BvBRepeater.Text = string.Empty;
            Ingame(false);
        }
        ///<summary>
        ///<para>changes if multiplayer or not</para>
        ///</summary>
        private void ChangeMP(object sender, EventArgs e)
        {
            if (MPlayer.Checked)
            {
                GoFirst.Checked = false;
                bots.Checked = false;
            }
        }
        ///<summary>
        ///<para>changes if you go first or not</para>
        ///</summary>
        private void ChangeGF(object sender, EventArgs e)
        {
            if (GoFirst.Checked)
            {
                MPlayer.Checked = false;
                bots.Checked = false;
            }
        }
        ///<summary>
        ///<para>Changes if there is no player</para>
        ///</summary>
        private void ChangeNP(object sender, EventArgs e)
        {
            if (bots.Checked)
            {
                GoFirst.Checked = false;
                MPlayer.Checked = false;
            }
        }
        ///<summary>
        ///<para>Training Function</para>
        ///</summary>
        private void TrainerB_Click(object sender, EventArgs e)
        {
            Ingame(true);
            Training = true;
            Link.Logs.Enabled = false;
            bots.Checked = true;
            ChangeNP(sender, e);
            if (BvBRepeater.Text != string.Empty)
            {
                try
                {
                    int reper = Convert.ToInt32(BvBRepeater.Text);
                    TrainProg trainers = new TrainProg();
                    trainers.Show();
                    trainers.progressBar.Invoke((Action)delegate { trainers.progressBar.Maximum = reper; });
                    for (int i = 0; i < reper; i++)
                    {
                        trainers.progressBar.Invoke((Action)delegate { trainers.progressBar.PerformStep(); });
                        trainers.Label.Invoke((Action)delegate { trainers.Label.Text = $"Training : {i + 1}/{reper}"; });
                        Start(sender, e);
                        Reset(sender, e);
                    }
                    trainers.Close();

                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }

            Training = false;
            Link.Logs.Enabled = true;
            Ingame(false);
        }
        ///<summary>
        ///<para>Changes Exploration Percentage</para>
        ///</summary>
        private void SliderExploreP(object sender, EventArgs e)
        {
            Agent.ExploreP = trackBar1.Value * 5;
            percentan.Text = $"{Agent.ExploreP}% ";

        }
        ///<summary>
        ///<para>maximises log</para>
        ///</summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Link.loggers.Invoke((Action)delegate { Link.loggers.WindowState = FormWindowState.Normal; });
        }
        ///<summary>
        ///<para>updates the indepth boolean value</para>
        ///</summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Link.loggers.indepth = true;
            }
            else if (!checkBox1.Checked)
            {
                Link.loggers.indepth = false;
            }
        }
        ///<summary>
        ///<para>closes evereything properly</para>
        ///</summary>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Link.loggers.Invoke((Action)delegate
            {
                Link.loggers.actualclose = true;
                Link.loggers.Close();
            });
        }
    }
}
