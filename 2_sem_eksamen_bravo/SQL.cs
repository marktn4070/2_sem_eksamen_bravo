using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace _2_sem_eksamen_bravo
{
    static class SQL
    {
        public static int SaveMessage(string headline, string subheadline, string message, bool sms, bool email, bool emailGeo, object roadName)
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

                cnct.Open();
                addedMessagesId = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cnct != null)
                {
                    cnct.Close();
                }
            }

            if (email && !emailGeo)
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
            if (sms || emailGeo) //husk ikke gem i historik for dem som er registered email
            {
                int roadCode = -1;
                try //get roadcode
                {

                    SqlCommand cmd = new SqlCommand(
                   string.Format("SELECT * FROM Address WHERE Road LIKE @Road"),
                   cnct);
                    cmd.Parameters.Add(CreateParam("@Road", roadName, SqlDbType.NVarChar));
                    cnct.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        roadCode = (int)reader[0];
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cnct != null)
                    {
                        cnct.Close();
                    }
                }
                try
                {
                    cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                    SqlCommand command = null;
                    if (email) //gemmer kun for dem der ikke har email hvis email allerede er blevet gemt i historik
                    {
                        command = new SqlCommand(string.Format("SELECT * FROM Customer WHERE Registered LIKE 0 AND RoadcodeID LIKE {0};", roadCode), cnct);
                    }
                    else 
                    {
                        command = new SqlCommand(string.Format("SELECT * FROM Customer WHERE RoadcodeID LIKE {0};", roadCode), cnct);
                    }
                    cnct.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        howManyReceived++;
                        SqlCommand addToHistory = new SqlCommand(string.Format("INSERT INTO Message_history VALUES ({0}, {1});", addedMessagesId, reader[0]), cnct);
                        addToHistory.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (cnct != null)
                    {
                        cnct.Close();
                    }
                }
            }
            return howManyReceived;
        }

        private static SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }

        public static List<string> GetMunicipalities()
        {
            List<string> municipalities = new List<string>();
            SqlConnection cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                SqlCommand command = new SqlCommand("SELECT DISTINCT Municipality FROM Address;", cnct);
                cnct.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    municipalities.Add(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowError(ex);
            }
            finally
            {
                if (cnct != null)
                {
                    cnct.Close();
                }
            }
            return municipalities;
        }
        public static List<string> GetRoads(string municipality)
        {
            List<string> roads = new List<string>();
            SqlConnection cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(
                    string.Format("SELECT * FROM Address WHERE Municipality LIKE @Mun;"),
                    cnct);
                cmd.Parameters.Add(CreateParam("@Mun", municipality, SqlDbType.NVarChar));
                cnct.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roads.Add(reader[1].ToString());
                }
            }
            catch (Exception ex)
            {
                MainWindow.ShowError(ex);
            }
            finally
            {
                if (cnct != null)
                {
                    cnct.Close();
                }
            }
            return roads;
        }
    }
}
