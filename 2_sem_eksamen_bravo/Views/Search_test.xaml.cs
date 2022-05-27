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

using _2_sem_eksamen_bravo.ViewModels;

using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Configuration;

namespace _2_sem_eksamen_bravo.Views
{
    /// <summary>
    /// Interaction logic for Search_test.xaml
    /// </summary>
    public partial class Search_test : UserControl
    {
        private List<Customer_strings> Customer_list = new List<Customer_strings>();

        public Search_test()
        {
            InitializeComponent();
            LoadGrid_Cusumer();
        }

        //SqlConnection data = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

        SqlConnection data = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);

        private void Refresh()
        {
            datagrid_customer.ItemsSource = new ObservableCollection<Customer_strings>(Customer_list);
        }


        private class Customer_strings
        {
            public string CustomerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Registered { get; set; }
            public string Gender { get; set; }
            public string Birth { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", data);
                DataTable dt = new DataTable();
                data.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                Customer_list.Clear();
                while (sdr.Read()) Customer_list.Add(new Customer_strings { CustomerID = sdr[0].ToString(), FirstName = sdr[1].ToString(), LastName = sdr[2].ToString(), Registered = sdr[3].ToString(), Gender = sdr[4].ToString(), Birth = sdr[5].ToString(), Phone = sdr[6].ToString(), Email = sdr[7].ToString() });
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (data != null) data.Close();
            }
        }


        private void Btn_Runner_Search_Click(object sender, RoutedEventArgs e)
        {
            if (Search_txt.Text != string.Empty)
            {
                string name_txt = Search_txt.Text;
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE FirstName + ' ' + LastName like '%" + name_txt + "%' or FirstName like '%" + name_txt + "%' or LastName like '%" + name_txt + "%'", data);
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE FirstName like '%" + name_txt + "%' or CustomerID like '%" + name_txt + "%'", data);
                DataTable dt = new DataTable();
                data.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                data.Close();
                datagrid_customer.ItemsSource = dt.DefaultView;
                Search_txt.Background = Brushes.Transparent;
                string runner_txt = Search_txt.Text;
                Search_txt.Text = "";

                int Search_items = datagrid_customer.Items.Count;

                if (Search_items == 0)
                {
                    Search_message.Content = "Der er ingen resultater på din søgningen";
                }
                else if (Search_items == 1)
                {
                    Search_message.Content = Search_items + " resultat på søgningen af '" + runner_txt + "'";
                }
                else
                {
                    Search_message.Content = Search_items + " resultater på søgningen af '" + runner_txt + "'";
                }

                ClearDataBtn.Visibility = Visibility.Visible;
                SearchDataBtn.Visibility = Visibility.Hidden;
            }
        }


        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            Search_txt.Clear();
            LoadGrid_Cusumer();
            ClearDataBtn.Visibility = Visibility.Hidden;
            SearchDataBtn.Visibility = Visibility.Visible;
            Search_txt.Background = (Brush)new BrushConverter().ConvertFrom("#fff");
        }


        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = datagrid_customer.SelectedIndex;
        }


        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }


        private void Search_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Search_txt.Text = "";
            if (Search_txt.Text != "")
            {
                Search_txt.Background = (Brush)new BrushConverter().ConvertFrom("#fff");

                ClearDataBtn.Visibility = Visibility.Hidden;
                SearchDataBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
