using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace Ebuiliumeter
{
    public partial class Form2 : Form
    {
        private string mode;
        private string lang;
        private DataGridView dgv = new DataGridView();
        private string selected_items;

        public Form2(string m, string l)
        {
            InitializeComponent();
            this.lang = l;

            checkBox1.Text = User.SetLanguage(this.lang, "Probe Name", "Име на Пробата");
            checkBox2.Text = User.SetLanguage(this.lang, "Wine", "Вино");
            checkBox3.Text = User.SetLanguage(this.lang, "Alcohol %", "Алкохолно Съдържание");
            checkBox4.Text = User.SetLanguage(this.lang, "Chart", "Графика");
            checkBox5.Text = User.SetLanguage(this.lang, "Pressure", "Налягане");
            checkBox6.Text = User.SetLanguage(this.lang, "Temperature", "Температура");
            button1.Text = User.SetLanguage(this.lang, "Print", "Принтиране");
            button2.Text = User.SetLanguage(this.lang, "Select Probes", "Избиране на Проби");

            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;

            chart1.Visible = false;

            mode = m;

            if (m == "lab")
            {
                dataGridView1.Columns[0].HeaderCell.Value = User.SetLanguage(this.lang, "Container", "Контейнер");
                dataGridView1.Columns[1].HeaderCell.Value = User.SetLanguage(this.lang, "Neshto", "Нещо");
                dataGridView1.Columns[2].HeaderCell.Value = User.SetLanguage(this.lang, "Wine", "Вино");
                dataGridView1.Columns[3].HeaderCell.Value = User.SetLanguage(this.lang, "Alcohol", "Алкохол");

                StreamReader r = new StreamReader(@"labdata.json");
                string line;
                LabData deserialized;
                string[] s = new string[5];

                while ((line = r.ReadLine()) != null)
                {
                    deserialized = JsonConvert.DeserializeObject<LabData>(line);
                    s[0] = deserialized.Container;
                    s[1] = deserialized.Date;
                    s[2] = deserialized.Wine;
                    s[3] = deserialized.Alcohol;
                    s[4] = deserialized.Pressure;

                    dataGridView1.Rows.Add(s);
                }
                r.Close();
            }
            else {  }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DGV
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            string[] items = this.selected_items.Split();
            string file;
            int height = 100;

            StreamReader r = new StreamReader(@"labdata.json");
            string line;
            LabData deserialized;

            e.Graphics.DrawString(User.SetLanguage(this.lang, "Electronic Ebulliometer", "Електронен Ебулиометър"), new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Black, new PointF(230, height-50));

            while ((line = r.ReadLine()) != null)
            {
                deserialized = JsonConvert.DeserializeObject<LabData>(line);

                foreach (string item in items)
                {
                    if (item.Replace('_', ' ') == deserialized.Container)
                    {
                        e.Graphics.DrawString(User.SetLanguage(this.lang, "Results from the probe:", "Резултати от пробата:"), new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, new PointF(100, height));
                        height += 19;
                        if (checkBox1.Checked == true)
                        {
                            e.Graphics.DrawString(User.SetLanguage(this.lang, "Probe:                ", "Проба:                  ") + deserialized.Container, new Font("Times New Roman", 12), Brushes.Black, new PointF(100, height));
                            height += 45;
                        }
                        e.Graphics.DrawLine(new Pen(Color.Black, 1), 50, height, 770, height);
                        height += 45;
                        e.Graphics.DrawString(User.SetLanguage(this.lang, "Parameters of the probe:", "Параметри на пробата:"), new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Black, new PointF(100, height));
                        height += 22;
                        if (checkBox2.Checked == true)
                        {
                            e.Graphics.DrawString(User.SetLanguage(this.lang, "1. Wine:                  ", "1. Вино:                   ") + deserialized.Wine, new Font("Times New Roman", 12), Brushes.Black, new PointF(100, height));
                            height += 22;
                        }
                        if (checkBox3.Checked == true)
                        {
                            e.Graphics.DrawString(User.SetLanguage(this.lang, "2. Alcohol Content:  ", "2. Алкохолно Съдържание:   ") + deserialized.Alcohol + "%", new Font("Times New Roman", 12), Brushes.Black, new PointF(100, height));
                            height += 22;
                        }
                        if (checkBox5.Checked == true)
                        {
                            e.Graphics.DrawString(User.SetLanguage(this.lang, "3. Pressure:             ", "3. Налягане:               ") + deserialized.Pressure + " kPa", new Font("Times New Roman", 12), Brushes.Black, new PointF(100, height));
                            height += 22;
                        }
                        if (checkBox6.Checked == true)
                        {
                            e.Graphics.DrawString(User.SetLanguage(this.lang, "4. Temperature:             ", "4. Температура:               ") + deserialized.Temperature + " °C", new Font("Times New Roman", 12), Brushes.Black, new PointF(100, height));
                            height += 22;
                        }
                        height += 60;
                        if (checkBox4.Checked == true)
                        {
                            file = item.Replace('_', ' ');

                            try
                            {
                                string filename = file + ".json";
                                StreamReader read = new StreamReader(filename);
                                string li;
                                Point p;

                                while ((li = read.ReadLine()) != null)
                                {
                                    p = JsonConvert.DeserializeObject<Point>(li);

                                    // dobavqne na veche zapisani danni v tablicata
                                    chart1.Series[0].Points.AddXY(p.x, p.y);
                                }

                                read.Close();
                            }
                            catch (IOException)
                            { }

                            Bitmap b = new Bitmap(chart1.Width, chart1.Height);
                            chart1.DrawToBitmap(b, new Rectangle(0, 0, this.chart1.Width, this.chart1.Height));

                            e.Graphics.DrawImage(b, 240, height);
                        }
                        e.HasMorePages = true;
                        return;
                    }
                }
            }
            r.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Form3 SelectionOfProbes = new Form3(this.mode, this.lang))
            {
                if (SelectionOfProbes.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.selected_items = SelectionOfProbes.Selected;
                }
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
