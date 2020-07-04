using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace ImageCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try { textBox1.Text = File.ReadAllText("list.log"); } catch { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            working = true;
            label1.Text = "Converting..";
            Task t = new Task(()=>{
                
                while (working)
                {
                    try
                    {
                        foreach (var folder in textBox1.Lines)
                        {
                            try
                            {
                                foreach (string file in Directory.GetFiles(folder))
                                {
                                    try
                                    {
                                        FileInfo info = new FileInfo(file);
                                        if (!IsFileLocked(info))
                                            Program.Convert(file);
                                    }
                                    catch { Program.log("Job failed for " + file); }
                                }
                            }
                            catch(Exception exp) {
                                Program.log(exp.Message,"Job failed for dir "+folder);
                            }
                        }
                    }
                    catch (Exception ex){
                        Program.log(ex.Message,"Job failed for folders");
                    }
                    Thread.Sleep(1000);
                }
            });
            t.Start();
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        bool working = false;

        private void button2_Click(object sender, EventArgs e)
        {
            working = false;
            label1.Text = "Stop.";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try {
                File.WriteAllText("list.log",textBox1.Text);
            } catch { }
        }
    }
}
