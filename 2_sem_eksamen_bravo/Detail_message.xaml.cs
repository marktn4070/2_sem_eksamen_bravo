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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for Detail_message.xaml
    /// </summary>
    public partial class Detail_message : Window
    {
        private string currentID;
        private string[] startAddress; //først vejnavn så kommunenavn
        private List<Customer> Customer_list = new List<Customer>();


        public Detail_message(Message message)
        {
            InitializeComponent();
            LoadGrid_Customer();
            Refresh();
            Clear();



            if (message.Email == true)
            {
                Email.IsChecked = true;
            }
            if (message.Sms == true)
            {
                Sms.IsChecked = true;
            }
            currentID = message.MessageID;
            Headline.Content = message.Headline;
            Subheadline.Content = message.Subheadline;
            Text.Content = message.Text;
            Time.Content = message.Time;
            if (message.Email == true)
            {
                Email.IsChecked = true;
            }
            if (message.Sms == true)
            {
                Sms.IsChecked = true;
            }
        }









        public void LoadGrid_Customer()
        {
            try
            {
                Customer_list = SQL.GetCustomerGotMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public CancelEventHandler Closing { get; private set; }

        private void Refresh()
        {
            datagrid_customer.ItemsSource = new ObservableCollection<Customer>(Customer_list);
        }






        private void Clear()
        {
            datagrid_customer.SelectedIndex = -1;
            LoadGrid_Customer();
        }





        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }










        //private void datagrid_customer()
        //{

        //}




    }
}
