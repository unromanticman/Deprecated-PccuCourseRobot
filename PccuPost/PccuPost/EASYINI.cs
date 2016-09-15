using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace PccuPost
{
    public class EASYINI
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);

        private const int size = 3000;//temp file source size

        public string filePath = "";

        public EASYINI(string path)
        {

            this.filePath = path;

            try
            {
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("");
                        fs.Write(info, 0, info.Length);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }


        public string getINI(string section, string name)
        {

            string str = "";

            try
            {
                StringBuilder temp = new StringBuilder(size); //temp file source
                GetPrivateProfileString(section, name, "", temp, size, filePath);
                str = Convert.ToString(temp);
            }
            catch (Exception ex)
            {

            }
            return str;
        }

        public void setINI(string section, string name, string value)
        {
            WritePrivateProfileString(section, name, value, filePath);
        }


    }
}
