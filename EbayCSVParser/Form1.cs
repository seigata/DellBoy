using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbayCSVParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtDispatchedDate.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInputFolder.Text) || string.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show("Select a input and output file");
            }
            else
            {
                string error = "";
                try
                {
                    error = Parser.ConvertReports(txtInputFolder.Text, txtOutput.Text, dtDispatchedDate.Value);
                }
                catch(Exception ex)
                {
                    error = "Failed to convert, error message:" + ex.Message;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error);
                }
                else
                {
                    MessageBox.Show("File Converted!");
                }
            }
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            browser.Filter = "CSV Files (*.csv)|*.csv";
            browser.DefaultExt = "csv";
            browser.Multiselect = false;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                txtInputFolder.Text = browser.FileName;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.DefaultExt = "csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = sfd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hi Jade");
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(files.Length > 0)
            {
                txtInputFolder.Text = files[0];

                txtOutput.Text = Path.Combine(Path.GetDirectoryName(files[0]), Path.GetFileNameWithoutExtension(files[0]) + "_out.csv");
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void txtInputFolder_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();

            settingsForm.ShowDialog();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
