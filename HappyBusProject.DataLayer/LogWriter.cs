using System;
using System.IO;
using System.Text;

namespace HappyBusProject
{
    public static class LogWriter
    {
        private static readonly string logPath = "D:\\Coding Projects\\Eleks\\HappyBusProject" + $"\\Log\\{DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day}.txt";
        public static void ErrorWriterToFile(string message)
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
