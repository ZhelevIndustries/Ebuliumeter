using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Ebuiliumeter
{
    public partial class Form3 : Form
    {
        public string Selected { get; set; }
        public Form3(string m, string l)
        {
            InitializeComponent();

            radioButton1.Text = User.SetLanguage(l, "Custom Select", "Избиране по избор");
            radioButton2.Text = User.SetLanguage(l, "Select all", "Избиране на всички");

            radioButton1.Checked = true;

            string file;

            if (m == "lab") { file = "labdata.json"; }
            else { file = "clientdata.json"; }

            StreamReader r = new StreamReader(file);
            string line;
            LabData labdata = new LabData();

            while ((line = r.ReadLine()) != null)
            {
                labdata = JsonConvert.DeserializeObject<LabData>(line);
                checkedListBox1.Items.Add(labdata.Container);
            }
            r.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    this.Selected += checkedListBox1.Items[i].ToString().Replace(' ', '_') + " ";
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
