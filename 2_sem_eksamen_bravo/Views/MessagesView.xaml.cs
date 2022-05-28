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
    /// Interaction logic for MessagesView.xaml
    /// </summary>
    public partial class MessagesView : UserControl
    {
        public MessagesView()
        {
            List<string> Headline = SQL.GetMMessage();
            InitializeComponent();
            //Headline_txt.ItemsSource = Headline;
            //teest.ItemsSource = Headline;



            var products = new List<Product>();

            for (int i = 0; i < 30; i++)
            {
                int j = i + 1;
                //products.Add(new Product("Product 1", 205.46, "/Assets/1.jpg"));
                products.Add(new Product("Headline " + j, "Subheadline", "Time"));
            }



            if (products.Count > 0)
                ListViewProducts.ItemsSource = products;



            //int i = 0;
            //while (i < 5)
            //{
            //    //string_1.Content = "test sring";
            //    i++;
            //}

            //for (int i = 0; i < listBox11.Items.Count; ++i)
            //{
            //    DataRowView drv = listBox11.Items[i] as DataRowView;
            //    if (drv != null)
            //    {
            //        if (!listBox11.SelectedItems.Contains(drv))
            //        {
            //            DataRow dr = drv.Row;
            //            //...
            //        }
            //    }
            //}


            //foreach (var item in collection)
            //{



            //for (int j = 0; j < 4; j++)
            //{
            //    //int j2 =
            //    for (int i = 0; i < 4; i++)
            //    {
            //        Button lbl = new Button { FontSize = 10, Foreground = Brushes.Black, Background = Brushes.White, Content = "name" + i, Height = 50, Width = 50, Name = "_" + i };
            //        lbl.SetValue(Grid.RowProperty, j);
            //        lbl.SetValue(Grid.ColumnProperty, i);

            //        this.LayoutRoot.Children.Add(lbl);

            //    }
            //}





            //for (int i = 0; i < 3; i++)
            //{
            //    Button lbl = new Button { FontSize = 10, Foreground = Brushes.Black, VerticalContentAlignment = System.Windows.VerticalAlignment.Top, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, Background = Brushes.White, Content = "name" + i, Height = 50, Width = 50, Name = "_" + i };
            //    lbl.SetValue(Grid.RowProperty, 2);
            //    lbl.SetValue(Grid.ColumnProperty, i);

            //    this.LayoutRoot.Children.Add(lbl);

            //}
            //var products = GetProducts();

        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
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

//SqlConnection host = new SqlConnection(ConfigurationManager.ConnectionStrings["host"].ConnectionString);


//        private void Button_Click(object sender, RoutedEventArgs e)
//        {

//            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM Message", host);
//            host.Open();
//                DataTable dt = new DataTable();
//                cmd.Fill(dt);

//            foreach (DataRow row in dt.Rows)
//            {
//                listBox1.Items.Add(row["Headline"].ToString()); 
//            }
//        //}

//        }


            //string Hello = "2";


        //private void Kommune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //    //if (Kommune.SelectedItem != null)
        //    //    //{
        //    //    //    Vej.IsEnabled = true;
        //    //    //    Vej.ItemsSource = SQL.GetRoads(Kommune.SelectedItem.ToString());
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    Vej.IsEnabled = false;
        //    //    //    Vej.ItemsSource = new List<string>();
        //    //    //}
        //}



        //private void button1_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
