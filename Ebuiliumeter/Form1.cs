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
using rtChart;
using System.Diagnostics;

namespace Ebuiliumeter
{
    public partial class Form1 : Form
    {
        public string language { get; set; }
        //Functions
        public void SetActive(UserControl uc)
        {
            uc11.Visible = false;
            uc21.Visible = false;
            uc31.Visible = false;
            uc41.Visible = false;

            uc.Visible = true;
        }


        //App
        public Form1()
        {
            InitializeComponent();
        }

        /*WebClient wc = new WebClient();
        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Hope");
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            /*wc.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
            Uri url = new Uri("https://drive.google.com/file/d/1WNZ5bK_kTK9woXMgwwRAVs_XgMXbGnnQ/view?usp=sharing");
            wc.DownloadFileAsync(url, @"user.json");*/
            this.Text = "Ebuliometer";
            SetActive(uc11);
            button1.BackColor = Color.SteelBlue;
            button2.BackColor = Color.SteelBlue;
            button3.BackColor = Color.SteelBlue;
            button4.BackColor = Color.SteelBlue;
            button1.ForeColor = Color.White;
            button2.ForeColor = Color.White;
            button3.ForeColor = Color.White;
            button4.ForeColor = Color.White;
            button1.TabStop = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button2.TabStop = false;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button3.TabStop = false;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button4.TabStop = false;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;

            if (File.Exists(@"settings.txt"))
            {
                StreamReader r = new StreamReader(@"settings.txt");
                string[] line = new string[3];

                line[0] = r.ReadLine();
                line[1] = r.ReadLine();
                line[2] = r.ReadLine();

                language = line[1];

                if (line[0] == "Yes")
                {
                    if (Cryptography.SearchUser(line[2])) { button4.Text = User.SetLanguage(language, "Sign Out", "Излезане"); }
                    else { button4.Text = User.SetLanguage(language, "Register/Log In", "Регистриране/Влизане"); }
                }
                else { button4.Text = User.SetLanguage(language, "Register/Log In", "Регистриране/Влизане"); }

                r.Close();
            }
            else
            {
                language = "en";
                StreamWriter w = new StreamWriter(@"settings.txt");

                w.WriteLine("No");
                w.WriteLine("en");
                w.WriteLine("a");
                w.Close();
            }

            button1.Text = User.SetLanguage(language, "Lab Specific Testing", "Лабораторно Тестване");
            button2.Text = User.SetLanguage(language, "Public Testing", "Публично Тестване");
            button3.Text = User.SetLanguage(language, "Settings", "Настройки");
        }

        public void SetBackground()
        {
            button1.Text = User.SetLanguage(language, "Lab Specific Testing", "Лабораторно Тестване");
            button2.Text = User.SetLanguage(language, "Public Testing", "Публично Тестване");
            button3.Text = User.SetLanguage(language, "Settings", "Настройки");
            StreamReader r = new StreamReader(@"settings.txt");
            string[] line = new string[3];

            line[0] = r.ReadLine();
            line[1] = r.ReadLine();
            line[2] = r.ReadLine();

            language = line[1];

            if (line[0] == "Yes")
            {
                if (Cryptography.SearchUser(line[2])) { button4.Text = User.SetLanguage(language, "Sign Out", "Излезане"); }
                else { button4.Text = User.SetLanguage(language, "Register/Log In", "Регистриране/Влизане"); }
            }
            else { button4.Text = User.SetLanguage(language, "Register/Log In", "Регистриране/Влизане"); }

            r.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            language = User.FindLang();
            SetActive(uc11);
            SetBackground();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            language = User.FindLang();
            SetActive(uc21);
            SetBackground();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            language = User.FindLang();
            SetActive(uc31);
            SetBackground();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            language = User.FindLang();
            if (button4.Text == User.SetLanguage(language, "Register/Log In", "Регистриране/Влизане"))
            {
                SetActive(uc41);
            }
            else
            {
                File.Delete(@"settings.json");
                StreamWriter w = new StreamWriter(@"settings.txt");

                w.WriteLine("No");
                w.WriteLine("en");
                w.WriteLine("a");
                w.Close();
            }
            SetBackground();
        }

        private void button1_MouseHover(object sender, EventArgs e) { button1.BackColor = Color.LightSteelBlue; }
        private void button1_MouseLeave(object sender, EventArgs e) { button1.BackColor = Color.SteelBlue; }
        private void button2_MouseHover_1(object sender, EventArgs e) { button2.BackColor = Color.LightSteelBlue; }
        private void button2_MouseLeave(object sender, EventArgs e) { button2.BackColor = Color.SteelBlue; }
        private void button3_MouseHover(object sender, EventArgs e) { button3.BackColor = Color.LightSteelBlue; }
        private void button3_MouseLeave(object sender, EventArgs e) { button3.BackColor = Color.SteelBlue; }
        private void button4_MouseHover(object sender, EventArgs e) { button4.BackColor = Color.LightSteelBlue; }
        private void button4_MouseLeave(object sender, EventArgs e) { button4.BackColor = Color.SteelBlue; }
        private void button1_MouseEnter(object sender, EventArgs e) { button1.BackColor = Color.LightSteelBlue; }
        private void button2_MouseEnter(object sender, EventArgs e) { button2.BackColor = Color.LightSteelBlue; }
        private void button3_MouseEnter(object sender, EventArgs e) { button3.BackColor = Color.LightSteelBlue; }
        private void button4_MouseEnter(object sender, EventArgs e) { button4.BackColor = Color.LightSteelBlue; }
    }
}
