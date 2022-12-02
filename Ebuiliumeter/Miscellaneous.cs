using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace Ebuiliumeter
{ 
    public class User
    {
        public User() { }

        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public static string SetLanguage(string lang, string en, string bg)
        {
            if (lang == "en") { return en; }
            if (lang == "bg") { return bg; }
            return en;
        }

        public static string FindLang()
        {
            StreamReader r = new StreamReader(@"settings.txt");
            r.ReadLine();
            string l2 = r.ReadLine();
            r.Close();
            if (l2 == "en") { return "en"; }
            else if (l2 == "bg") { return "bg"; }
            
            return "en";
        }

        public static double GetPressure(string s)
        {
            return s[0] * 1000 + s[1] * 100 + s[2] * 10 + s[3] + s[4] / 10 + s[5] / 100;
        }

        public static double GetAlcohol(string s)
        {
            return s[0] * 10 + s[1] + s[2] / 10 + s[3] / 100;
        }
    }

    public class LabData
    {
        public string Container;
        public string Date;
        public string Wine;
        public string Alcohol;
        public string Pressure;
        public string Temperature;
    }

    public class Cryptography
    {
        public static bool Email_Password_Check(string pwd, string email)
        {
            StreamReader r = new StreamReader(@"user.json");
            string line;
            bool flag = true;
            User deserialized;

            while ((line = r.ReadLine()) != null)
            {
                deserialized = JsonConvert.DeserializeObject<User>(line);
                Console.WriteLine(deserialized.Email);
                if (deserialized.Email == email || deserialized.Password == pwd) { flag = false; break; }
            }

            r.Close();
            return flag;
        }

        public static bool LogInData(string pwd, string email)
        {
            StreamReader r = new StreamReader(@"user.json");
            bool flag = false;
            string line;
            User deserialized;

            while ((line = r.ReadLine()) != null)
            {
                deserialized = JsonConvert.DeserializeObject<User>(line);
                if (deserialized.Email == email && deserialized.Password == pwd) { flag = true; break; }
            }

            r.Close();
            return flag;
        }

        public static bool SearchUser(string email)
        {
            StreamReader r = new StreamReader(@"user.json");
            bool flag = false;
            string line;
            User deserialized;

            while ((line = r.ReadLine()) != null)
            {
                deserialized = JsonConvert.DeserializeObject<User>(line);
                if (deserialized.Email == email) { flag = true; break; }
            }

            r.Close();
            return flag;
        }

        // The following function is not my original code
        // The auther of the code as well as the code can be found at
        // https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class Point
    {
        public double x;
        public double y;
    }

    public class Temperature
    {
        public double Calc_delta_temp(double p) { return 1/ (1 / this.T_w - (this.R * Math.Log(p / this.p_0)) / this.latent_heat); }

        private double p_0 = 1013.25;
        private double R = 8.314;
        private double T_w = 100;
        private double latent_heat = 2960;
    }
}
