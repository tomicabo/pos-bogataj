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

namespace Blagajna_BogatajCementniIzdelki_v2.Print
{
    /// <summary>
    /// Interaction logic for NatisniRacun.xaml
    /// </summary>
    public partial class NatisniRacun : Window
    {
        public NatisniRacun()
        {
            InitializeComponent();

            //PDF print
            try
            {
                PrintDialog printDlg = new PrintDialog();
                printDlg.PrintVisual(PrintGrid, "test.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
