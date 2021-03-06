using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using SqlBulkTools;
using System.Data.OleDb;

namespace _2_sem_eksamen_bravo
{
    static class SQL 
    {
        #region Coded by James
        public static int SaveMessage(string headline, string subheadline, string message, bool sms, bool email, bool emailGeo, object kommuneName, object roadName) //james
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
            catch (Exception)
            {
                throw;
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
                catch (Exception)
                {
                    throw;
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
                    roadCode = GetRoadCode(kommuneName.ToString(), roadName.ToString());
                }
                catch (Exception)
                {
                    throw;
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
                    if (email && !sms) //gemmer kun for dem der ikke har email hvis email allerede er blevet gemt i historik
                    {
                        command = new SqlCommand(string.Format("SELECT * FROM Customer WHERE Registered LIKE 1 AND RoadcodeID LIKE {0};", roadCode), cnct);
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
                catch (Exception)
                {
                    throw;
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
        #endregion

        #region Coded by Mark
        public static List<Customer> GetCustomer() //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                List<Customer> customer_list = new List<Customer>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", host);
                DataTable dt = new DataTable(); //nødvendig?
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Customer currentCustomer = new Customer
                    {
                        CustomerID = sdr[0].ToString(),
                        FirstName = sdr[1].ToString(),
                        LastName = sdr[2].ToString(),
                        Registered = (bool)sdr[3],
                        Gender = sdr[4].ToString(),
                        Birth = sdr[5].ToString(),
                        Phone = sdr[6].ToString(),
                        Email = sdr[7].ToString(),
                        RoadcodeID = sdr[8].ToString()
                    };
                    currentCustomer.UpdateAddress();
                    customer_list.Add(currentCustomer);
                }
                host.Close();
                return customer_list;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static List<Customer> GetCustomerGotMessage(int messageID) //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                SqlCommand getHistory = new SqlCommand(string.Format("SELECT * FROM Message_history WHERE MessageID LIKE {0}", messageID), host);
                List<int> customerIDS = new List<int>();
                
                DataTable dt = new DataTable(); //nødvendig?
                host.Open();

                SqlDataReader reader = getHistory.ExecuteReader();
                while (reader.Read())
                {
                    customerIDS.Add((int)reader[1]);
                }
                List<Customer> customer_list = new List<Customer>();

                foreach (int id in customerIDS)
                {
                    SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM Customer WHERE CustomerID LIKE {0}", id), host);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        Customer currentCustomer = new Customer
                        {
                            CustomerID = sdr[0].ToString(),
                            FirstName = sdr[1].ToString(),
                            LastName = sdr[2].ToString(),
                            Registered = (bool)sdr[3],
                            Gender = sdr[4].ToString(),
                            Birth = sdr[5].ToString(),
                            Phone = sdr[6].ToString(),
                            Email = sdr[7].ToString(),
                            RoadcodeID = sdr[8].ToString()
                        };
                        currentCustomer.UpdateAddress();
                        customer_list.Add(currentCustomer);
                    }
                }
                host.Close();
                return customer_list;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<Customer> SearchCustomer(string name) //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                List<Customer> customer_list = new List<Customer>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE FirstName + ' ' + LastName like '%" + name + "%' or FirstName like '%" + name + "%' or LastName like '%" + name + "%'", host);
                DataTable dt = new DataTable();
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Customer currentCustomer = new Customer
                    {
                        CustomerID = sdr[0].ToString(),
                        FirstName = sdr[1].ToString(),
                        LastName = sdr[2].ToString(),
                        Registered = (bool)sdr[3],
                        Gender = sdr[4].ToString(),
                        Birth = sdr[5].ToString(),
                        Phone = sdr[6].ToString(),
                        Email = sdr[7].ToString(),
                        RoadcodeID = sdr[8].ToString()
                    };
                    currentCustomer.UpdateAddress();
                    customer_list.Add(currentCustomer);
                }
                host.Close();
                return customer_list;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static List<Message> GetMMessage() //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                List<Message> message_list = new List<Message>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Message", host);
                DataTable dt = new DataTable(); //nødvendig?
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    message_list.Add(new Message
                    {
                        MessageID = (int)sdr[0],
                        Headline = sdr[1].ToString(),
                        Subheadline = sdr[2].ToString(),
                        Text = sdr[3].ToString(),
                        Time = sdr[4].ToString(),
                        Email = (bool)sdr[5],
                        Sms = (bool)sdr[6]

                    });
                }
                return message_list;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<Message> GetMessageSendToCustomer(int customerID) //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                List<Message> message_list = new List<Message>();
                SqlCommand intermediate = new SqlCommand(string.Format("SELECT * FROM Message_history WHERE CustomerID LIKE {0}", customerID), host);
                DataTable dt = new DataTable(); //nødvendig?

