using Blagajna_BogatajCementniIzdelki_v2.Models;
using Blagajna_BogatajCementniIzdelki_v2.ViewModels;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Artikli.xaml
    /// </summary>
    public partial class Artikli : Window
    {
        int id_artikla;
        public string koda;
        public string ime_artikla;
        public int teza;
        public decimal cena;

        byte[] slika_dodaj;
        byte[] slika_uredi;

        bool isci_kategorije = false;
        int kategorija;

        public Artikli(int a)
        {
            InitializeComponent();

            if (a == 0)
                btn_dodaj_v_racun.Visibility = Visibility.Hidden;
            else
                btn_dodaj_v_racun.Visibility = Visibility.Visible;

            cb_kategorije.DataContext = new KategorijeViewModel();
            lv_artikli_slika.DataContext = new ArtikliViewModel(0, "");
            lv_artikli.DataContext = new ArtikliViewModel(0, "");
            d_kategorija.DataContext = new KategorijeViewModel();
            u_kategorija.DataContext = new KategorijeViewModel();
        }

        private void btn_shrani_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            if (dodaj.IsSelected == true)
            {
                MessageBoxResult result = MessageBox.Show("Ali želite dodati artikel?", "Dodaj artikel", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.ConnectionString = connectionString;
                            var queryGetId = "select max(id)+1 from artikli;";

                            var queryInsertArtikel = "insert into artikli (id, koda, kategorija, ime_artikla, dimenzije, teza, cena, slika)" +
                                "values (@Id, @Koda, @Kategorija, @ImeArtikla, @Dimenzije, @Teza, @Cena, @Slika);";

                            connection.Open();

                            MySqlCommand commandgetid = new MySqlCommand(queryGetId, connection);
                            int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                            using (MySqlCommand cmd = new MySqlCommand(queryInsertArtikel, connection))
                            {
                                cmd.Parameters.AddWithValue("@Id", maxid);
                                if (d_koda.Text != "")
                                    cmd.Parameters.AddWithValue("@Koda", d_koda.Text);
                                else cmd.Parameters.AddWithValue("@Koda", "");
                                if (d_kategorija.SelectedIndex != 0)
                                    cmd.Parameters.AddWithValue("@Kategorija", d_kategorija.SelectedIndex);
                                else cmd.Parameters.AddWithValue("@Kategorija", 0);
                                if (d_ime_artikla.Text != "")
                                    cmd.Parameters.AddWithValue("@ImeArtikla", d_ime_artikla.Text);
                                else cmd.Parameters.AddWithValue("@ImeArtikla", "");
                                if (d_dimenzije.Text != "")
                                    cmd.Parameters.AddWithValue("@Dimenzije", d_dimenzije.Text);
                                else cmd.Parameters.AddWithValue("@Dimenzije", "");
                                if (d_teza.Text != "")
                                    cmd.Parameters.AddWithValue("@Teza", d_teza.Text);
                                else cmd.Parameters.AddWithValue("@Teza", "");
                                if (d_cena.Text != "")
                                    cmd.Parameters.AddWithValue("@Cena", d_cena.Text);
                                else cmd.Parameters.AddWithValue("@Cena", 0);
                                if (slika_dodaj != null)
                                    cmd.Parameters.AddWithValue("@Slika", slika_dodaj);
                                else cmd.Parameters.AddWithValue("@Slika", null);

                                cmd.ExecuteNonQuery();
                            }

                            connection.Close();
                            MessageBox.Show("Artikel je bil uspešno dodan");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            else if (uredi.IsSelected == true)
            {
                MessageBoxResult r = MessageBox.Show("Ali želite urediti artikel?", "Uredi artikel", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.ConnectionString = connectionString;

                            var queryUpdateArtikel = "update artikli set koda = @Koda, kategorija = @Kategorija, ime_artikla = @ImeArtikla, dimenzije = @Dimenzije, teza = @Teza, cena = @Cena, slika = @Slika where id = " + id_artikla + ";";

                            connection.Open();

                            using (MySqlCommand cmd = new MySqlCommand(queryUpdateArtikel, connection))
                            {
                                if (u_koda.Text != "")
                                    cmd.Parameters.AddWithValue("@Koda", u_koda.Text);
                                else cmd.Parameters.AddWithValue("@Koda", "");
                                if (u_kategorija.SelectedIndex != 0)
                                    cmd.Parameters.AddWithValue("@Kategorija", u_kategorija.SelectedIndex);
                                else cmd.Parameters.AddWithValue("@Kategorija", 0);
                                if (u_ime_artikla.Text != "")
                                    cmd.Parameters.AddWithValue("@ImeArtikla", u_ime_artikla.Text);
                                else cmd.Parameters.AddWithValue("@ImeArtikla", "");
                                if (u_dimenzije.Text != "")
                                    cmd.Parameters.AddWithValue("@Dimenzije", u_dimenzije.Text);
                                else cmd.Parameters.AddWithValue("@Dimenzije", "");
                                if (u_teza.Text != "")
                                    cmd.Parameters.AddWithValue("@Teza", u_teza.Text);
                                else cmd.Parameters.AddWithValue("@Teza", "");
                                if (u_cena.Text != "")
                                    cmd.Parameters.AddWithValue("@Cena", u_cena.Text);
                                else cmd.Parameters.AddWithValue("@Cena", 0);
                                if (slika_uredi != null)
                                    cmd.Parameters.AddWithValue("@Slika", slika_uredi);
                                else cmd.Parameters.AddWithValue("@Slika", null);

                                cmd.ExecuteNonQuery();
                            }

                            connection.Close();
                            MessageBox.Show("Artikel je bil uspešno urejen");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            lv_artikli.DataContext = new ArtikliViewModel(kategorija, tb_isci.Text);
            lv_artikli_slika.DataContext = new ArtikliViewModel(kategorija, tb_isci.Text);
        }

        private void Dodaj_Sliko_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file_dialog = new OpenFileDialog();

            file_dialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|All files (*.*)|*.*";
            file_dialog.Title = "Izberi sliko";

            try
            {
                if (file_dialog.ShowDialog() == true)
                {
                    FileInfo fi = new FileInfo(file_dialog.FileName);

                    if (fi.Length < 10000000)
                    {
                        if (dodaj.IsSelected == true)
                        {
                            slika_dodaj = ReadBytesFromStream(file_dialog.OpenFile());
                            d_slika.Source = new BitmapImage(new Uri(file_dialog.FileName));
                        }
                        else if (uredi.IsSelected == true)
                        {
                            slika_uredi = ReadBytesFromStream(file_dialog.OpenFile());
                            u_slika.Source = new BitmapImage(new Uri(file_dialog.FileName));
                        }
                    }
                    else MessageBox.Show("Prevelika slika");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static byte[] ReadBytesFromStream(Stream stream)
        {
            byte[] size = new byte[16 * 1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int readCount;
                while ((readCount = stream.Read(size, 0, size.Length)) > 0)
                {
                    memoryStream.Write(size, 0, readCount);
                }
                return memoryStream.ToArray();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void lv_artikli_slika_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_artikli.SelectedIndex != -1 || lv_artikli_slika.SelectedIndex != -1)
            {
                if (cb_slike.IsChecked == true)
                {
                    ArtikliModel izbran = lv_artikli_slika.SelectedItem as ArtikliModel;
                    id_artikla = izbran.Id;
                    koda = izbran.Koda;
                    ime_artikla = izbran.ImeArtikla;
                    teza = izbran.Teza;
                    cena = izbran.Cena;

                    if (izbran != null)
                    {
                        u_koda.Text = izbran.Koda;
                        u_ime_artikla.Text = izbran.ImeArtikla;
                        u_kategorija.SelectedIndex = izbran.Kategorija;
                        u_teza.Text = izbran.Teza.ToString();
                        u_dimenzije.Text = izbran.Dimenzije;
                        u_cena.Text = izbran.Cena.ToString();

                        byte[] byteBlob = izbran.Slika as byte[];
                        if (byteBlob != null)
                        {
                            MemoryStream ms = new MemoryStream(byteBlob);
                            BitmapImage bmi = new BitmapImage();
                            bmi.BeginInit();
                            bmi.StreamSource = ms;
                            bmi.EndInit();
                            u_slika.Source = bmi;
                        }
                        else u_slika.Source = null;
                    }
                }
                else if (cb_slike.IsChecked == false)
                {
                    ArtikliModel izbran = lv_artikli.SelectedItem as ArtikliModel;
                    id_artikla = izbran.Id;
                    koda = izbran.Koda;
                    ime_artikla = izbran.ImeArtikla;
                    teza = izbran.Teza;
                    cena = izbran.Cena;

                    if (izbran != null)
                    {
                        u_koda.Text = izbran.Koda;
                        u_ime_artikla.Text = izbran.ImeArtikla;
                        u_kategorija.SelectedIndex = izbran.Kategorija;
                        u_teza.Text = izbran.Teza.ToString();
                        u_dimenzije.Text = izbran.Dimenzije;
                        u_cena.Text = izbran.Cena.ToString();

                        byte[] byteBlob = izbran.Slika as byte[];
                        if (byteBlob != null)
                        {
                            MemoryStream ms = new MemoryStream(byteBlob);
                            BitmapImage bmi = new BitmapImage();
                            bmi.BeginInit();
                            bmi.StreamSource = ms;
                            bmi.EndInit();
                            u_slika.Source = bmi;
                        }
                        else u_slika.Source = null;
                    }
                }
            }
        }

        private void cb_slike_Checked(object sender, RoutedEventArgs e)
        {
            lv_artikli_slika.Visibility = Visibility.Visible;
            lv_artikli.Visibility = Visibility.Hidden;
        }

        private void cb_slike_Unchecked(object sender, RoutedEventArgs e)
        {
            lv_artikli_slika.Visibility = Visibility.Hidden;
            lv_artikli.Visibility = Visibility.Visible;
        }

        private void btn_dodaj_v_racun_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void tb_isci_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(isci_kategorije == false)
            {
                lv_artikli_slika.DataContext = new ArtikliViewModel(0, tb_isci.Text);
                lv_artikli.DataContext = new ArtikliViewModel(0, tb_isci.Text);
            }
            else if (isci_kategorije == true)
            {
                lv_artikli_slika.DataContext = new ArtikliViewModel(cb_kategorije.SelectedIndex, tb_isci.Text);
                lv_artikli.DataContext = new ArtikliViewModel(cb_kategorije.SelectedIndex, tb_isci.Text);
            }
        }

        private void cb_kategorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_kategorije.SelectedIndex == 0)
            {
                lv_artikli_slika.DataContext = new ArtikliViewModel(0, tb_isci.Text);
                lv_artikli.DataContext = new ArtikliViewModel(0, tb_isci.Text);
                isci_kategorije = false;
            }

            if (cb_kategorije.SelectedIndex != 0)
            {
                lv_artikli_slika.DataContext = new ArtikliViewModel(cb_kategorije.SelectedIndex, tb_isci.Text);
                lv_artikli.DataContext = new ArtikliViewModel(cb_kategorije.SelectedIndex, tb_isci.Text);
                isci_kategorije = true;
            }
        }
    }
}