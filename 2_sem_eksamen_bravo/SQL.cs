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
        public static int SaveMessage(string headline, string subheadline, string message, bool sms, bool email, bool emailGeo, object roadName) //james
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

        public static List<string> GetMunicipalities() //james
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
            municipalities.Sort();
            return municipalities;
        }
        public static List<string> GetRoads(string municipality) //james
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
            roads.Sort();
            return roads;
        }

        public static void RegisterCustomer(string firstName, string lastName, bool registered, string gender, string birth, int phone, string email, int zip, string road) //james
        {
            int roadCode;
            SqlConnection cnct = null;

            try //get roadcode
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand cmd = new SqlCommand(
                    string.Format("SELECT * FROM Address WHERE Zip LIKE {0} AND Road LIKE @Road;", zip),
                    cnct);
                cmd.Parameters.Add(CreateParam("@Road", road.Trim(), SqlDbType.NVarChar));
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
                SqlCommand cmd = new SqlCommand(
                    string.Format("INSERT INTO Customer VALUES (@FirstName, @LastName, {0}, @Gender, GETDATE(), '{0}', '{1}');", registered),
                    cnct);
                cmd.Parameters.Add(CreateParam("@FirstName", firstName.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@LastName", lastName.Trim(), SqlDbType.NVarChar));
                cmd.Parameters.Add(CreateParam("@Gender", gender.Trim(), SqlDbType.NVarChar));

                cnct.Open();
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
