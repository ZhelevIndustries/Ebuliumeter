using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Ebuiliumeter
{
    public partial class uc3 : UserControl
    {
        public string lang { get; set; }
        public uc3()
        {
            InitializeComponent();
        }

        private void uc3_Load(object sender, EventArgs e)
        {
            lang = User.FindLang();
            comboBox1.Text = lang;
            label2.Text = User.SetLanguage(lang, "Language", "Език");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lang = comboBox1.Text;
            string[] arrLine = File.ReadAllLines(@"settings.txt");
            arrLine[1] = comboBox1.Text;
            File.WriteAllLines(@"settings.txt", arrLine);
        }
    }
}
