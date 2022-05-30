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
    /// Interaction logic for Detail_customer.xaml
    /// </summary>
    public partial class Detail_customer : Window
    {
        private string currentID;
        private string[] startAddress; //først vejnavn så kommunenavn

        public Detail_customer(Customer customer)
        {
            InitializeComponent();

            currentID = customer.CustomerID;
            C_firstName_txt.Content = customer.FirstName;
            C_LastName_txt.Content = customer.LastName;
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
            C_Birth_txt.Content = customer.Birth;
            C_Phone_txt.Content = customer.Phone;
            C_Email_txt.Content = customer.Email;

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

    }
}
