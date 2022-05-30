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
            LoadGrid_Customer();
            Refresh();
            Clear();
        }



        private void Btn_OpenCreateCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomer Window = new CreateCustomer();
            Window.DataChanged += CreateCustomer_Created;
            Window.ShowDialog();

        }



        //SqlConnection host = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

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





        public void LoadGrid_Customer()
        {                
            try
            {
                Customer_list = SQL.GetCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                win2.ShowDialog();
            }
        }
        private void btn_Detail_Click(object sender, RoutedEventArgs e)
        {
            int n = datagrid_customer.SelectedIndex;
            if (n >= 0)
            {
                Detail_customer win2 = new Detail_customer(Customer_list[n]);
                win2.DataChanged += UpdateCustomer_Updated;
                win2.ShowDialog();
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
                    Refresh();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                LoadGrid_Customer();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e) //til sql klasse
        {
            //string name_txt = cb_Search.Text.ToString();
            string name_txt = cb_Search.Text.ToString();

            if (name_txt != string.Empty)
            {
                DataTable dt = SQL.SearchCustomer(name_txt);
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
            LoadGrid_Customer();
            Refresh();
            Clear();
        }

        private void UpdateCustomer_Updated(object sender, EventArgs e)
        {
            LoadGrid_Customer();
            Refresh();
            Clear();
        }

        //private void datagrid_customer()
        //{

        //}
    }
}
