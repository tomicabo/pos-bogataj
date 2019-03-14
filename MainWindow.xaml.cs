using Blagajna_BogatajCementniIzdelki_v2.Models;
using Blagajna_BogatajCementniIzdelki_v2.Print;
using Blagajna_BogatajCementniIzdelki_v2.ViewModels;
using Blagajna_BogatajCementniIzdelki_v2.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace Blagajna_BogatajCementniIzdelki_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int id_izbranega_dokumenta;
        int st_racuna;
        int st_dobavnice;
        int st_ponudbe;

        int checkbox_izbran = 0;

        string query_tipplacila;

        public MainWindow()
        {
            InitializeComponent();
            lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
        }

        private void lv_dokumenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lv_dokumenti.SelectedIndex != -1)
            {
                DokumentiModel izbran = lv_dokumenti.SelectedItem as DokumentiModel;

                id_izbranega_dokumenta = izbran.Id;

                if (izbran.UstvarjenRacun != "")
                    st_racuna = int.Parse(izbran.UstvarjenRacun);
                else st_racuna = 0;
                if (izbran.UstvarjenaDobavnica != "")
                    st_dobavnice = int.Parse(izbran.UstvarjenaDobavnica);
                else st_dobavnice = 0;
                if (izbran.UstvarjenaPonudba != "")
                    st_ponudbe = int.Parse(izbran.UstvarjenaPonudba);
                else st_ponudbe = 0;

                btn_uredi_dokument.IsEnabled = true;
                btn_izbrisi_dokument.IsEnabled = true;
                btn_izdaj_natisni_racun.IsEnabled = true;
                btn_izdaj_natisni_dobavnico.IsEnabled = true;
                btn_izdaj_natisni_ponudbo.IsEnabled = true;
            }

            else
            {
                btn_uredi_dokument.IsEnabled = false;
                btn_izbrisi_dokument.IsEnabled = false;
                btn_izdaj_natisni_racun.IsEnabled = false;
                btn_izdaj_natisni_dobavnico.IsEnabled = false;
                btn_izdaj_natisni_ponudbo.IsEnabled = false;
            }
        }

        private void btn_ustvari_dokument_Click(object sender, RoutedEventArgs e)
        {
            UstvariDokument okno = new UstvariDokument(0);
            okno.Owner = this;
            okno.ShowDialog();

            if (okno.DialogResult == true)
            {
                lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                cb_tip_dokumentov.SelectedIndex = checkbox_izbran;
            }
        }

        private void btn_uredi_dokument_Click(object sender, RoutedEventArgs e)
        {
            UstvariDokument okno = new UstvariDokument(id_izbranega_dokumenta);
            okno.Owner = this;
            okno.ShowDialog();

            if (okno.DialogResult == true)
            {
                lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                cb_tip_dokumentov.SelectedIndex = checkbox_izbran;

            }
        }


        private void btn_artikli_okno_Click(object sender, RoutedEventArgs e)
        {
            Artikli okno = new Artikli(0);
            okno.Owner = this;
            okno.ShowDialog();
        }

        private void btn_izdaj_natisni_racun_Click(object sender, RoutedEventArgs e)
        {
            if (st_racuna == 0)
            {
                IzdajRacun okno = new IzdajRacun();
                okno.ShowDialog();

                if (okno.DialogResult == true)
                {
                    string rok_placila = okno.RokPlacila;
                    int tip_placila = okno.TipPlacila;

                    if (tip_placila == 0)
                        query_tipplacila = ", tip_placila = 0";
                    else if (tip_placila == 1)
                        query_tipplacila = ", tip_placila = 1, rok_placila = '" + rok_placila + "'";

                    var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

                        try
                        {
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.ConnectionString = connectionString;

                                var queryGetRacunId = "select max(ustvarjen_racun+1) from dokumenti where YEAR(ustvarjeno) = YEAR(CURDATE());";

                                connection.Open();

                                MySqlCommand commandgetid = new MySqlCommand(queryGetRacunId, connection);
                                int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                                var queryUpdateRacun = "update dokumenti set ustvarjen_racun = " + maxid + query_tipplacila + " where id = " + id_izbranega_dokumenta + ";";

                                using (MySqlCommand cmd = new MySqlCommand(queryUpdateRacun, connection))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                connection.Close();
                            }

                            lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                            cb_tip_dokumentov.SelectedIndex = checkbox_izbran;
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                }
            }
            else
            {
                //Natisni
                NatisniRacun racun = new NatisniRacun();
            }
        }

        private void btn_izbrisi_dokument_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            MessageBoxResult result = MessageBox.Show("Ali želite izbrisati dokument?", "Izbriši dokument", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.ConnectionString = connectionString;
                        var queryUpdateIzbrisano = "update dokumenti set izbrisano = 1 where id = " + id_izbranega_dokumenta + ";";

                        connection.Open();

                        using (MySqlCommand cmd = new MySqlCommand(queryUpdateIzbrisano, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                        MessageBox.Show("Dokument je bil izbrisan");
                    }

                    lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                    cb_tip_dokumentov.SelectedIndex = checkbox_izbran;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cb_tip_dokumentov_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_tip_dokumentov.SelectedIndex != -1)
            {
                checkbox_izbran = cb_tip_dokumentov.SelectedIndex;
                if (checkbox_izbran == 0)
                    lv_dokumenti.DataContext = new DokumentiViewModel(0);
                else if (checkbox_izbran == 1)
                    lv_dokumenti.DataContext = new DokumentiViewModel(1);
                else if (checkbox_izbran == 2)
                    lv_dokumenti.DataContext = new DokumentiViewModel(2);
                else if (checkbox_izbran == 3)
                    lv_dokumenti.DataContext = new DokumentiViewModel(3);
            }
        }

        private void btn_izdaj_natisni_dobavnico_Click(object sender, RoutedEventArgs e)
        {
            if (st_dobavnice == 0)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

                MessageBoxResult result = MessageBox.Show("Ali želite izdati dobavnico?", "Izdaj dobavnico", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.ConnectionString = connectionString;

                            var queryGetDobavnicaId = "select max(ustvarjena_dobavnica+1) from dokumenti where YEAR(ustvarjeno) = YEAR(CURDATE());";

                            connection.Open();

                            MySqlCommand commandgetid = new MySqlCommand(queryGetDobavnicaId, connection);
                            int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                            var queryUpdateDobavnica = "update dokumenti set ustvarjena_dobavnica = " + maxid + " where id = " + id_izbranega_dokumenta + ";";

                            using (MySqlCommand cmd = new MySqlCommand(queryUpdateDobavnica, connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            connection.Close();
                        }

                        lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                        cb_tip_dokumentov.SelectedIndex = checkbox_izbran;
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            else
            {
                //Natisni
                
            }
        }

        private void btn_izdaj_natisni_ponudbo_Click(object sender, RoutedEventArgs e)
        {
            if (st_ponudbe == 0)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

                MessageBoxResult result = MessageBox.Show("Ali želite izdati ponudbo?", "Izdaj ponudbo", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.ConnectionString = connectionString;

                            var queryGetPonudbaId = "select max(ustvarjena_ponudba+1) from dokumenti where YEAR(ustvarjeno) = YEAR(CURDATE());";

                            connection.Open();

                            MySqlCommand commandgetid = new MySqlCommand(queryGetPonudbaId, connection);
                            int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                            var queryUpdatePonudba = "update dokumenti set ustvarjena_ponudba = " + maxid + " where id = " + id_izbranega_dokumenta + ";";

                            using (MySqlCommand cmd = new MySqlCommand(queryUpdatePonudba, connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            connection.Close();
                        }

                        lv_dokumenti.DataContext = new DokumentiViewModel(checkbox_izbran);
                        cb_tip_dokumentov.SelectedIndex = checkbox_izbran;
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            else
            {
                //Natisni
            }
        }
    }
}
