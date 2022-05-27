using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions; // Skal bruges for at kunne bruge Regex
using System.Configuration;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for Update_customer.xaml
    /// </summary>
    public partial class Update_customer : Window
    {
        public string C_id_public;

        public Update_customer(string CustomerID_sting, string C_firstName_sting, string C_LastName_sting, string C_Registered_sting, string C_Gender_sting, string C_Birth_sting, string C_Phone_sting, string C_Email_sting)
        {
            InitializeComponent();


            C_id_public = CustomerID_sting;
            C_firstName_txt.Text = C_firstName_sting;
            C_LastName_txt.Text = C_LastName_sting;
            //C_Registered_txt.Text = C_Registered_sting;
            //C_Gender_txt.Text = C_Gender_sting;
            C_Birth_txt.Text = C_Birth_sting;
            C_Phone_txt.Text = C_Phone_sting;
            C_Email_txt.Text = C_Email_sting;

        }
        SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);

        public void clearData()
        {
            C_firstName_txt.Clear();
            C_LastName_txt.Clear();
            //C_Registered_txt.Clear();
            //C_Gender_txt.Clear();
            C_Birth_txt.Clear();
            C_Phone_txt.Clear();
            C_Email_txt.Clear();
        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }
        public bool IsValid()
        {

            //Fornavn feltet
            if (C_firstName_txt.Text == string.Empty)
            {
                MessageBox.Show("Navn skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (Regex.IsMatch(C_firstName_txt.Text, "[^æøåÆØÅa-zA-Z ]"))
            {
                MessageBox.Show("Vær venligst at indtaste bogstaver ved 'Navn'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //Efternavn feltet
            if (C_LastName_txt.Text == string.Empty)
            {
                MessageBox.Show("Navn skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (Regex.IsMatch(C_LastName_txt.Text, "[^æøåÆØÅa-zA-Z ]"))
            {
                MessageBox.Show("Vær venligst at indtaste bogstaver ved 'Navn'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Telefonnummer feltet
            if (C_Phone_txt.Text == string.Empty)
            {
                MessageBox.Show("Telefon nr. skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (Regex.IsMatch(C_Phone_txt.Text, "[^0-9]"))
            {
                MessageBox.Show("Vær venligst at indtaste tal ved 'Telefon nr'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                C_Phone_txt.Text = C_Phone_txt.Text.Remove(C_Phone_txt.Text.Length - 1);
                return false;
            }

            // Email feltet
            if (C_Email_txt.Text == string.Empty)
            {
                MessageBox.Show("E-mail skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.IsMatch(C_Email_txt.Text, @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$"))
            {
                MessageBox.Show("Vær venligst at indtaste en korrekt e-mail adresse", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }


        private void Update_Btn_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (IsValid())
                {
                    host.Open();
                    //SqlCommand cmd = new SqlCommand("update Customer set FirstName = '" + C_firstName_txt.Text + "', LastName = '" + C_LastName_txt.Text + "', Registered = '" + C_Registered_txt.Text + "', Gender = '" + C_Gender_txt.Text + "', Birth = '" + C_Birth_txt.Text + "', Phone = '" + C_Phone_txt.Text + "', Email = '" + C_Email_txt.Text + "' WHERE CustomerID = '" + C_id_public + "' ", host);
                    SqlCommand cmd = new SqlCommand("update Customer set FirstName = '" + C_firstName_txt.Text + "', LastName = '" + C_LastName_txt.Text + "', Birth = '" + C_Birth_txt.Text + "', Phone = '" + C_Phone_txt.Text + "', Email = '" + C_Email_txt.Text + "' WHERE CustomerID = '" + C_id_public + "' ", host);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("'" + C_firstName_txt.Text + " " + C_LastName_txt.Text + " er opdateret", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        host.Close();
                        clearData();

                        this.Close();
                    }

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DataChangedEventHandler handler = DataChanged;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

        private void Kommune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Vej_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
