using System.Windows.Forms;

namespace TicTacToe
{
    class Player
    {
        public string XorO = "Null";
        public string name;

        public void Valuenter(int i)
        {
            Grid.grid[i] = XorO;
            Link.Logs.Input($"{name} - {XorO} Placed On {Link.Logs.Translate(i)}\n");
        }

        public void PlayerPlace(object sender)
        {
            switch (((Button)sender).Name)
            {
                case "A1":
                    Valuenter(0);
                    break;
                case "A2":
                    Valuenter(1);
                    break;
                case "A3":
                    Valuenter(2);
                    break;
                case "B1":
                    Valuenter(3);
                    break;
                case "B2":
                    Valuenter(4);
                    break;
                case "B3":
                    Valuenter(5);
                    break;
                case "C1":
                    Valuenter(6);
                    break;
                case "C2":
                    Valuenter(7);
                    break;
                case "C3":
                    Valuenter(8);
                    break;
            }
            Grid.Updategrid();
        }
    }
}
