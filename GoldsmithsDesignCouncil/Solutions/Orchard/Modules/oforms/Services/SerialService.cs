using System;
using System.IO;
using System.Web;
using Orchard.Logging;

namespace oforms.Services
{
    public class SerialService : ISerialService
    {
        private string fileName;
        private string pathDestination;

        public SerialService() {
            this.pathDestination = HttpContext.Current.Server.MapPath("~/App_Data/oforms/");
            this.fileName = Path.Combine(this.pathDestination, "sn.dat");
            Logger = NullLogger.Instance;
        }

        public ILogger Logger {get;set;}

        public void SaveSerialToFile(string value) {
            this.CreateMissingDirectory();
            TextWriter tw = new StreamWriter(this.fileName);
            tw.WriteLine(value);
            tw.Close();
        }

        private void CreateMissingDirectory() {
            if (!Directory.Exists(this.pathDestination))
            {
                Directory.CreateDirectory(this.pathDestination);
            }
        }

        public string ReadSerialFromFile() {
            string text;
            try {
                text = File.ReadAllText(this.fileName);
            }
            catch (Exception ex)
            {
            	Logger.Information(ex, "Can not read file.");
                return "";
            }
            return text.Trim();
        }

        private string DecodeSerial(string str)
        {
            try
            {
                // ignore 3st 3 letters
                str = str.Substring(3, str.Length - 3);
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch (Exception ex)
            {
            	Logger.Information(ex, "Can not decode serial");
                return "";
            }

        }

        private static bool IsNumeric(string s)
        {
            double Result;
            return double.TryParse(s, out Result);  // TryParse routines were added in Framework version 2.0.
        } 


        public bool ValidateSerial() {
            if (IsNumeric(DecodeSerial(this.ReadSerialFromFile())))
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}