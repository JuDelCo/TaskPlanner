using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace TaskPlanner
{
    class EncriptarMD5
    {
        public EncriptarMD5() {}

        public static string Encript(string txt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] CHash = md5.ComputeHash(ASCIIEncoding.Default.GetBytes(txt));
            string cripted = "";
            foreach (byte i in CHash)
            {
                cripted += i.ToString("x2");
            }
            return cripted; //32
        }
    }
}
