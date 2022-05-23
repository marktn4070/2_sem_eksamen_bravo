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


namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for Update_customer.xaml
    /// </summary>
    public partial class Update_customer : Window
    {
        public string C_id_public;

        public Update_customer(string C_id_sting, string C_firstName_sting, string C_LastName_sting, string C_Registered_sting, string C_Gender_sting, string C_Birth_sting, string C_Phone_sting, string C_Email_sting)
        {
            InitializeComponent();


            C_id_public = C_id_sting;
            C_firstName_txt.Text = C_firstName_sting;
            C_LastName_txt.Text = C_LastName_sting;
            C_Registered_txt.Text = C_Registered_sting;
            C_Gender_txt.Text = C_Gender_sting;
            C_Birth_txt.Text = C_Birth_sting;
            C_Phone_txt.Text = C_Phone_sting;
            C_Email_txt.Text = C_Email_sting;

        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
