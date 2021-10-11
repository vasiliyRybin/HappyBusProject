using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyBusProject
{
    public static class AppTools
    {
        private static readonly string logPath = Environment.CurrentDirectory.ToString() + $"\\Log\\{DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day}.txt";
        public static void ErrorWriterTpFile(string message)
        {
            try
            {
                using var fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using BufferedStream bs = new(fs);
                using TextWriter sr = new StreamWriter(bs, Encoding.Default);
                sr.WriteLine(DateTime.Now + "\t" + message);

            }
            catch (Exception e)
            {
                throw new IOException(e.Message);
            }
        }
    }
}
