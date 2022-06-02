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
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using SqlBulkTools;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for Detail_customer.xaml
    /// </summary>
    public partial class Detail_customer : Window
    {
        private string currentID;
        private List<Message> Message_list = new List<Message>();

        public Detail_customer(Customer customer)
        {
            InitializeComponent();

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
            Kommune.Text = customer.Municipality;
            Vej.Text = customer.Road;
            Zip.Text = customer.Zip;

            LoadGrid_Message();
            Refresh();
            Clear();
        }






        //SqlConnection host = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

        public CancelEventHandler Closing { get; private set; }

        private void Refresh()
        {
            datagrid_message.ItemsSource = new ObservableCollection<Message>(Message_list);
        }



        private void Clear()
        {
            datagrid_message.SelectedIndex = -1;
            LoadGrid_Message();
        }





        public void LoadGrid_Message()
        {
            try
            {
                Message_list = SQL.GetMessageSendToCustomer(int.Parse(currentID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btn_Detail_Click(object sender, RoutedEventArgs e)
        {
            int n = datagrid_message.SelectedIndex;
            if (n >= 0)
            {
                Detail_message win2 = new Detail_message(Message_list[n]);
                //win2.DataChanged += DetailMessage_Detaild;
                win2.ShowDialog();
            }
        }


        private void datagrid_message_changed(object sender, SelectionChangedEventArgs e)
        {
            int n = datagrid_message.SelectedIndex;
        }



        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }





        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

    }
}
