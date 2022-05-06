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

        public static void SaveMessage(string headline, string subheadline, string message, bool sms, bool email)
        {
            SqlConnection cnct = null;
            try
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand cmd = new SqlCommand(
                    string.Format("INSERT INTO Message VALUES (@Headline, @Subheadline, @Message, GETDATE());"),
                    cnct);
                cmd.Parameters.Add(CreateParam("@Headline", headline.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Subheadline", subheadline.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Message", message.Trim(), SqlDbType.NVarChar));

                try
                {
                    cnct.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //aasdsad
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (cnct != null)
                {
                    cnct.Close();
                }
            }
        }
        private static SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
    }
}
