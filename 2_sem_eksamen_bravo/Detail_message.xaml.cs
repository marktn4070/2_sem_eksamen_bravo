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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions; // Skal bruges for at kunne bruge Regex
using System.Configuration;

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for Detail_message.xaml
    /// </summary>
    public partial class Detail_message : Window
    {
        private string currentID;
        private string[] startAddress; //først vejnavn så kommunenavn

        public Detail_message(Message message)
        {
            InitializeComponent();



            if (message.Email == true)
            {
                Email.IsChecked = true;
            }
            if (message.Sms == true)
            {
                Sms.IsChecked = true;
            }
            currentID = message.MessageID;
            Headline.Content = message.Headline;
            Subheadline.Content = message.Subheadline;
            Text.Content = message.Text;
            Time.Content = message.Time;
            if (message.Email == true)
            {
                Email.IsChecked = true;
            }
            if (message.Sms == true)
            {
                Sms.IsChecked = true;
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            DataChangedEventHandler handler = DataChanged;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;



    }
}
