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
using System.IO;
using System.Net.Mail;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window
    {
        public CreateCustomer()
        {
            InitializeComponent();
            Kommune.ItemsSource = SQL.GetMunicipalities();
        }

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
                    
                    if (Birthday.Text.Length == 2 && Birthmonth.Text.Length == 2 && Birthyear.Text.Length == 4)
                    {
                        //DateTime.Now.Date
                        if (int.Parse(Birthday.Text) < 1 || int.Parse(Birthday.Text) > 31 ||  int.Parse(Birthmonth.Text) < 1 || int.Parse(Birthmonth.Text) > 12 || int.Parse(Birthyear.Text) < 1850)
                        {
                            MessageBox.Show("Invalid dato!");
                        }
                        else
                        {
                            string birth = Birthday.Text + "-" + Birthmonth.Text + "-" + Birthyear.Text;
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
                            if (Phone.Text.Length == 8 && int.TryParse(Phone.Text.Trim(), out phone))
                            {
                                MailAddress temp;
                                if (MailAddress.TryCreate(Email.Text.Trim(), out temp))
                                {

                                }
                                else
                                {

                                }
                                SQL.RegisterCustomer(FirstName.Text, LastName.Text, (bool)Registered.IsChecked, gender, birth, phone, Email.Text, Kommune.SelectedItem.ToString(), Vej.SelectedItem.ToString());

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
