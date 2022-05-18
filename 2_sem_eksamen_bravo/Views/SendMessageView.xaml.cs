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

namespace _2_sem_eksamen_bravo.Views
{
    /// <summary>
    /// Interaction logic for SendMessageView.xaml
    /// </summary>
    public partial class SendMessageView : UserControl
    {
        public SendMessageView()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (((bool)Email.IsChecked || (bool)Sms.IsChecked) && (Headline.Text != string.Empty && Message.Text != string.Empty))
            {
                if ((bool)Email.IsChecked)
                {
                    MessageEmulator.EmulateSendEmail(Headline.Text, Subheadline.Text, Message.Text);
                }
                if ((bool)Sms.IsChecked)
                {
                    MessageEmulator.EmulateSendSms(Headline.Text, Subheadline.Text, Message.Text);
                }
                int howManyReceived = MessageEmulator.SaveMessage(Headline.Text, Subheadline.Text, Message.Text, (bool)Sms.IsChecked, (bool)Email.IsChecked);
                ClearAll();
                MessageBox.Show(string.Format("Sendt til {0} modtagere!", howManyReceived));
            }
            else
            {

            }

        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            Headline.Clear();
            Subheadline.Clear();
            Message.Clear();
            Sms.IsChecked = false;
            Email.IsChecked = false;
        }
    }
}
