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
        private string currentID;
        private string[] startAddress; //først vejnavn så kommunenavn

        public Update_customer(Customer customer)
        {
            InitializeComponent();
            try
            {
                startAddress = SQL.GetRoadAndMunicipalityNames(customer.RoadCodeID);
                Kommune.ItemsSource = SQL.GetMunicipalities();
                Kommune.SelectedItem = startAddress[1];
                Vej.SelectedItem = startAddress[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); //måske gøre mere ud af den her..
            }
            currentID = customer.CustomerID;
            C_firstName_txt.Text = customer.FirstName;
            C_LastName_txt.Text = customer.LastName;
            Registered.IsChecked = customer.Registered;
            if (customer.Gender == "Male")
            {
                Male.IsChecked = true;
            }
            else if (customer.Gender == "Female")
            {
                Female.IsChecked = true;
            }
            else
            {
                Other.IsChecked = true;
            }
            C_Birth_txt.Text = customer.Birth;
            C_Phone_txt.Text = customer.Phone;
            C_Email_txt.Text = customer.Email;

        }

        public void ClearData()
        {
            Vej.ItemsSource = new List<string>();
            Kommune.SelectedItem = null;
            C_firstName_txt.Clear();
            C_LastName_txt.Clear();
            Registered.IsChecked = false;
            Male.IsChecked = false;
            Female.IsChecked = false;
            Other.IsChecked = false;
            C_Birth_txt.Clear();
            C_Phone_txt.Clear();
            C_Email_txt.Clear();
        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            //spørg om sikker nok en god ide................
            ClearData();
        }
        public bool IsValid()
        {
            if (Vej.SelectedItem == null)
            {
                MessageBox.Show("Der skal vælges en adresse!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //Fornavn feltet
            if (C_firstName_txt.Text == string.Empty)
            {
                MessageBox.Show("Navn skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (Regex.IsMatch(C_firstName_txt.Text, "[^æøåÆØÅa-zA-Z ]"))
            {
                MessageBox.Show("Vær venlig at indtaste bogstaver ved 'Navn'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    string gender = "";
                    if ((bool)Male.IsChecked)
                    {
                        gender = "Male";
                    }
                    else if ((bool)Female.IsChecked)
                    {
                        gender = "Female";
                    }
                    else
                    {
                        gender = "Other";
                    }
                    try
                    {
                        SQL.UpdateCustomer(new Customer { FirstName = C_firstName_txt.Text, LastName = C_LastName_txt.Text,
                            Registered = (bool)Registered.IsChecked, Birth = C_Birth_txt.Text, CustomerID = currentID, Gender = gender,
                            Email = C_Email_txt.Text, Phone = C_Phone_txt.Text, RoadCodeID = SQL.GetRoadCode(Kommune.SelectedItem.ToString(), Vej.SelectedItem.ToString()).ToString() } );
                        MessageBox.Show("'" + C_firstName_txt.Text + " " + C_LastName_txt.Text + " er opdateret", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
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
            if (Kommune.SelectedItem != null)
            {
                Vej.IsEnabled = true;
                Vej.ItemsSource = SQL.GetRoads(Kommune.SelectedItem.ToString());
            }
            else
            {
                Vej.IsEnabled = false;
                Vej.ItemsSource = new List<string>();
            }
        }

    }
}
