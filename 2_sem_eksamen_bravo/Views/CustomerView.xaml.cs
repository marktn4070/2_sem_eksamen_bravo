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
using System.Text.RegularExpressions;






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
            cb_LoadName();
            LoadGrid_Cusumer();
        }





        private void Btn_OpenCreateCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomer Window = new CreateCustomer();
            Window.Show();

        }








        //SqlConnection host = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

        SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);
        public CancelEventHandler Closing { get; private set; }

        private void Refresh()
        {
           datagrid_customer.ItemsSource = new ObservableCollection<Customer_strings>(Customer_list);
       }


        private class Customer_strings
        {
            public string C_id { get; set; }
            public string C_FirstName { get; set; }
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
                while (sdr.Read()) Customer_list.Add(new Customer_strings { C_id = sdr[0].ToString(), C_FirstName = sdr[1].ToString(), C_LastName = sdr[2].ToString(), C_Registered = sdr[3].ToString(), C_Gender = sdr[4].ToString(), C_Birth = sdr[5].ToString(), C_Phone = sdr[6].ToString(), C_Email = sdr[7].ToString() });
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
                string C_FirstName_sting = Customer_list[n].C_FirstName;
                string C_LastName_sting = Customer_list[n].C_LastName;
                string C_Registered_sting = Customer_list[n].C_Registered;
                string C_Gender_sting = Customer_list[n].C_Gender;
                string C_Birth_sting = Customer_list[n].C_Birth;
                string C_Phone_sting = Customer_list[n].C_Phone;
                string C_Email_sting = Customer_list[n].C_Email;

                Update_customer win2 = new Update_customer(C_id_sting, C_FirstName_sting, C_LastName_sting, C_Registered_sting, C_Gender_sting, C_Birth_sting, C_Phone_sting, C_Email_sting);
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
            string selected_name = Customer_list[datagrid_customer.SelectedIndex].C_FirstName;
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
            //string name_txt = cb_Search.Text.ToString();
            string name_txt = Search_txt.Text.ToString();

            if (name_txt != string.Empty)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE FirstName + ' ' + LastName like '%" + name_txt + "%' or FirstName like '%" + name_txt + "%' or LastName like '%" + name_txt + "%'", host);
                DataTable dt = new DataTable();
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                host.Close();
                datagrid_customer.ItemsSource = dt.DefaultView;
                //cb_Search.Background = Brushes.Transparent;
                //string name_txt = name_txt;
                ////name_txt = "";

                int Search_items = datagrid_customer.Items.Count;

                if (Search_items == 0)
                {
                    Search_message.Content = "Der er ingen resultater på din søgningen";
                }
                else if (Search_items == 1)
                {
                    Search_message.Content = Search_items + " resultat på søgningen af '" + name_txt + "'";
                    Search_message.Foreground = Brushes.Black;
                }
                else
                {
                    Search_message.Content = Search_items + " resultater på søgningen af '" + name_txt + "'";
                    Search_message.Foreground = Brushes.Black;
                }



                //ClearDataBtn.Visibility = Visibility.Visible;
                //SearchDataBtn.Visibility = Visibility.Hidden;

            }
            else
            {

                Search_message.Content = "Der er ingen tekst i søgningsfeltet";
                Search_message.Foreground = Brushes.Red;
            }
        }

        private void cb_LoadName() //Kevin
        {
            cb_Search.ItemsSource = SQL.GetCustomerName();



            if (cb_Search.SelectedItem == null)
            {
                // do something
                Search_test.Content = cb_Search.SelectedItem;
            }

        }

        //private void datagrid_customer()
        //{

        //}
    }
}
