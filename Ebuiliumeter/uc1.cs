using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;



namespace Ebuiliumeter
{
    public partial class uc1 : UserControl
    {
        byte[] auto = new byte[3] { 0x45, 0x53, 0x50 }; // baitove za avtomatichno namirane na ebuliumetyra
        byte[] start = new byte[5] { 0x53, 0x54, 0x41, 0x52, 0x54 }; // bytove za startirane na ebuiliumetyra
        byte[] stop = new byte[4] { 0x53, 0x54, 0x4F, 0x50 }; // bytove za spirane na ebuliometyra
        byte[] clean = new byte[5] { 0x43, 0x4C, 0x45, 0x41, 0x4E }; // bytove za chistene
        public bool ebuliumeter; 
        byte[] prb = new byte[100];
        int bytestoread = 0; // promenliva za kolko baita sa nalichni za prochitane
        bool connected = false; // promenliva kazvashta dali sme svyrzani s ustroistvoto
        bool running = true;
        int time = 0;
        //bool cleanable = false;
        //double pressure = 0;
        //double alcohol = 0;

        public string lang { get; set; } // pokazva koi ezik se polzva

        public void SerialConnect(string port, int baudrate, string stopbits, string paritybits)
        {
            serialPort1.PortName = port; // zadavane na port
            serialPort1.BaudRate = baudrate; // zadavane na baudrate
            //zadavane na stop bitove
            if (stopbits == "One") { serialPort1.StopBits = StopBits.One; }
            //zadavane na parity bitove
            if (paritybits == "None") { serialPort1.Parity = Parity.None; }
            serialPort1.DataBits = 8; // zadavane na data bitove
            serialPort1.Open(); // otvarqne na seriiniq port
           // serialPort1.ReadTimeout = 1000;
           // serialPort1.WriteTimeout = 1000;
        }

        public uc1()
        {
            InitializeComponent();
        }

