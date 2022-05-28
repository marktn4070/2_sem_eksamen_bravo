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

        private List<Customer> Customer_list = new List<Customer>();
        public CustomerView()
        {
            InitializeComponent();
            cb_LoadName();
            LoadGrid_Cusumer();
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
           datagrid_customer.ItemsSource = new ObservableCollection<Customer>(Customer_list);
       }


        



        private void Clear()
        {
            datagrid_customer.SelectedIndex = -1;
            LoadGrid_Cusumer();
        }





        public void LoadGrid_Cusumer()
        {                
            //SQL.GetMCustomer();

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", host);
                //SqlCommand cmd_2 = new SqlCommand("SELECT * FROM Address WHERE RoadcodeID LIKE @RoadcodeID", host);
                DataTable dt = new DataTable();
                host.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                //SqlDataReader sdr_2 = cmd_2.ExecuteReader();
                Customer_list.Clear();
                while (sdr.Read()) Customer_list.Add(new Customer { CustomerID = sdr[0].ToString(), FirstName = sdr[1].ToString(), LastName = sdr[2].ToString(), Registered = sdr[3].ToString(), Gender = sdr[4].ToString(), Birth = sdr[5].ToString(), Phone = sdr[6].ToString(), Email = sdr[7].ToString(), RoadcodeID = sdr[8].ToString() });
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
                Update_customer win2 = new Update_customer(Customer_list[n]);
                win2.DataChanged += UpdateCustomer_Updated;
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

            string selected_id = Customer_list[datagrid_customer.SelectedIndex].CustomerID;
            string selected_name = Customer_list[datagrid_customer.SelectedIndex].FirstName;
            int n = datagrid_customer.SelectedIndex;

            var Result = MessageBox.Show("Er du sikker på, at du vil slette kunden '" + selected_name + "'?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
            string name_txt = cb_Search.Text.ToString();

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



            //if (cb_Search.SelectedItem == null)
            //{
            //    // do something
            //    Search_test.Content = cb_Search.SelectedItem;
            //}

        }

        private void CreateCustomer_Created(object sender, EventArgs e)
        {
            Clear();
        }

        private void UpdateCustomer_Updated(object sender, EventArgs e)
        {
            Clear();
        }

        //private void datagrid_customer()
        //{

        //}
    }
}
