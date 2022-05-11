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
            int addedMessagesId = 0;
            SqlConnection cnct = null;
            try
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand cmd = new SqlCommand(
                    string.Format("INSERT INTO Message OUTPUT Inserted.MessageID VALUES (@Headline, @Subheadline, @Message, GETDATE(), '{0}', '{1}');", email, sms),
                    cnct);
                cmd.Parameters.Add(CreateParam("@Headline", headline.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Subheadline", subheadline.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Message", message.Trim(), SqlDbType.NVarChar));

                try
                {
                    cnct.Open();
                    addedMessagesId = (int)cmd.ExecuteScalar();
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


            List<int> customerIdsSentTo = new List<int>(); //gemmer liste af dem der har email så de ikke bliver gemt i historikken 2 gange hvis de også har sms
            try
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM Customer WHERE Registered LIKE 1", cnct); 
                cnct.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customerIdsSentTo.Add((int)reader[0]);
                    SqlCommand addToHistory = new SqlCommand(string.Format("INSERT INTO Message_history VALUES ({0}, {1})", addedMessagesId, reader[0].ToString()), cnct);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
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
