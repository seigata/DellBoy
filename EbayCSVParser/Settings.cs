using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbayCSVParser
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

        }

        private void Settings_Load(object sender, EventArgs e)
        {

            ParserSettings ps = Properties.Settings.Default.ParseSettings;

            if(ps == null)
            {
                ps = new ParserSettings();
            }


            chkIntLabels.Checked = ps.CreateForAllInternational;
            textBox1.Text = ps.Category;
        }
        

        public ParserSettings GetSettings()
        {
            ParserSettings ps = new ParserSettings();

            ps.Category = textBox1.Text;
            ps.CreateForAllInternational = chkIntLabels.Checked;

            return ps;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ParseSettings = GetSettings();
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
