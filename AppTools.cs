using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string ValuesValidation(string name, ref string phoneNumber, ref string email)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email)) return "Name, phone and email fields empty";
            if (string.IsNullOrWhiteSpace(phoneNumber)) phoneNumber = " ";
            if (string.IsNullOrWhiteSpace(email)) email = " ";
            if (name.Length > 50 || !new Regex(pattern: @"(^[a-zA-Z '-]{1,25})|(^[А-Яа-я '-]{1,25})").IsMatch(name)) return "Invalid name";
            if (!string.IsNullOrWhiteSpace(phoneNumber)) if (phoneNumber.Length > 13 || phoneNumber[1..].Any(c => !char.IsDigit(c))) return "Invalid phone number";
            if (!string.IsNullOrWhiteSpace(email)) if (email.Length > 30 || !new Regex(pattern: @"^([.,0-9a-zA-Z_-]{1,20}@[a-zA-Z]{1,10}.[a-zA-Z]{1,3})").IsMatch(email)) return "Invalid E-Mail address type";
            if (phoneNumber.StartsWith("80")) phoneNumber = "375" + phoneNumber[2..];

            return "ok";
        }
    }
}
