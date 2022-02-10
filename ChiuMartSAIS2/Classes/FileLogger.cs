using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiuMartSAIS2.Classes
{
    class FileLogger
    {
        public static bool WriteLog(string strMessage)
        {
            try
            {
                string strFileName = "c:\\posBackup\\log\\log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                var dir = Path.GetDirectoryName(strFileName);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }


                //FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), strFileName), FileMode.Append, FileAccess.Write);
                FileStream objFilestream = new FileStream(strFileName, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.WriteLine("------------------");
               
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
