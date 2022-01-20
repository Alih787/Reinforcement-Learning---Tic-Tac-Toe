using System;
using System.Windows.Forms;

namespace TicTacToe
{
    static class Link
    {
        static public Main Main;
        static public Loggers loggers;
        static public TrainProg trainProg;
        static public RichTextBox Logger;
        static public CheckBox Botters;

        static public class Logs
        {
            static public bool Enabled = true;
            static public string Translate(int i)
            {
                if (Enabled)
                {
                    return i switch
                    {
                        0 => "A1",
                        1 => "A2",
                        2 => "A3",
                        3 => "B1",
                        4 => "B2",
                        5 => "B3",
                        6 => "C1",
                        7 => "C2",
                        8 => "C3",
                        _ => "Null",
                    };
                }
                else
                {
                    return "Null";
                }
            }
            static public void Input(string input)
            {
                if (Enabled)
                {
                    Logger.Invoke((Action)delegate
                    {
                        Logger.Text += $"{input}\n";
                    });
                }
            }
            static public void IInput(string input)
            {
                if (Enabled&&Link.loggers.indepth)
                {
                    Logger.Invoke((Action)delegate
                    {
                        Logger.Text += $"{input}\n";
                    });
                }
            }
            static public void Clear()
            {
                if (Enabled)
                {
                    Logger.Invoke((Action)delegate
                    {
                        Logger.Text = string.Empty;
                    });
                }
            }
        }
    }
}
