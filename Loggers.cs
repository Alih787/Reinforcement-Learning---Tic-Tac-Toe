using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe
{
    ///<summary>
    ///<para>Form For Logs</para>
    ///</summary>
    public partial class Loggers : Form
    {
        public bool actualclose = false;
        public bool indepth = false;
        public Loggers()
        {
            InitializeComponent();
            Link.Logger = Loggingbox;
            Link.loggers = (Loggers)FindForm();
        }

        private void Loggers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (actualclose)
            {
                return;
            }
            else if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
        }
    }
}
