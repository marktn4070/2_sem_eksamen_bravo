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
using System.IO;
using SqlBulkTools;

namespace _2_sem_eksamen_bravo.Views
{
    /// <summary>
    /// Interaction logic for MessagesView.xaml
    /// </summary>
    public partial class MessagesView : UserControl
    {
        private List<Message> Message_list = new List<Message>();

        public MessagesView()
        {
            InitializeComponent();
            LoadGrid_Message();
            Refresh();
            Clear();
        }

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
                Message_list = SQL.GetMMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void datagrid_message_changed(object sender, SelectionChangedEventArgs e)
        {
            int n = datagrid_message.SelectedIndex;
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
    

        private void Search_Click(object sender, RoutedEventArgs e) //til sql klasse
        {
            if (startDate_Search.SelectedDate != null && endDate_Search.SelectedDate != null)
            {
            DateTime SD = startDate_Search.SelectedDate.Value;
            string startDate = SD.ToString("yyyy/MM/dd");
            DateTime ED = endDate_Search.SelectedDate.Value;
            string endDate = ED.ToString("yyyy/MM/dd");
            try
            {
                if (DateTime.Compare(SD, ED) <= 0)
                {
                    DataTable dt = SQL.SearchMessage(startDate, endDate);
                    datagrid_message.ItemsSource = dt.DefaultView;
                    //time_Search.Background = Brushes.Transparent;
                    //string name_txt = name_txt;
                    ////name_txt = "";

                    int Search_items = datagrid_message.Items.Count;

                    if (Search_items == 0)
                    {
                    Search_message.Content = "Der er ingen resultater på din søgningen";
                    }
                    else if (Search_items == 1)
                    {
                    Search_message.Content = Search_items + " resultat af søgningen fra '" + startDate + "' til '" + endDate + "'";
                    Search_message.Foreground = Brushes.Black;
                    }
                    else if (Search_items > 1)
                    {
                        Search_message.Content = Search_items + " resultater af søgningen fra '" + startDate + "' til '" + endDate + "'";
                        Search_message.Foreground = Brushes.Black;
                    }
                    //ClearDataBtn.Visibility = Visibility.Visible;
                    //SearchDataBtn.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        //måske skal beskeden ændres
                        //throw new ArgumentException("slut datoerne er før start datoerne");
                        Search_message.Content = "Slut datoen er før start datoen";
                        Search_message.Foreground = Brushes.Red;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    }
                }
                else if (startDate_Search.SelectedDate != null && endDate_Search.SelectedDate == null ||
                startDate_Search.SelectedDate == null && endDate_Search.SelectedDate != null)
                {
                    Search_message.Content = "Der er ingen dato i en af søgningsfelteterne";
                    Search_message.Foreground = Brushes.Red;
                }
            else
            {
                Search_message.Content = "Der er ingen dato i søgningsfelteterne";
                Search_message.Foreground = Brushes.Red;
            }

        }




        SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);

        public void GetMMessage_not_done()
        {
            //List<string> Headline = SQL.GetMMessage_2();
            var products = new List<Product>();

            for (int i = 0; i < 5; i++)
            {
                int j = i + 1;
                //products.Add(new Product("Product 1", 205.46, "/Assets/1.jpg"));
                products.Add(new Product("Headline " + j, "Subheadline", "Time"));
            }

            //if (products.Count > 0)
                //ListViewProducts.ItemsSource = products;
        }
    }



    public class Product
    {
        public string Headline_Btn { get; set; }
        public string Subheadline { get; set; }
        public string Time { get; set; }

        public Product(string headline_btn, string subheadline, string time)
        {
            Headline_Btn = headline_btn;
            Subheadline = subheadline;
            Time = time;
        }
    }
}
