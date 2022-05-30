﻿using System;
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


namespace _2_sem_eksamen_bravo
{
    /// <summary>
/////// HEAD
    /// Interaction logic for MainWindow.xaml
//////
    /// Interaction logic for MainWindow.xamlerferfedd
////// master
    /// Interaction logic for MainWindow.xaml
////// Temporary merge branch 2
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                SQL.AdresseImpoter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitializeComponent();
            DataContext = new MainViewModel();
            this.MainContent.Content = new ViewModels.CustomerViewModel();
        }

        private void Menu_Click_1(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new ViewModels.CustomerViewModel();
        }

        private void Menu_Click_2(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new ViewModels.MessagesViewModel();
        }

        private void Menu_Click_3(object sender, RoutedEventArgs e)
        {
            this.MainContent.Content = new ViewModels.SendMessageViewModel();
        }

        public static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        //private void ImpoterAddresser_Click(object sender, RoutedEventArgs e) //Kevin
        //{
        //    SQL.AdresseImpoter();
        //}
    }
}