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
                startAddress = SQL.GetRoadAndMunicipalityNames(customer.RoadcodeID);
                Kommune.ItemsSource = SQL.GetMunicipalities();
                Kommune.SelectedItem = startAddress[1];
                Vej.SelectedItem = startAddress[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); //måske gøre mere ud af den her..
            }
            currentID = customer.CustomerID;
            FirstName_txt.Text = customer.FirstName;
            LastName_txt.Text = customer.LastName;
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
            Birthday_txt.Text = customer.Birth;
            Phone_txt.Text = customer.Phone;
            Email_txt.Text = customer.Email;

        }

        public void ClearData()
        {
            Vej.ItemsSource = new List<string>();
            Kommune.SelectedItem = null;
            FirstName_txt.Clear();
            LastName_txt.Clear();
            Registered.IsChecked = false;
            Male.IsChecked = false;
            Female.IsChecked = false;
            Other.IsChecked = false;

            Birthday_txt.Clear();
            Phone_txt.Clear();
            Email_txt.Clear();
        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            //spørg om sikker nok en god ide................
            ClearData();
        }

        public bool IsValid()
        {
            //if (Birthday_txt.Text.Trim().Length == 2 && Birthmonth_txt.Text.Trim().Length == 2 && Birthyear_txt.Text.Trim().Length == 4)
            //{
            //    MessageBox.Show("Dato skal være i format 31-12-2000", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}



            // Dato feltet
            if (Birthday_txt.Text == string.Empty)
            {
                Birthday__error.Text = "Fødseldag dato, måned eller år er ikke udfyldet";
            }


            //// Dato feltet
            //if (Birthday_txt.Text == string.Empty)
            //{
            //    Birthday__error.Text = "Fødseldag dato, måned eller år er ikke udfyldet";
            //}


            //// Måned feltet
            //else if (Birthmonth_txt.Text == string.Empty)
            //{
            //    Birthday__error.Text = "Fødseldag dato, måned eller år er ikke udfyldet";
            //}


            //// År feltet
            //else if (Birthyear_txt.Text == string.Empty)
            //{
            //    Birthday__error.Text = "Fødseldag dato, måned eller år er ikke udfyldet";
            //}




            ////DateTime.Now.Date
            //else if (int.Parse(Birthday_txt.Text) < 1 || int.Parse(Birthday_txt.Text) > 31 || int.Parse(Birthmonth_txt.Text) < 1 || int.Parse(Birthmonth_txt.Text) > 12 || int.Parse(Birthyear_txt.Text) < 1850)
            //{
            //    Birthday__error.Text = "Invalid dato!";
            //}


            // Køn feltet
            if (Male.IsChecked == Female.IsChecked && Female.IsChecked == Other.IsChecked)
            {
                Gender__error.Text = "Køn skal vælges";
            }

            // Kommune feltet
            if (Kommune.SelectedItem == null)
            {
                Kommune__error.Text = "Kommunenavn skal vælges";
            }

            // Vej feltet
            if (Vej.SelectedItem == null)
            {
                Vej__error.Text = "Vejnavn skal vælges";
            }


            // Fornavn feltet
            if (FirstName_txt.Text == string.Empty)
            {
                FirstName__error.Text = "Fornavn skal udfyldes";
            }
            else if (Regex.IsMatch(FirstName_txt.Text, "[^æøåÆØÅa-zA-Z ]"))
            {
                FirstName__error.Text = "Vær venligst at indtaste bogstaver ved 'Fornavn'";
            }


            // Efternavn feltet
            if (LastName_txt.Text == string.Empty)
            {
                LastName__error.Text = "Efternavn skal udfyldes";
            }
            else if (Regex.IsMatch(LastName_txt.Text, "[^æøåÆØÅa-zA-Z ]"))
            {
                LastName__error.Text = "Vær venligst at indtaste bogstaver ved 'Efternavn'";
            }


            // Email feltet
            if (Email_txt.Text == string.Empty)
            {
                Email__error.Text = "E-mail skal udfyldes";
            }
            else if (!Regex.IsMatch(Email_txt.Text, @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$"))
            {
                Email__error.Text = "Vær venligst at indtaste en korrekt e-mail adresse";
            }

            // Telefonnummer feltet
            if (Phone_txt.Text == string.Empty)
            {
                Phone__error.Text = "Telefon nr. skal udfyldes";
            }
            else if (Regex.IsMatch(Phone_txt.Text, "[^0-9]"))
            {
                Phone__error.Text = "Vær venligst at indtaste tal ved 'Telefon nr'";
                Phone_txt.Text = Phone_txt.Text.Remove(Phone_txt.Text.Length - 1);
            }


            if (
            Birthday_txt.Text == string.Empty ||
            Male.IsChecked == Female.IsChecked && Female.IsChecked == Other.IsChecked ||
            Kommune.SelectedItem == null ||
            Vej.SelectedItem == null ||
            FirstName_txt.Text == string.Empty ||
            Regex.IsMatch(FirstName_txt.Text, "[^æøåÆØÅa-zA-Z ]") ||
            LastName_txt.Text == string.Empty ||
            Regex.IsMatch(LastName_txt.Text, "[^æøåÆØÅa-zA-Z ]") ||
            Email_txt.Text == string.Empty ||
            !Regex.IsMatch(Email_txt.Text, @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$") ||
            Phone_txt.Text == string.Empty ||
            Regex.IsMatch(Phone_txt.Text, "[^0-9]"))
            {
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

                        SQL.UpdateCustomer(new Customer { FirstName = FirstName_txt.Text, LastName = LastName_txt.Text,
                            Registered = (bool)Registered.IsChecked, Birth = Birthday_txt.Text, CustomerID = currentID, Gender = gender,
                            Email = Email_txt.Text, Phone = Phone_txt.Text,
                            RoadcodeID = SQL.GetRoadCode(Kommune.SelectedItem.ToString(), Vej.SelectedItem.ToString()).ToString() } );
                        MessageBox.Show("'" + FirstName_txt.Text + " " + LastName_txt.Text + " er opdateret", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
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
            Kommune__error.Text = "";


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

        private void FirstName_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            FirstName__error.Text = "";
        }

        private void LastName_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            LastName__error.Text = "";
        }

        private void Phone_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Phone__error.Text = "";
        }

        private void Email_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Email__error.Text = "";
        }

        private void Birthday_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            Birthday__error.Text = "";
        }

        private void Male_Checked(object sender, RoutedEventArgs e)
        {
            Gender__error.Text = "";
        }

        private void Female_Checked(object sender, RoutedEventArgs e)
        {
            Gender__error.Text = "";
        }

        private void Other_Checked(object sender, RoutedEventArgs e)
        {
            Gender__error.Text = "";
        }

        private void Vej_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Vej__error.Text = "";
        }
    }
}
