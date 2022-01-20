namespace TicTacToe
{
    partial class Loggers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Loggingbox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Loggingbox
            // 
            this.Loggingbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Loggingbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Loggingbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Loggingbox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Loggingbox.Location = new System.Drawing.Point(12, 8);
            this.Loggingbox.Name = "Loggingbox";
            this.Loggingbox.ReadOnly = true;
            this.Loggingbox.Size = new System.Drawing.Size(570, 441);
            this.Loggingbox.TabIndex = 16;
            this.Loggingbox.Text = "";
            // 
            // Loggers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 461);
            this.Controls.Add(this.Loggingbox);
            this.Name = "Loggers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Logs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Loggers_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox Loggingbox;
    }
}