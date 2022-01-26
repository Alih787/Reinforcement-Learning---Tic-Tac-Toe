using System.Windows.Forms;

namespace TicTacToe
{
    ///<summary>
    ///<para>Training Form to update training progress</para>
    ///</summary>
    public partial class TrainProg : Form
    {
        public ProgressBar progressBar;
        public Label Label;

        public TrainProg()
        {
            InitializeComponent();
            Link.trainProg = (TrainProg)FindForm();
            progressBar = progressBar1;
            Label = label1;
        }


    }
}

