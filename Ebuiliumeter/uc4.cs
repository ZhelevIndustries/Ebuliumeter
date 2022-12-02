using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace Ebuiliumeter
{
    public partial class uc4 : UserControl
    {
        public string lang { get; set; }

        public uc4()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label7.Text = "";
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                if (textBox2.Text.Length > 7)
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        string hashed_password = Cryptography.CreateMD5(textBox2.Text);
                        if (Cryptography.Email_Password_Check(hashed_password, textBox4.Text))
                        {
                            User u = new User()
                            {
                                CompanyName = textBox1.Text,
                                Password = hashed_password,
                                Email = textBox4.Text
                            };
                            string serialized = JsonConvert.SerializeObject(u);

                            StreamWriter sw = new StreamWriter(@"user.json", append: true);
                            await sw.WriteLineAsync(serialized);
                            sw.Close();

                            label7.Text = User.SetLanguage(lang, "You have registered successfully, now you must Log In.", "Успешно се регистрирахте, сега трябва да влезете в профила си.");
                        }
                        else { label7.Text = User.SetLanguage(lang, "The password or the email address is already used by another user.", "Паролата или email адреса са използвани вече от друг потребител."); }
                    }
                    else { label7.Text = User.SetLanguage(lang, "The password and the password confirmation must be the same.", "Парола и потвърждаването и трябва да съвпадат."); }
                }
                else { label7.Text = User.SetLanguage(lang, "The password length should be at least 8 characters.", "Паролата трябва да е поне 8 символа."); }
            }
            else { label7.Text = User.SetLanguage(lang, "All of the fields must be filled.", "Трябва да попълните полетата."); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            if (textBox5.Text != "" && textBox6.Text != "")
            {
                if (Cryptography.LogInData(Cryptography.CreateMD5(textBox6.Text), textBox5.Text))
                {
                    label8.Text = User.SetLanguage(lang, "Successful Log in", "Успешно Влизане");
                }
                else { label8.Text = User.SetLanguage(lang, "Invalid email address or password", "Грешен email адрес или парола"); }
            }
            else { label8.Text = User.SetLanguage(lang, "You must fill in the fields", "Трябва да попълните полетата"); }
        }

        private void label2_Click(object sender, EventArgs e) { }

        private void uc4_VisibleChanged(object sender, EventArgs e)
        {
            this.lang = User.FindLang();
            groupBox1.Text = User.SetLanguage(lang, "Register", "Регистрация");
            groupBox2.Text = User.SetLanguage(lang, "Log In", "Влизане");
            button1.Text = User.SetLanguage(lang, "Register", "Регистрация");
            button2.Text = User.SetLanguage(lang, "Log In", "Влизане");
            label1.Text = User.SetLanguage(lang, "Company name", "Име на компания");
            label2.Text = User.SetLanguage(lang, "Password", "Парола");
            label3.Text = User.SetLanguage(lang, "Password Confirmation", "Потвърждаване на Паролата");
            label4.Text = User.SetLanguage(lang, "Email", "Email");
            label5.Text = User.SetLanguage(lang, "Email", "Email");
            label6.Text = User.SetLanguage(lang, "Password", "");
        }
    }
}
