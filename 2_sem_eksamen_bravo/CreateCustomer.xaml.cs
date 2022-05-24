﻿using System;
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
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            //validation here
            if (FirstName.Text == string.Empty || LastName.Text == string.Empty || Birthday.Text == string.Empty || Birthmonth.Text == string.Empty ||
                Birthyear.Text == string.Empty || Phone.Text == string.Empty ||Email.Text == string.Empty || Zip.Text == string.Empty 
                || Road.Text == string.Empty || (Male.IsChecked == Female.IsChecked && Female.IsChecked == Other.IsChecked))
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
                
                string birth = "";
                birth = Birthday.Text + "-" + Birthmonth.Text + "-" + Birthyear.Text;
                string gender = "";
                SQL.RegisterCustomer(FirstName.Text, LastName.Text, (bool)Registered.IsChecked, gender, birth, int.Parse(Phone.Text), Email.Text, int.Parse(Zip.Text), Road.Text);
            }
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
