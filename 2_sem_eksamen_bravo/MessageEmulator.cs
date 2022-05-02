using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace _2_sem_eksamen_bravo
{
    static class MessageEmulator
    {
        public static void EmulateSendSms(string message)
        {

        }
        public static void EmulateSendEmail(string message)
        {

        }

        public static void SaveMessage(string headline, string message)
        {
            SqlConnection cnct = null;
            try
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand cmd = new SqlCommand(
                    string.Format("INSERT INTO Message VALUES ('{0}', '{1}', 1900-01-01)", headline, message), //time placeholder!!!!!, mangler også at sanitize input
                    cnct);

                cnct.Open();
                //Print(cmd.ExecuteReader());
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
    }
}
