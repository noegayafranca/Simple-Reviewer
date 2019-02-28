using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reviewer_v._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                lbfolder.Text = fbd.SelectedPath;
                menucombobox.Items.Clear();
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                string[] dirs = Directory.GetDirectories(fbd.SelectedPath);
                Properties.Settings.Default.foldersettings = lbfolder.Text;
                Properties.Settings.Default.Save();

                foreach (string item2 in dirs)
                {
                    FileInfo f = new FileInfo(item2);

                    menucombobox.Items.Add(f.Name);

                }
            }
        }

        public void ShowAllFolders()
        {
           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(menucombobox.Text == "")
            {
                MsgboxError("Select folder where you want to save your data.");
                menucombobox.Focus();
            }
            else if(textBox1.Text == "")
            {
                MsgboxError("Enter filename.");
                textBox1.Focus();
            }
            else if(richTextBox1.Text == "")
            {
                MsgboxError("Field is empty, nothing to save.");
                richTextBox1.Focus();
            }
            else
            {
                if(review.Text == "ON")
                {
                    SaveFile();
                    MsgboxInfo("Successfully Updated.");
                    richTextBox1.Clear();
                    textBox1.Clear();
                }
                else
                {
                    if (System.IO.File.Exists(lbfolder.Text + "\\" + menucombobox.Text + "\\" + textBox1.Text + ".txt"))
                    {
                        MsgboxError("File already exist.");
                        textBox1.Focus();
                    }
                    else
                    {
                        SaveFile();
                        MsgboxInfo("Successfully Saved");
                        treeView1.Nodes.Add(textBox1.Text + ".txt");
                        richTextBox1.Clear();
                        Properties.Settings.Default.lastsave = textBox1.Text;
                        Properties.Settings.Default.Save();
                        history.Text = Properties.Settings.Default.lastsave;
                        textBox1.Clear();                   
                    }
                }
            }
        }

        public void SaveFile()
        {
            try
            {
                StreamWriter str = new StreamWriter(lbfolder.Text + "\\" + menucombobox.Text + "\\" + textBox1.Text + ".txt");
                str.WriteLine(richTextBox1.Text);
                str.Close();
            }
            catch(Exception ex)
            {
                MsgboxError(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            //int i;
            //for (i = 1; i < 100; i++)
            //{
            //    richTextBox1.Text += i + "\n";
            //}
        }

        public void MsgboxError(string message)
        {
            MessageBox.Show(message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void MsgboxInfo(string message)
        {
            MessageBox.Show(message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           if(review.Text == "ON")
            {
                treeView1.SelectedNode.Remove();
                textBox1.Text = e.Node.Text;
                string text = textBox1.Text;
                textBox1.Text = text.Replace(".txt", "");
                lbquestion.Text = text.Replace(".txt", " ?");
                StreamReader str = new StreamReader(lbfolder.Text + "\\" + menucombobox.Text + "\\" + e.Node.Text);
                richTextBox1.Text = str.ReadToEnd();
                str.Close();
                save.Text = "UPDATE";
                panel1.Show();
            }
            else
            {

            }
        }

        private void reviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(review.Text == "OFF")
            {
                review.Text = "ON";
            }
            else
            {
                review.Text = "OFF";
                save.Text = "Save";
            }
        }

        private void menucombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            DirectoryInfo d = new DirectoryInfo(lbfolder.Text+"\\"+menucombobox.Text);
            FileInfo[] Files = d.GetFiles("*.txt");       
            foreach (FileInfo file in Files)
            {          
                treeView1.Nodes.Add(file.Name);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            textBox1.Clear();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(paste.Text == "OFF")
            {
                paste.Text = "On";
            }
            else
            {
                paste.Text = "OFF";
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (paste.Text == "On")
            {
                textBox1.Paste();
                timer1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            history.Text = Properties.Settings.Default.lastsave;

            lbfolder.Text = Properties.Settings.Default.foldersettings;

            
            try
            {
                string[] dirs = Directory.GetDirectories(lbfolder.Text);
                foreach (string item2 in dirs)
                {
                    FileInfo f = new FileInfo(item2);

                    menucombobox.Items.Add(f.Name);

                }
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            textBox1.Text = str.Replace("()", "");          
            timer1.Enabled = false;
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            e.Node.ToolTipText = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(review.Text == "ON")
            {

            }
            else
            {
                string full = lbfolder.Text + "\\" + menucombobox.Text + "\\" + textBox1.Text + ".txt";
                if (System.IO.File.Exists(full))
                {
                    MsgboxError("Filename Already Exist.");
                }
            }
        }
    }
}
