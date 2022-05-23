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

namespace _2_sem_eksamen_bravo.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {

        //private List<Customer_strings> Customer_list = new List<Customer_strings>();
        public CustomerView()
        {
            InitializeComponent();
        }

        private void Btn_OpenCreateCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomerView vindue = new CreateCustomerView();
            vindue.Show();
        }
        //SqlConnection host = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");




        //private void Refresh()
        //{
        //    datagrid_customer.ItemsSource = new ObservableCollection<Customer_strings>(Customer_list);
        //}


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





        //public void LoadGrid_Cusumer()
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", host);
        //        DataTable dt = new DataTable();
        //        host.Open();
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        Customer_list.Clear();
        //        while (sdr.Read()) Customer_list.Add(new Customer_strings { C_id = sdr[0].ToString(), C_firstName = sdr[1].ToString(), C_LastName = sdr[2].ToString(), C_Registered = sdr[3].ToString(), C_Gender = sdr[4].ToString(), C_Birth = sdr[5].ToString(), C_Phone = sdr[6].ToString(), C_Email = sdr[6].ToString() });
        //        Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        if (host != null) host.Close();
        //    }
        //}





        //private void btnView_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Closing += new System.ComponentModel.CancelEventHandler(Update_runner_opdate);
        //    this.Close();
        //}


        //private void Update_runner_opdate(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    int n = datagrid_customer.SelectedIndex;
        //    if (n >= 0)
        //    {
        //        string P_id_sting = Customer_list[n].P_id;
        //        string P_name_sting = Customer_list[n].P_name;
        //        string P_mail_sting = Customer_list[n].P_mail;
        //        string P_phone_sting = Customer_list[n].P_phone;
        //        string P_address_sting = Customer_list[n].P_address;
        //        string P_zip_sting = Customer_list[n].P_zip;
        //        string P_city_sting = Customer_list[n].P_city;
        //        Update_runner win2 = new Update_runner(P_id_sting, P_name_sting, P_mail_sting, P_phone_sting, P_address_sting, P_zip_sting, P_city_sting);
        //        win2.Show();
        //    }
        //}



        //private void btnDelete_Click_2(object sender, RoutedEventArgs e)
        //{
        //    string error = "";
        //    string selected_id = Customer_list[datagrid_customer.SelectedIndex].P_id;
        //    string selected_name = Customer_list[datagrid_customer.SelectedIndex].P_name;
        //    int n = datagrid_customer.SelectedIndex;

        //    var Result = MessageBox.Show("Er du sikker på, at du vil slette deltageren '" + selected_name + "'?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    if (Result == MessageBoxResult.Yes)
        //    {
        //        SqlConnection connection = null;
        //        try
        //        {
        //            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["data"].ConnectionString);
        //            SqlCommand command = new SqlCommand("Delete FROM Participant WHERE P_id = @P_id", connection);
        //            command.Parameters.Add(CreateParam("@P_id", selected_id.Trim(), SqlDbType.NVarChar));
        //            connection.Open();
        //            if (command.ExecuteNonQuery() == 1)
        //            {
        //                Clear();
        //                return;
        //            }
        //            error = "Illegal database operation";
        //        }
        //        catch (Exception ex)
        //        {
        //            error = ex.Message;
        //        }
        //        finally
        //        {
        //            if (connection != null) connection.Close();
        //        }
        //        MessageBox.Show(error);
        //    }
        //    else if (Result == MessageBoxResult.No)
        //    {
        //        LoadGrid_Runner();
        //    }
        //}

    }
}
