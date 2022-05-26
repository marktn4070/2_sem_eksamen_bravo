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
using System.Globalization;
using System.Text.RegularExpressions;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window //james
    {
        public CreateCustomer()
        {
            InitializeComponent();
            Kommune.ItemsSource = SQL.GetMunicipalities();
        }


        private void txb_TextChanged_Birthday(object sender, TextChangedEventArgs e)
        {
            int time = Birthday.Text.Length;

            if (Birthday.Text == "" || Birthmonth.Text == "" || Birthyear.Text == "")
            {
                if (time == 2)
                {
                    Birthday.Text.Remove(time - 1);
                    Keyboard.Focus(Birthmonth);
                }
            }
        }

        private void txb_TextChanged_Birthmonth(object sender, TextChangedEventArgs e)
        {
            int time = Birthmonth.Text.Length;

            if (Birthday.Text == "" || Birthmonth.Text == "" || Birthyear.Text == "")
            {
                if (time == 2)
                {
                    Birthmonth.Text.Remove(time - 1);
                    Keyboard.Focus(Birthyear);
                }
            }
        }

    




        //public bool IsValid()
        //{



        //    // Navne feltet
        //    if (Name.Text == string.Empty)
        //    {
        //        MessageBox.Show("Navn skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else if (Regex.IsMatch(Name.Text, "[^æøåÆØÅa-zA-Z ]"))
        //    {
        //        MessageBox.Show("Vær venligst at indtaste bogstaver ved 'Navn'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    // Email feltet
        //    if (Mail.Text == string.Empty)
        //    {
        //        MessageBox.Show("E-mail skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else if (!Regex.IsMatch(Mail.Text, @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$"))
        //    {
        //        MessageBox.Show("Vær venligst at indtaste en korrekt e-mail adresse", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    // Telefonnummer feltet
        //    if (Phone.Text == string.Empty)
        //    {
        //        MessageBox.Show("Telefon nr. skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else if (Regex.IsMatch(Phone.Text, "[^0-9]"))
        //    {
        //        MessageBox.Show("Vær venligst at indtaste tal ved 'Telefon nr'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        Phone.Text = Phone.Text.Remove(Phone.Text.Length - 1);
        //        return false;
        //    }

        //    // Adresse feltet
        //    if (Address.Text == string.Empty)
        //    {
        //        MessageBox.Show("Addresse skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    // Postnummer feltet
        //    if (Zip.Text == string.Empty)
        //    {
        //        MessageBox.Show("Post nr. skal udfyldes", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else if (Regex.IsMatch(Zip.Text, "[^0-9](0-4)"))
        //    {
        //        MessageBox.Show("Vær venligst at kun indtaste tal ved 'Post nr'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    // By feltet
        //    if (City.Text == string.Empty)
        //    {
        //        MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //    else if (Regex.IsMatch(City.Text, @"[^ÆØÅæøåa-zA-Z^\s]"))
        //    {
        //        MessageBox.Show("Vær venligst at indtaste bogstaver ved 'By'", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }

        //    return true;
        //}




        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            //validation here
            if (FirstName.Text == string.Empty || LastName.Text == string.Empty || Birthday.Text == string.Empty || Birthmonth.Text == string.Empty ||
                Birthyear.Text == string.Empty || Phone.Text == string.Empty || Email.Text == string.Empty || Kommune.SelectedItem == null
                || Vej.SelectedItem == null || (Male.IsChecked == Female.IsChecked && Female.IsChecked == Other.IsChecked))
            {
                MessageBox.Show("En eller flere felter mangler!");
            }
            else
            {
                try
                {
                    int.Parse(Birthday.Text);
                    int.Parse(Birthmonth.Text);
                    int.Parse(Birthyear.Text);
                    
                    if (Birthday.Text.Trim().Length == 2 && Birthmonth.Text.Trim().Length == 2 && Birthyear.Text.Trim().Length == 4)
                    {
                        //DateTime.Now.Date
                        if (int.Parse(Birthday.Text) < 1 || int.Parse(Birthday.Text) > 31 ||  int.Parse(Birthmonth.Text) < 1 || int.Parse(Birthmonth.Text) > 12 || int.Parse(Birthyear.Text) < 1850)
                        {
                            MessageBox.Show("Invalid dato!");
                        }
                        else
                        {
                            string birth = Birthyear.Text.Trim() + "-" + Birthmonth.Text.Trim() + "-" + Birthday.Text.Trim();
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
                            int phone = 0;
                            if (Phone.Text.Trim().Length == 8 && int.TryParse(Phone.Text.Trim(), out phone))
                            {
                                Regex emailCheck = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                + "@"
                                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"); 
                                if (emailCheck.IsMatch(Email.Text.Trim()))
                                {
                                    SQL.RegisterCustomer(FirstName.Text.Trim(), LastName.Text.Trim(), (bool)Registered.IsChecked, gender, birth, phone, Email.Text.Trim(), Kommune.SelectedItem.ToString(), Vej.SelectedItem.ToString());
                                    MessageBox.Show("Kunde oprettet!");
                                    ClearAll();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid email!");
                                }

                            }
                            else
                            {
                                MessageBox.Show("Telefonnummer skal være et 8-cifret tal!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dato skal være i format 31-12-2000");
                    }
                }
                catch
                {
                    MessageBox.Show("Dato skal være i format 31-12-2000");
                }
                
                
            }
        }
        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            Vej.ItemsSource = new List<string>();
            Kommune.SelectedItem = null;
            FirstName.Text = "";
            LastName.Text = "";
            Phone.Text = "";
            Email.Text = "";
            Birthday.Text = "";
            Birthmonth.Text = "";
            Birthyear.Text = "";
            Male.IsChecked = false;
            Female.IsChecked = false;
            Other.IsChecked = false;
            Registered.IsChecked = false;
        }
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
