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

namespace Blagajna_BogatajCementniIzdelki_v2.Views
{
    /// <summary>
    /// Interaction logic for IzdajRacun.xaml
    /// </summary>
    public partial class IzdajRacun : Window
    {
        public string RokPlacila { get; set; }
        public int TipPlacila { get; set; }

        public IzdajRacun()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (rb_gotovina.IsChecked == true)
            {
                TipPlacila = 0;

                MessageBoxResult result = MessageBox.Show("Ali želite izdati račun?", "Izdaj Račun", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    this.DialogResult = true;
                }
            }
            else if (rb_transakcijski.IsChecked == true)
            {
                TipPlacila = 1;
                if (dp_rok.SelectedDate != null)
                {
                    RokPlacila = dp_rok.SelectedDate.Value.ToString("yyyy-MM-dd");
                    MessageBoxResult result = MessageBox.Show("Ali želite izdati račun?", "Izdaj Račun", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.DialogResult = true;
                    }
                }
                else MessageBox.Show("Izberi datum");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void rb_gotovina_Unchecked(object sender, RoutedEventArgs e)
        {
            if (rb_transakcijski.IsChecked == true)
            {
                dp_rok.IsEnabled = true;
            }
        }

        private void rb_transakcijski_Unchecked(object sender, RoutedEventArgs e)
        {
            if (rb_gotovina.IsChecked == true)
            {
                dp_rok.IsEnabled = false;
            }
        }
    }
}
