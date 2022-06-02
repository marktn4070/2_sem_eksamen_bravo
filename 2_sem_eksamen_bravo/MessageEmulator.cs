using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace _2_sem_eksamen_bravo
{
    static class MessageEmulator
    {
        #region Coded by James
        public static string path = Directory.GetCurrentDirectory();

        public static void EmulateSendSms(string headline, string subheadline, string message)
        {
            CreateFolder();
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\logs\log" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".txt"))
            {
                w.Write(headline + "\n \n" + subheadline + "\n \n" + message);
            }
        }
        public static void EmulateSendEmail(string headline, string subheadline, string message)
        {
            CreateFolder();
            //Husk at lave en mappe for de filer der bliver gemt
            using (StreamWriter w = new StreamWriter(path.Remove(path.IndexOf(@"\bin")) + @"\logs\log" + DateTime.Now.ToString("yyyyddMMHHmmss") + ".html"))
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

        public static int SaveMessage(string headline, string subheadline, string message, bool sms, bool email, bool emailGeo, object kommuneName, object roadName)
        {
            return SQL.SaveMessage(headline, subheadline, message, sms, email, emailGeo, kommuneName, roadName);
        }
        #endregion

        #region Coded by Kevin
        public static void CreateFolder()
        {
            Directory.CreateDirectory(path.Remove(path.IndexOf(@"\bin")) + @"\logs");
        }
        #endregion
    }
}
