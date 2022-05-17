using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace _2_sem_eksamen_bravo
{
    static class SQL
    {
        public static int SaveMessage(string headline, string subheadline, string message, bool sms, bool email)
        {
            int addedMessagesId = 0;
            int howManyReceived = 0;
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
            if (email)
            {
                try
                { 
                    cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                    SqlCommand command = new SqlCommand("SELECT * FROM Customer WHERE Registered LIKE 1;", cnct);
                    cnct.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        howManyReceived++;
                        customerIdsSentTo.Add((int)reader[0]);
                        SqlCommand addToHistory = new SqlCommand(string.Format("INSERT INTO Message_history VALUES ({0}, {1});", addedMessagesId, reader[0]), cnct);
                        addToHistory.ExecuteNonQuery();
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
            if (sms)
            {

            }
            return howManyReceived;
        }

        private static SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }

        public static void AdresseImpoter()
        {
            int vejkode;
            string vejnavn;
            string kommune;
            int postnummer;
            SqlCommand cmd;
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            //skal kigge på addressID (måske bruge kommunekode i stedet for)
            try
            {
                connect.Open();
                string tjek = string.Empty;
                string tjek2 = string.Empty;
                //cmd = new SqlCommand("Delete from Address", connect);
                //cmd.ExecuteNonQuery();
                foreach (var line in File.ReadLines(@"C:\dropzone\Vejregister-postdistrikt\Vejregister-postdistrikt.txt", System.Text.Encoding.Default).Skip(1))
                {
                    //skal tjekke om det samme vejnavn går igen i databasen
                    if (tjek != line.Substring(60, 4) && tjek2 != line.Substring(31, 20))
                    {
                        vejkode = Convert.ToInt32(line.Substring(0, 11));
                        vejnavn = line.Substring(31, 20).Trim();
                        kommune = line.Substring(11, 20).Trim();
                        postnummer = Convert.ToInt32(line.Substring(60, 4));
                        cmd = new SqlCommand(string.Format("Insert into Address (RoadcodeID, Road, Zip, Municipality)" +
                            " Values ('{0}', @Road, '{1}', '{2}')", vejkode, postnummer, kommune), connect);
                        //cmd = new SqlCommand(string.Format("Insert into Address (Road, Zip, Municipality)" + " Values ('{0}', '{1}', '{2}')", vejnavn, postnummer, kommune), connect);
                        //skal kun når der er brug for
                        cmd.Parameters.AddWithValue("@Road", vejnavn);
                        cmd.ExecuteNonQuery();
                    }

                    tjek = line.Substring(60, 4);
                    tjek2 = line.Substring(31, 20);
                }
            }
            //skal ændre exception beskeden (måske som en return)
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (connect != null)
                {
                    connect.Close();
                }
            }

        }
    }
}
