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

        //for logger
        static public class Logs
        {
            static public bool Enabled = true;
            static public string Text = string.Empty;
            static public string Translate(int i)
            {
                if (Enabled)
                {
                    switch (i)
                    {
                        case 0: return "A1";
                        case 1: return "A2";
                        case 2: return "A3";
                        case 3: return "B1";
                        case 4: return "B2";
                        case 5: return "B3";
                        case 6: return "C1";
                        case 7: return "C2";
                        case 8: return "C3";
                        default: return "Null";
                    };
                }
                return "Null";
            }
            static public void Input(string input)
            {
                if (Enabled)
                {
                    lock (Text)
                    {
                        Text += $"{input}\n";
                    }
                }
            }
            static public void IInput(string input)
            {
                if (Enabled && Link.loggers.indepth)
                {
                    lock (Text)
                    {
                        Text += $"{input}\r";
                    }
                }
            }
            static public void UpdateLog()
            {
                if (Enabled)
                {
                    Logger.Invoke((Action)delegate
                    {
                        lock (Text)
                        {
                            Logger.Text += $"{Text}";
                            Text = string.Empty;
                        }
                    });

                    Logger.Invoke((Action)delegate
                    {
                        try
                        {
                            if (Logger.Text.Length - 1 > 0)
                            {
                                Logger.Select(Logger.Text.Length - 1, 1);
                                Logger.ScrollToCaret();
                            }
                        } 
                        catch (Exception) { }
                    });
                }
            }
            static public void Clear()
            {
                if (Enabled)
                {
                    Logger.Invoke((Action)delegate
                    {
                        lock (Text)
                        {
                            Logger.Text = string.Empty;
                            Text = string.Empty;
                        }
                    });
                }
            }
        }
    }
}
