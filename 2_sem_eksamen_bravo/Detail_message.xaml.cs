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
    #region Coded by Mark
    public partial class Detail_message : Window
    {
        private int currentID;
        private List<Customer> Customer_list = new List<Customer>();


        public Detail_message(Message message)
        {
            currentID = message.MessageID;
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
            
            Headline.Text = message.Headline;
            Subheadline.Text = message.Subheadline;
            Text.Text = message.Text;
            Time.Text = message.Time;
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
                Customer_list = SQL.GetCustomerGotMessage(currentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public CancelEventHandler Closing { get; private set; }

        private void Refresh()
        {
            datagrid_message_receivers.ItemsSource = new ObservableCollection<Customer>(Customer_list);
        }

        private void Clear()
        {
            datagrid_message_receivers.SelectedIndex = -1;
            LoadGrid_Customer();
        }
    }
#endregion
}
