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





        private List<Participant_strings> Participant_list = new List<Participant_strings>();


        public Search_test()
        {
            InitializeComponent();


            LoadGrid_Runner();

        }





        SqlConnection data = new SqlConnection(@"Data Source=.;Initial Catalog=Golf; Integrated Security=True");

        //SqlConnection data = new SqlConnection(ConfigurationManager.ConnectionStrings["data"].ConnectionString);

        private void Refresh()
        {
            datagrid_deltager.ItemsSource = new ObservableCollection<Participant_strings>(Participant_list);
        }


        private class Participant_strings
        {
            public string P_id { get; set; }
            public string P_name { get; set; }
            public string P_mail { get; set; }
            public string P_phone { get; set; }
            public string P_address { get; set; }
            public string P_zip { get; set; }
            public string P_city { get; set; }
        }


      


        private void Clear()
        {
            datagrid_deltager.SelectedIndex = -1;
            LoadGrid_Runner();
        }




        public void LoadGrid_Runner()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Participant", data);
                DataTable dt = new DataTable();
                data.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                Participant_list.Clear();
                while (sdr.Read()) Participant_list.Add(new Participant_strings { P_id = sdr[0].ToString(), P_name = sdr[1].ToString(), P_mail = sdr[2].ToString(), P_phone = sdr[3].ToString(), P_address = sdr[4].ToString(), P_zip = sdr[5].ToString(), P_city = sdr[6].ToString() });
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

                string runner_id = Search_txt.Text;
                SqlCommand cmd = new SqlCommand("SELECT * FROM Participant WHERE P_name like '%" + runner_id + "%' or P_id like '%" + runner_id + "%'", data);
                DataTable dt = new DataTable();
                data.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                data.Close();
                datagrid_deltager.ItemsSource = dt.DefaultView;
                Search_txt.Background = Brushes.Transparent;
                string runner_txt = Search_txt.Text;
                Search_txt.Text = "";

                int Search_items = datagrid_deltager.Items.Count;

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
            LoadGrid_Runner();
            ClearDataBtn.Visibility = Visibility.Hidden;
            SearchDataBtn.Visibility = Visibility.Visible;
            Search_txt.Background = (Brush)new BrushConverter().ConvertFrom("#fff");
        }


        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = datagrid_deltager.SelectedIndex;
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
