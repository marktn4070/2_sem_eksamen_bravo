using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2_sem_eksamen_bravo
{
    static class MessageEmulator
    {
        public static string path = Directory.GetCurrentDirectory();

        public static void EmulateSendSms(string headline, string subheadline, string message)
        {
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\log" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".txt"))
            {
                w.Write(headline + "\n \n" + subheadline + "\n \n" + message);
            }
        }
        public static void EmulateSendEmail(string headline, string subheadline, string message)
        {
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\log" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".html"))
            {
                w.Write(@" <!DOCTYPE html>
                <html>
                <body>

                <h1> " + headline + @" </h1>
                <h2> " + subheadline + @" </h2>
                <p> " + message + @"</p>
                </body>
                </html> ");
            }
        }
    }
}
