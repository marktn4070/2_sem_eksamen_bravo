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

        public static void RegisterCustomer(string firstName, string lastName, bool registered, string gender, string birth, int phone, string email, string municipality, string road) //james
        {
            int roadCode = 0;
            SqlConnection cnct = null;

            try //get roadcode
            {
                cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand cmd = new SqlCommand(
                    string.Format("SELECT * FROM Address WHERE Municipality LIKE @Mun AND Road LIKE @Road;"),
                    cnct);
                cmd.Parameters.Add(CreateParam("@Mun", municipality.Trim(), SqlDbType.NVarChar));
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

            if (roadCode != 0)
            {
                try
                {
                    cnct = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                    SqlCommand cmd = new SqlCommand(
                        string.Format("INSERT INTO Customer VALUES (@FirstName, @LastName, '{0}', '{1}', '{2}', {3}, @Email, {4});", registered, gender, birth, phone, roadCode),
                        cnct);
                    cmd.Parameters.Add(CreateParam("@FirstName", firstName.Trim(), SqlDbType.NVarChar));
                    cmd.Parameters.Add(CreateParam("@LastName", lastName.Trim(), SqlDbType.NVarChar));
                    cmd.Parameters.Add(CreateParam("@Email", email.Trim(), SqlDbType.NVarChar));

                    cnct.Open();
                    cmd.ExecuteNonQuery();
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
        }

        public static void DeleteCustomer(string customerID)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand command = new SqlCommand("Delete FROM Customer WHERE CustomerID LIKE @C_id", connection);
                command.Parameters.Add(CreateParam("@C_id", customerID.Trim(), SqlDbType.NVarChar));
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
        public static void AdresseImpoter() //Kevin
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("RoadcodeID", typeof(int)));
            tbl.Columns.Add(new DataColumn("Road", typeof(string)));
            tbl.Columns.Add(new DataColumn("Zip", typeof(int)));
            tbl.Columns.Add(new DataColumn("Municipality", typeof(string)));
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                string tjek = string.Empty;
                string tjek2 = string.Empty;
                //cmd = new SqlCommand("Delete from Address", connect);
                //cmd.ExecuteNonQuery();
                foreach (string file in Directory.EnumerateFiles(@"C:\dropzone", "*.txt"))
                {
                    foreach (var line in File.ReadLines(file, System.Text.Encoding.Default).Skip(1))
                    {

                        //Den her condition tal kan måske være bedre (kigge på pdf om det)
                        if (1000000000 > Convert.ToInt64(line.Substring(0, 11)))
                        {
                            if (tjek != line.Substring(60, 4) && tjek2 != line.Substring(31, 20))
                            {
                                DataRow dr = tbl.NewRow();
                                dr["RoadcodeID"] = Convert.ToInt32(line.Substring(0, 11));
                                dr["Road"] = line.Substring(31, 20).Trim();
                                dr["Zip"] = Convert.ToInt32(line.Substring(60, 4));
                                dr["Municipality"] = line.Substring(11, 20).Trim();

                                tbl.Rows.Add(dr);
                            }

                            tjek = line.Substring(60, 4);
                            tjek2 = line.Substring(31, 20);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                SqlBulkCopy objbulk = new SqlBulkCopy(connect);
                objbulk.DestinationTableName = "Address";
                objbulk.ColumnMappings.Add("RoadcodeID", "RoadcodeID");
                objbulk.ColumnMappings.Add("Road", "Road");
                objbulk.ColumnMappings.Add("Zip", "Zip");
                objbulk.ColumnMappings.Add("Municipality", "Municipality");
                connect.Open();
                objbulk.WriteToServer(tbl);
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

        public static List<string> GetCustomerName()
        {
            List<string> Names = new List<string>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                connect.Open();
                SqlCommand cmd = new SqlCommand(string.Format("SELECT FirstName, LastName FROM Customer;", connect));
                cmd.Connection = connect;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Names.Add(reader[0].ToString() + " " + reader[1].ToString());
                }
            }
            catch (Exception ex)
            {
                //MainWindow.ShowError(ex);
            }
            finally
            {
                if (connect != null)
                {
                    connect.Close();
                }
            }
            return Names;
        }
    }
}