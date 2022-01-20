using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

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