        private void uc1_Load(object sender, EventArgs e)
        {
            string[] COM_ports = SerialPort.GetPortNames(); // vzimane na spisyk s vsichki nalichni COM portove

            for (int i = 0; i < COM_ports.Length; i++)
            {
                comboBox1.Items.Add(COM_ports[i]); // dobavqne na COM portovete v combobox1
            }

            //pyrvonachalno izklyuchvane na butonite
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button5.Visible = true;
            button7.Enabled = true;

            ebuliumeter = false;

            //syzdavane na prazna grafika
            int length = 75;
            double[] x = new double[length];
            double[] y = new double[length];
            for (int i = 0; i < length; i++)
            {
                x[i] = i+1;
                y[i] = 0;
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
            //dataGridView1.Columns[0].HeaderCell.Value = User.SetLanguage(this.lang, "Container", "Контейнер");
        }

        private void uc1_VisibleChanged(object sender, EventArgs e)
        {
            // zadavane na ezika na otdelnite komponenti
            this.lang = User.FindLang();
            label2.Text = User.SetLanguage(this.lang, "COM Port", "COM порт");
            label3.Text = User.SetLanguage(this.lang, "Baud Rate", "Baud Rate");
            label4.Text = User.SetLanguage(this.lang, "Parity bits", "Парити бит");
            label5.Text = User.SetLanguage(this.lang, "Stop bits", "Stop bits");
            label6.Text = User.SetLanguage(this.lang, "Data bits", "Data bits");
            label7.Text = User.SetLanguage(this.lang, "Container", "Контейнер");
            label8.Text = User.SetLanguage(this.lang, "Wine", "Вино");
            button1.Text = User.SetLanguage(this.lang, "Auto Find", "Автоматично намиране");
            button2.Text = User.SetLanguage(this.lang, "Connect", "Свързване");
            button3.Text = User.SetLanguage(this.lang, "Disconnect", "Разкачване");
            button4.Text = User.SetLanguage(this.lang, "Start", "Започване");
            button5.Text = User.SetLanguage(this.lang, "Stop", "Спиране");
            button6.Text = User.SetLanguage(this.lang, "Print", "Принтиране");
            button7.Text = User.SetLanguage(this.lang, "Clean", "Почистване");
            comboBox2.Text = "9600";
            comboBox3.Text = "8";
            comboBox4.Text = "None";
            comboBox5.Text = "One";
            dataGridView1.Columns[0].HeaderCell.Value = User.SetLanguage(this.lang, "Container", "Контейнер");
            dataGridView1.Columns[1].HeaderCell.Value = User.SetLanguage(this.lang, "Date", "Дата");
            dataGridView1.Columns[2].HeaderCell.Value = User.SetLanguage(this.lang, "Wine", "Вино");
            dataGridView1.Columns[3].HeaderCell.Value = User.SetLanguage(this.lang, "Alcohol", "Алкохол");

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            //zarejdane na zapisanite danni v tablicata
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

                dataGridView1.Rows.Add(s); // dobavqne na veche zapisani danni v tablicata
            }
            r.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //namirane na ebuliometyr avtomatichno
            foreach(var port in SerialPort.GetPortNames())
            {
                bytestoread = 0;
                SerialConnect(port, 9600, "One", "None");
                serialPort1.Write(auto, 0, auto.Length); // izprashvane na zapitvane kym ustroistvoto
                serialPort1.DiscardInBuffer();

                await PutTaskDelay();
                // prochitane na izprateni danni
                try
                {
                    bytestoread = serialPort1.BytesToRead;
                }
                catch (InvalidOperationException) { }
                label1.Text = bytestoread.ToString();
                if (bytestoread != 0)
                {
                    byte[] b = new byte[bytestoread];
                    serialPort1.Read(b, 0, bytestoread);
                    string str = Encoding.Default.GetString(b);
                    label1.Text = str;
                    if (str == "APARAT") { comboBox1.Text = port; serialPort1.Close(); break; }
                }
                serialPort1.Close();
            }

        }
        async Task PutTaskDelay()
        {
            await Task.Delay(2000); // asinhronichno chakane
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //Svyrzvane s ustroistvoto po serien port
            try
            {
                if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "" && comboBox5.Text != "")
                {
                    bytestoread = 0;
                    SerialConnect(comboBox1.Text, int.Parse(comboBox2.Text), comboBox5.Text, comboBox4.Text);
                    serialPort1.Write(auto, 0, auto.Length);
                    serialPort1.DiscardInBuffer();

                    await PutTaskDelay();
                    try
                    {
                        bytestoread = serialPort1.BytesToRead;
                    }
                    catch (InvalidOperationException) { }
                    label1.Text = bytestoread.ToString();
                    if (bytestoread != 0)
                    {
                        byte[] b = new byte[bytestoread];
                        serialPort1.Read(b, 0, bytestoread);
                        string str = Encoding.Default.GetString(b);
                        label1.Text = str;
                        if (str == "APARAT") { label1.Text = "connected"; connected = true; button4.Enabled = true; }
                        else
                        {
                            connected = false;
                            serialPort1.Close();
                            // syobshtenie v sluchai che ustroistvo ne e namereno
                            MessageBox.Show(User.SetLanguage(lang, "There is no ebuliumeter on that COM port", "Няма ебулиуметър на този COM порт"), User.SetLanguage(lang, "Error", "Грешка"));
                        }
                    }
                    else
                    {
                        connected = false;
                        serialPort1.Close();
                        // syobshtenie v sluchai che ustroistvo ne e namereno
                        MessageBox.Show(User.SetLanguage(lang, "There is no ebuliumeter on that COM port", "Няма ебулиуметър на този COM порт"), User.SetLanguage(lang, "Error", "Грешка"));
                    }
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Unauthorized Access");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // otvyrzvane ot COM porta
            if (serialPort1.IsOpen)
            {
                connected = false;
                serialPort1.Close();
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            running = true;
            button7.Enabled = true;
            int length = 75;
            double[] x = new double[length];
            double[] y = new double[length];
            LabData l = new LabData();
            // startirane na izmervaniqta
            if (serialPort1.IsOpen && connected)
            {
                // syzdavane na prazna grafika
                for (int i = 0; i < length; i++)
                {
                    x[i] = i+1;
                    y[i] = 0;
                    chart1.Series[0].Points.AddXY(x[i], y[i]);
                }
                bytestoread = 0;
                serialPort1.Write(start, 0, start.Length); // izorashtane kym ESP32 da zapochne izmervaniqta
                serialPort1.DiscardInBuffer();

                await PutTaskDelay();
                try
                {
                    bytestoread = serialPort1.BytesToRead;
                }
                catch (InvalidOperationException) { }
                if (bytestoread != 0)
                {
                    byte[] b = new byte[bytestoread];
                    serialPort1.Read(b, 0, bytestoread); // prochitane na dannite prateni po seriiniq port
                    string str = Encoding.Default.GetString(b);
                    label1.Text = str;
                    //pressure = User.GetPressure(str);
                    l.Pressure = str;
                }
                await PutTaskDelay();
                while (running) // vzimane na dannite ot temperaturata na vinoto
                {
                    time += 1;
                    try
                    {
                        bytestoread = serialPort1.BytesToRead;
                    }
                    catch (InvalidOperationException) { }
                    if (bytestoread != 0)
                    {
                        byte[] b = new byte[bytestoread];
                        serialPort1.Read(b, 0, bytestoread); // prochitane na izpratenite danni
                        
                        string str = Encoding.Default.GetString(b);
                        label1.Text = str;

                        for (int i = 0; i < bytestoread / 5; i++) // vzimane na izpratenite danni za temperaturata
                        {
                            chart1.Series[0].Points.Clear(); // izchistvane na tochkite na grafikata
                            for (int j = 0; j < y.Length - 1; j++) // syzdavane na novi tochki na grafikata
                            {
                                y[j] = y[j + 1];
                                chart1.Series[0].Points.AddXY(x[j], y[j]);
                            }
                            y[y.Length - 1] = double.Parse(str.Substring(5*i, 5));
                            chart1.Series[0].Points.AddXY(x[x.Length - 1], y[y.Length - 1]);
                        }

                        if (str.Substring(str.Length - 4) == "Done") // spirane na rejim vzimane na izmervaniq za temperaturata
                        {
                            //serialPort1.Close();
                            running = false;
                            //break;
                        }

                        if (str.Contains("Done"))
                        {
                            running = false;
                        }

                        await PutTaskDelay();
                    }
                }
                await PutTaskDelay();
                try
                {
                    bytestoread = serialPort1.BytesToRead;
                }
                catch (InvalidOperationException) { }
                if (bytestoread != 0)
                {
                    byte[] b = new byte[bytestoread];
                    serialPort1.Read(b, 0, bytestoread);
                    string str = Encoding.Default.GetString(b);
                    label1.Text = str;
                    //alcohol = User.GetAlcohol(str);
                    l.Alcohol = str; // vzimane na alkoholniq procent
                }
            }
            serialPort1.Close(); // zatvarqne na serien port 1
            
            l.Container = textBox1.Text;
            l.Wine = textBox2.Text;
            l.Date = DateTime.UtcNow.ToString("MM.dd.yyyy");
            l.Temperature = y[y.Length - 1].ToString();

            // zapisvane na dannite v JSON file
            string serialized = JsonConvert.SerializeObject(l);

            StreamWriter sw = new StreamWriter(@"labdata.json", append: true);
            await sw.WriteLineAsync(serialized);
            sw.Close();

            StreamWriter graph_data = new StreamWriter(textBox1.Text + ".json");
            string data;
            int x_p = 0;
            for (int i = 0; i < 75; i++)
            {
                x_p = time - (75 - i);
                data = "{x: \"" + x_p.ToString() + "\", y: \"" + y[i].ToString() + "\"}";
                await graph_data.WriteLineAsync(data);
            }

            button5.Enabled = false;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            //Stop
            running = false;
            serialPort1.Write(stop, 0, stop.Length); // izprashvane na zapitvane kym ustroistvoto
            serialPort1.DiscardInBuffer();
            await PutTaskDelay();
            serialPort1.Close();
            button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Print
            Form2 f2 = new Form2("lab", this.lang);
            f2.ShowDialog(); // otvarqne na otdelna forma s opcii za printirane 
        }


        //Functionallity Based  
        private void serialPort1_DataReceived(object sender, EventArgs e)
        { }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        //GUI based
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "" && comboBox5.Text != "")
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "" && comboBox5.Text != "")
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "" && comboBox5.Text != "")
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox4.Text != "" && comboBox5.Text != "")
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "") { button4.Enabled = true; }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        { 
            //zarejdane na grafika pri natiskane dva pyti vyrhiu syovetniq red na tablicata
            try
            {
                string filename = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + ".json";
                StreamReader r = new StreamReader(filename);
                string line;
                Point p;
                string[] s = new string[5];

                //izchistvane na grafikata dosega
                chart1.Series["Series1"].Points.Clear();

                while ((line = r.ReadLine()) != null)
                {
                    p = JsonConvert.DeserializeObject<Point>(line);

                    // dobavqne na veche zapisani danni v tablicata
                    chart1.Series[0].Points.AddXY(p.x, p.y);
                }
                r.Close();
            }
            catch (IOException)
            { }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(User.SetLanguage(this.lang, "Automatic search for Electronic Ebuliometer", "Автоматично търсене на Електронен Ебулиометър"), button1);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(User.SetLanguage(this.lang, "Connecting with Electronic Ebuliometer through the chosen serial port", "Свързване с Електронен Ебулиометър по зададения сериен порт"), button2);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(User.SetLanguage(this.lang, "Disconnecting with Electronic Ebuliometer through the serial port", "Прекъсване на връзката с Електронен Ебулиометър по серийния порт"), button3);

        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(User.SetLanguage(this.lang, "Conducting", "Започване на измерване"), button4);
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(User.SetLanguage(this.lang, "Printing results obtained from measurements", "Изготвяне и принтиране на справка"), button6);
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            //SerialConnect(comboBox1.SelectedItem.ToString(), 9600, "One", "None");
            serialPort1.Write(clean, 0, clean.Length); // izprashvane na zapitvane kym ustroistvoto
            serialPort1.DiscardInBuffer();
            await PutTaskDelay();
            serialPort1.Close();
            button7.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "") { button4.Enabled = true; }
        }
    }
}
