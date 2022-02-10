using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChiuMartSAIS2.Classes
{
    class functions
    {
        public static bool isMac(string mac)
        {
            if (mac == Classes.functions.GetMacAddress())
            { return true; }
            return false;
        }
        public static string GetMacAddress()
        {
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BIOS");
                foreach (ManagementObject getserial in MOS.Get())
                {
                    return getserial["SerialNumber"].ToString();
                }
            }
            catch (Exception) { }
            return "";
        }
        public bool checkspecial(string name)
        {
            var regexItem = new Regex("^[a-zA-Z0-9_@()/' ]*$");

            if (regexItem.IsMatch(name)) { return true; }
            else { return false; }
        }
        public string RemoveCurrency(string curr)
        {
            curr = curr.Replace("Rs.", "");
            //curr = curr.Replace("$", "");
            return curr;

        }
        public string RemoveCurrencyCheckOut(string curr)
        {
            if (curr.Contains("$"))
            {
                curr = curr.Replace("$", "");
            }
            else
            {
                curr = curr.Replace("Rs", "");
            }
            return curr;

        }

        //public string CheckValidDate(string date)
        //{
        //    curr = curr.Replace("Rs.", "");
        //    curr = curr.Replace("$", "");
        //    return curr;

        //}

        public string randomBarcode()
        {
            string val = "";
            dbHelper objhelp = new dbHelper();
            do
            {

                val = "";
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomNumber(4, 100000));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomNumber(2000, 10000000));
                val = builder.ToString();
            }
            while (objhelp.checkbarcode(val));

            return val;

        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static void WriteEncryptedFile(string filename, string data)
        {
            File.WriteAllText(filename, "");
            using (StreamWriter _streamWriter = new StreamWriter(filename, true))
            {
                _streamWriter.Write(new Crypto().Encrypt(data));
            }
        }
        public static string ReadEncryptedfile(string filename)
        {
            using (StreamReader _streamReader = new StreamReader(filename))
            {
                return new Crypto().Decrypt(_streamReader.ReadToEnd());
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
    class Crypto
    {
        byte[] _key { get; set; }
        byte[] _iv { get; set; }

        public Crypto()
        {
            _key = Encoding.Default.GetBytes("ABCDEFGHIJKLMNOP");
            _iv = Encoding.Default.GetBytes("ABCDEFGHIJKLMNOP");
        }

        public string Encrypt(string data)
        {
            using (ICryptoTransform _iCrypto = new TripleDESCryptoServiceProvider().CreateEncryptor(_key, _iv))
            {
                var _byteData = Encoding.Default.GetBytes(data);
                var _encryptedData = _iCrypto.TransformFinalBlock(_byteData, 0, _byteData.Length);
                return Convert.ToBase64String(_encryptedData, 0, _encryptedData.Length);
            }
        }
        public string Decrypt(string data)
        {
            using (ICryptoTransform _iCrypto = new TripleDESCryptoServiceProvider().CreateDecryptor(_key, _iv))
            {
                var _byteData = Convert.FromBase64String(data);
                var _decryptedData = _iCrypto.TransformFinalBlock(_byteData, 0, _byteData.Length);
                return Encoding.Default.GetString(_decryptedData);
            }
        }
    }
}
