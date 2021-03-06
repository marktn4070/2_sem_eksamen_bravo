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


namespace _2_sem_eksamen_bravo
{
    /// <summary>
/////// HEAD
    /// Interaction logic for MainWindow.xaml
//////
    /// Interaction logic for MainWindow.xamlerfdsjcsddsedff
////// master
    /// Interaction logic for MainWindow.xaml
////// Temporary merge branch 2
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Adressimport();
            InitializeComponent();
            Menu_CustomerView.BorderBrush = Brushes.Black;
            MainWindow_Loaded("/Views/CustomerView.xaml");
        }

        #region Coded by Mark
        private void Menu_Click_1(object sender, RoutedEventArgs e)
        {
            Menu_CustomerView.BorderBrush = Brushes.Black;
            Menu_MessagesView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");
            Menu_SendMessageView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");

            string View = "/Views/CustomerView.xaml";
            MainWindow_Loaded(View);
        }

        private void Menu_Click_2(object sender, RoutedEventArgs e)
        {
            Menu_MessagesView.BorderBrush = Brushes.Black;
            Menu_CustomerView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");
            Menu_SendMessageView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");

            string View = "/Views/MessagesView.xaml";
            MainWindow_Loaded(View);
        }

        private void Menu_Click_3(object sender, RoutedEventArgs e)
        {
            Menu_SendMessageView.BorderBrush = Brushes.Black;
            Menu_MessagesView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");
            Menu_CustomerView.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCDDAFF");

            string View = "/Views/SendMessageView.xaml";
            MainWindow_Loaded(View);
        }

        private void MainWindow_Loaded(string View)
        {
            this.mainFrame.Navigate(new Uri(View, UriKind.Relative));
        }
        #endregion

        #region Coded by Kevin
        public void Adressimport()
        {
            try
            {
                SQL.AdresseImpoter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}