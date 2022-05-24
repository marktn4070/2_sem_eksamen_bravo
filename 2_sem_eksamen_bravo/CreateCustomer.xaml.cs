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

namespace _2_sem_eksamen_bravo
{
    /// <summary>
    /// Interaction logic for CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window
    {
        public CreateCustomer()
        {
            InitializeComponent();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            //validation here
            if (FirstName.Text == string.Empty || LastName.Text == string.Empty || Birthday.Text == string.Empty || Birthmonth.Text == string.Empty ||
                Birthyear.Text == string.Empty || Phone.Text == string.Empty ||Email.Text == string.Empty || Zip.Text == string.Empty || Road.Text == string.Empty)
            {

            }
            try
            {
                int.Parse(Birthday.Text);
                int.Parse(Birthmonth.Text);
                int.Parse(Birthyear.Text);
            }
            catch
            {
                MessageBox.Show("Dato skal være i format 31-12-2000");
            }
            /*
            string birth = "";
            if (Birthday.Text.Length == 2 && Birthday.Text[0])
            birth = Birthday.Text + "-" + Birthmonth.Text + "-" + Birthyear.Text;
            string gender = "";
            SQL.RegisterCustomer(FirstName.Text, LastName.Text, (bool)Registered.IsChecked, gender, birth, int.Parse(Phone.Text), Email.Text, int.Parse(Zip.Text), Road.Text);
        */
            }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
