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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;

namespace _2_sem_eksamen_bravo.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xamldcs
    /// </summary>
    public partial class CustomerView : UserControl
    {

        private List<Customer_strings> Customer_list = new List<Customer_strings>();
        public CustomerView()
        {
            InitializeComponent();
            LoadGrid_Cusumer();
            cb_LoadName();
        }





        private void Btn_OpenCreateCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomer Window = new CreateCustomer();
            Window.DataChanged += CreateCustomer_Created;
            Window.Show();

        }








        //SqlConnection host = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

        SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
        public CancelEventHandler Closing { get; private set; }

        private void Refresh()
        {
           datagrid_customer.ItemsSource = new ObservableCollection<Customer_strings>(Customer_list);
           //datagrid_customer.DataContext = new ObservableCollection<Customer_strings>(Customer_list);
       }


        private class Customer_strings
        {
            public string C_id { get; set; }
            public string C_firstName { get; set; }
            public string C_LastName { get; set; }
            public string C_Registered { get; set; }
            public string C_Gender { get; set; }
            public string C_Birth { get; set; }
            public string C_Phone { get; set; }
            public string C_Email { get; set; }
        }



        private void Clear()
        {
            datagrid_customer.SelectedIndex = -1;
            LoadGrid_Cusumer();
        }





        public void LoadGrid_Cusumer()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", host);
                DataTable dt = new DataTable();
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                Customer_list.Clear();
                while (sdr.Read()) Customer_list.Add(new Customer_strings { C_id = sdr[0].ToString(), C_firstName = sdr[1].ToString(), C_LastName = sdr[2].ToString(), C_Registered = sdr[3].ToString(), C_Gender = sdr[4].ToString(), C_Birth = sdr[5].ToString(), C_Phone = sdr[6].ToString(), C_Email = sdr[7].ToString() });
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (host != null) host.Close();
            }
        }



        private void datagrid_customer_changed(object sender, SelectionChangedEventArgs e)
        {
            int n = datagrid_customer.SelectedIndex;
        }



        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            int n = datagrid_customer.SelectedIndex;
            if (n >= 0)
            {
                string C_id_sting = Customer_list[n].C_id;
                string C_firstName_sting = Customer_list[n].C_firstName;
                string C_LastName_sting = Customer_list[n].C_LastName;
                string C_Registered_sting = Customer_list[n].C_Registered;
                string C_Gender_sting = Customer_list[n].C_Gender;
                string C_Birth_sting = Customer_list[n].C_Birth;
                string C_Phone_sting = Customer_list[n].C_Phone;
                string C_Email_sting = Customer_list[n].C_Email;

                Update_customer win2 = new Update_customer(C_id_sting, C_firstName_sting, C_LastName_sting, C_Registered_sting, C_Gender_sting, C_Birth_sting, C_Phone_sting, C_Email_sting);
                win2.Show();
            }
        }


        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }




        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

            string selected_id = Customer_list[datagrid_customer.SelectedIndex].C_id;
            string selected_name = Customer_list[datagrid_customer.SelectedIndex].C_firstName;
            int n = datagrid_customer.SelectedIndex;

            var Result = MessageBox.Show("Er du sikker på, at du vil slette deltageren '" + selected_name + "'?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (Result == MessageBoxResult.Yes)
            {
                try
                {
                    SQL.DeleteCustomer(selected_id);
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                LoadGrid_Cusumer();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //cb_Search.SelectedItem
        }

        private void cb_LoadName() //Kevin
        {
            cb_Search.ItemsSource = SQL.GetCustomerName();
        }

        private void CreateCustomer_Created(object sender, EventArgs e)
        {
            Clear();
        }

        //private void datagrid_customer()
        //{

        //}
    }
}