                host.Open();
                List<int> relevantMessageIDS = new List<int>();
                SqlDataReader reader = intermediate.ExecuteReader();
                while(reader.Read())
                {
                    relevantMessageIDS.Add((int)reader[0]);
                }
                foreach (int id in relevantMessageIDS)
                {
                    SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM Message WHERE MessageID LIKE {0}", id), host);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        message_list.Add(new Message
                        {
                            MessageID = (int)sdr[0],
                            Headline = sdr[1].ToString(),
                            Subheadline = sdr[2].ToString(),
                            Text = sdr[3].ToString(),
                            Time = sdr[4].ToString(),
                            Email = (bool)sdr[5],
                            Sms = (bool)sdr[6]
                        });
                    }
                }
                
                return message_list;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static DataTable SearchMessage(string startDate, string endDate) //Mark
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM Message WHERE Time between '{0}' and '{1}';", startDate, endDate), host);
                DataTable dt = new DataTable();
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                host.Close();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Coded by James
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
            catch (Exception)
            {
                throw;
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
        public static List<string> GetRoads(string municipality) //James
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
            catch (Exception)
            {
                throw;
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

        public static int GetRoadCode(string municipality, string road) //James
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
                cnct.Close();
                return roadCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void RegisterCustomer(string firstName, string lastName, bool registered, string gender, string birth, int phone, string email, string municipality, string road) //james
        {

            SqlConnection cnct = null;
            int roadCode = GetRoadCode(municipality, road);

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
                catch (Exception)
                {
                    throw;
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

     

        public static void DeleteCustomer(string customerID) //james
        {            
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
                SqlCommand deleteHistFor = new SqlCommand(string.Format("DELETE FROM Message_history WHERE CustomerID LIKE {0};", customerID), connection);
                SqlCommand deleteCust = new SqlCommand(string.Format("DELETE FROM Customer WHERE CustomerID LIKE {0};", customerID), connection);
                connection.Open();
                
                deleteHistFor.ExecuteNonQuery();
                deleteCust.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
        }
        #endregion

        #region Coded by Kevin
        public static void AdresseImpoter() //Kevin
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
            List<Address> adresslist = new List<Address>();
            try
            {
                string path = @"C:\dropzone"; //pas på med at ændre "path"
                string tjek = string.Empty;
                string tjek2 = string.Empty;
                connect.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from Address", connect);
                int count = (int)cmd.ExecuteScalar();
                connect.Close();
                DirectoryInfo folder = new DirectoryInfo(path);
                FileInfo[] txtExist = folder.GetFiles("*.txt");
                if (true == Directory.EnumerateFileSystemEntries(path).Any() && txtExist.Length == 0)
                {
                    throw new ArgumentException("de satte filer er ikke tekst filer i dropzone mappen");
                }
                if (txtExist.Length == 0 && count <= 0)
                {
                    throw new ArgumentException("Ingen adresser i databasen, tilføj postdistrikt filen til dropzone for at bruge programmet");
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
                    {
                        if (84 != File.ReadLines(file, System.Text.Encoding.Default).Skip(1).First().Length)
                        {
                            throw new ArgumentException(string.Format("Forkert file i dropzone: {0}", file.Remove(0, 12)));
                        }

                        foreach (var line in File.ReadLines(file, System.Text.Encoding.Default).Skip(1))
                        {
                            if (001 == Convert.ToInt64(line.Substring(0, 3)))
                            {
                                if (tjek != line.Substring(60, 4) && tjek2 != line.Substring(31, 20))
                                {
                                    adresslist.Add(new Address
                                    {
                                        RoadcodeID = Convert.ToInt32(line.Substring(3, 8)),
                                        Road = line.Substring(31, 20).Trim(),
                                        Zip = Convert.ToInt32(line.Substring(60, 4)),
                                        Municipality = line.Substring(11, 20).Trim()
                                    });

                                }

                                tjek = line.Substring(60, 4);
                                tjek2 = line.Substring(31, 20);
                            }
                            else
                            {
                                break;
                            }
                        }
                        var bulk = new BulkOperations();
                        bulk.Setup<Address>(x => x.ForCollection(adresslist))
                        .WithTable("Address")
                        .AddColumn(x => x.RoadcodeID)
                        .AddColumn(x => x.Road)
                        .AddColumn(x => x.Zip)
                        .AddColumn(x => x.Municipality)
                        .BulkInsertOrUpdate()
                        .MatchTargetOn(x => x.RoadcodeID);

                        bulk.CommitTransaction(connect);
                    }

                    foreach (string file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connect != null)
                {
                    connect.Close();
                }
            }
        }

        public static List<string> GetCustomerName() //kevin
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
            catch (Exception)
            {
                throw;
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
        #endregion

        #region Coded by James
        public static void UpdateCustomer(Customer customer) //james
        {
            SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);

            host.Open();
            SqlCommand cmd = new SqlCommand(string.Format("UPDATE Customer SET FirstName = @First, LastName = @Last, Registered = '{0}', Gender = '{1}', Birth = '{2}', Phone = '{3}', Email = @Email, RoadcodeID = '{4}' WHERE CustomerID LIKE '{5}';", customer.Registered, customer.Gender, customer.Birth, customer.Phone, customer.RoadcodeID, customer.CustomerID), host);
            cmd.Parameters.Add(CreateParam("@First", customer.FirstName, SqlDbType.NVarChar));
            cmd.Parameters.Add(CreateParam("@Last", customer.LastName, SqlDbType.NVarChar));
            cmd.Parameters.Add(CreateParam("@Email", customer.Email, SqlDbType.NVarChar));
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (host != null)
                {
                    host.Close();
                }
            }
        }

        public static string[] GetRoadAndMunicipalityNames(string roadCode) //james
        {
            string[] names = new string[3]; //første vejnavn, næste zip, så kommunenavn
            try
            {
                SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);

                host.Open();
                SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM Address WHERE RoadcodeID LIKE {0}", roadCode), host);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    names[0] = reader[1].ToString();
                    names[1] = reader[3].ToString();
                    names[2] = reader[2].ToString();
                }
                host.Close();
                return names;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}