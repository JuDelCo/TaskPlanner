using System;
using System.Net;
using System.Text;

namespace TaskPlanner
{
    class ObtenerIP
    {
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        public static IPAddress ExternalIPAddress
        {
            get
            {
                string whatIsMyIp = "http://www.whatismyip.com/automation/n09230945.asp";
                WebClient wc = new WebClient();
                string response = utf8.GetString(wc.DownloadData(whatIsMyIp));
                IPAddress myIPAddress = IPAddress.Parse(response);

                return myIPAddress;
            }
        }
    }
}