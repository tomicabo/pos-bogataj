using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    /// Interaction logic for UstvariDokument.xaml
    /// </summary>
    public partial class UstvariDokument : Window
    {
        public decimal? cena = 0;
        public decimal? za_placilo = 0;
        public decimal? popust_artikli = 0;
        public decimal? popust = 0;
        public int skupaj_teza;
        public int st_artiklov;

        int st_dokumenta_artikli;
        int st_kupca;

        bool uredi = false;

        int id_dokumenta_urejanje;

        public class ArtikliLista
        {
            public string Koda { get; set; }
            public string ImeArtikla { get; set; }
            public string ME { get; set; }
            public int Teza { get; set; }
            public int Kolicina { get; set; }
            public decimal Cena { get; set; }
            public decimal Vrednost { get; set; }
            public decimal PopustArt { get; set; }
        }

        ObservableCollection<ArtikliLista> collection_nakup = new ObservableCollection<ArtikliLista>();

        public UstvariDokument(int id)
        {
            InitializeComponent();

            if(id != 0)
            {
                id_dokumenta_urejanje = id;
                NapolniListView();
                OsveziIzracun();
                uredi = true;
            }
            lv_artikli_dokument.ItemsSource = collection_nakup;
        }

        private void lv_artikli_dokument_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_artikli_dokument.SelectedIndex != -1)
            {
                btn_odstrani_artikel.IsEnabled = true;
                btn_spremeni_ceno.IsEnabled = true;
                btn_spremeni_kolicino.IsEnabled = true;
                btn_popust_izbran_artikel.IsEnabled = true;
            }
            else
            {
                btn_odstrani_artikel.IsEnabled = false;
                btn_spremeni_ceno.IsEnabled = false;
                btn_spremeni_kolicino.IsEnabled = false;
                btn_popust_izbran_artikel.IsEnabled = false;
            }
        }

        private void OsveziIzracun()
        {
            decimal? temp_cena = 0, temp_popust = 0; //osveži label - Skupaj, Popust pri artiklih, Popust, Za plačilo
            st_artiklov = 0;
            skupaj_teza = 0;

            foreach (var y in collection_nakup)
            {
                temp_cena = y.Vrednost + temp_cena;
                cena = temp_cena;

                temp_popust = (y.Cena * y.Kolicina) - y.Vrednost + temp_popust;
                popust_artikli = temp_popust;

                za_placilo = temp_cena - (temp_cena * popust / 100);

                skupaj_teza = skupaj_teza + y.Teza;
                st_artiklov++;
            }

            tb_skupaj.Text = String.Format("{0:C2}", cena);     //osveži - Skupaj
            tb_popust_artikli.Text = String.Format("{0:C2}", popust_artikli);      //osveži - Popust pri artiklih
            tb_za_placilo.Text = String.Format("{0:C2}", za_placilo);     //osveži - Za plačilo
            tb_popust_skupaj.Text = String.Format("{0:0.00}", popust) + " %";     //osveži - Popust

            if (collection_nakup.Count < 1)
            {
                tb_skupaj.Text = "0";
                tb_popust_artikli.Text = "0";
                tb_za_placilo.Text = "0";
                tb_popust_skupaj.Text = "0";
            }
        }

        private void btn_dodaj_zac_artikel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_zac_ime_artikla.Text != "" && tb_zac_nakupna_cena.Text != "" && tb_zac_kolicina.Text != "")
                {
                    int teza_temp;
                    if (tb_zac_teza.Text == "")
                        teza_temp = 0;
                    else teza_temp = int.Parse(tb_zac_teza.Text);

                    collection_nakup.Add(new ArtikliLista
                    {
                        Koda = tb_zac_koda.Text,
                        ImeArtikla = tb_zac_ime_artikla.Text,
                        ME = "KOS",
                        Teza = teza_temp,
                        Kolicina = int.Parse(tb_zac_kolicina.Text),
                        Cena = decimal.Parse(tb_zac_nakupna_cena.Text),
                        Vrednost = int.Parse(tb_zac_kolicina.Text) * decimal.Parse(tb_zac_nakupna_cena.Text)
                    });
                }
                else
                    MessageBox.Show("Ime artikla, cena in količina so obvezni podatki");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            OsveziIzracun();
        }       

        private void btn_odstrani_artikel_Click(object sender, RoutedEventArgs e)
        {
            collection_nakup.Remove((ArtikliLista)lv_artikli_dokument.SelectedItem);
            OsveziIzracun();
        }

        private void btn_spremeni_ceno_Click(object sender, RoutedEventArgs e)
        {
            if (tb_spremeni_ceno.Text != "")
            {
                ArtikliLista izbran_artikel = lv_artikli_dokument.SelectedItem as ArtikliLista;

                if (izbran_artikel != null)
                {
                    izbran_artikel.Cena = int.Parse(tb_spremeni_ceno.Text);

                    if (izbran_artikel.PopustArt == 0)
                        izbran_artikel.Vrednost = izbran_artikel.Cena * izbran_artikel.Kolicina;
                    else
                        izbran_artikel.Vrednost = (izbran_artikel.Cena * izbran_artikel.Kolicina) - ((izbran_artikel.Cena * izbran_artikel.Kolicina) * (izbran_artikel.PopustArt / 100));
                }

                lv_artikli_dokument.Items.Refresh();

                OsveziIzracun();
            }
            else MessageBox.Show("Vnesi vrednost");
        }

        private void btn_spremeni_kolicino_Click(object sender, RoutedEventArgs e)
        {
            if (tb_spremeni_kolicino.Text != "")
            {
                ArtikliLista izbran_artikel = lv_artikli_dokument.SelectedItem as ArtikliLista;

                if (izbran_artikel != null)
                {
                    izbran_artikel.Kolicina = int.Parse(tb_spremeni_kolicino.Text);

                    if (izbran_artikel.PopustArt == 0)
                        izbran_artikel.Vrednost = izbran_artikel.Cena * izbran_artikel.Kolicina;
                    else
                        izbran_artikel.Vrednost = (izbran_artikel.Cena * izbran_artikel.Kolicina) - ((izbran_artikel.Cena * izbran_artikel.Kolicina) * (izbran_artikel.PopustArt / 100));
                }

                lv_artikli_dokument.Items.Refresh();

                OsveziIzracun();
            }
            else MessageBox.Show("Vnesi vrednost");
        }

        private void btn_popust_izbran_artikel_Click(object sender, RoutedEventArgs e)
        {
            if (tb_popust_izbran_artikel.Text != "")
            {
                ArtikliLista izbran_artikel = lv_artikli_dokument.SelectedItem as ArtikliLista;

                if (izbran_artikel != null)
                {
                    izbran_artikel.PopustArt = decimal.Parse(tb_popust_izbran_artikel.Text);
                    izbran_artikel.Vrednost = (izbran_artikel.Cena * izbran_artikel.Kolicina) - ((izbran_artikel.Cena * izbran_artikel.Kolicina) * (izbran_artikel.PopustArt / 100));
                }

                lv_artikli_dokument.Items.Refresh();

                OsveziIzracun();
            }
            else MessageBox.Show("Vnesi vrednost");
        }

        private void btn_popust_skupaj_Click(object sender, RoutedEventArgs e)
        {
            if(tb_spremeni_popust_skupaj.Text != "")
            {
                popust = int.Parse(tb_spremeni_popust_skupaj.Text);
                OsveziIzracun();
            }
            else MessageBox.Show("Vnesi vrednost");
        }

        private void btn_svoj_koncni_znesek_Click(object sender, RoutedEventArgs e)
        {
            if (tb_svoj_koncni_znesek.Text != "")
            {
                popust = 100 - ((int.Parse(tb_svoj_koncni_znesek.Text) * 100) / cena);
                popust = Decimal.Round(decimal.Parse(popust.ToString()), 2, MidpointRounding.AwayFromZero);
                za_placilo = int.Parse(tb_svoj_koncni_znesek.Text);

                tb_za_placilo.Text = String.Format("{0:C2}", za_placilo); //osveži - Za plačilo
                tb_popust_skupaj.Text = String.Format("{0:0.00}", popust) + " %";     //osveži - Popust

                MessageBox.Show("Ob dodajanju novih artiklov ali spreminjanju količine se bo znesek za plačilo spremenil, popust bo pa ostal enak.", "Opozorilo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Vnesi vrednost");
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btn_dodaj_artikel_baza_Click(object sender, RoutedEventArgs e)
        {
            var okno = new Artikli(1);
            okno.Owner = this;
            if (okno.ShowDialog() == true)
            {
                try
                {
                    collection_nakup.Add(new ArtikliLista
                    {
                        Koda = okno.koda,
                        ImeArtikla = okno.ime_artikla,
                        ME = "KOS",
                        Teza = okno.teza,
                        Kolicina = 1,
                        Cena = okno.cena,
                        Vrednost = okno.cena
                    });
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
            OsveziIzracun();
        }

        private void btn_shrani_dokument_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ali želite shraniti dokument?", "Shrani Dokument", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                InsertArtikliDokument();
                InsertKupec();
                if (uredi == false)
                    InsertDokumenti();
                else
                    UpdateDokumenti();
                MessageBox.Show("Dokument je bil uspešno shranjen");
                DialogResult = true;
                this.Close();
            }
        }

        private void InsertArtikliDokument()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.ConnectionString = connectionString;
                    var queryGetId = "select max(id)+1 from artikli_dokument;";
                    var queryGetStDokumenta = "select max(st_dokumenta)+1 from artikli_dokument;";

                    var queryInsertDokumentArtikli = "insert into artikli_dokument (id, koda, ime_artikla, me, teza, kolicina, popust, nakupna_cena, vrednost, st_dokumenta)" +
                        "values (@Id, @Koda, @ImeArtikla, @ME, @Teza, @Kolicina, @Popust, @NakupnaCena, @Vrednost, @StDokumenta);";

                    connection.Open();

                    MySqlCommand commandgetid = new MySqlCommand(queryGetId, connection);
                    int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                    MySqlCommand commandgetstdokumenta = new MySqlCommand(queryGetStDokumenta, connection);
                    st_dokumenta_artikli = Convert.ToInt32(commandgetstdokumenta.ExecuteScalar());



                    foreach (var item in lv_artikli_dokument.Items.OfType<ArtikliLista>())
                    {
                        using (MySqlCommand cmd = new MySqlCommand(queryInsertDokumentArtikli, connection))
                        {
                            cmd.Parameters.AddWithValue("@Id", maxid);
                            cmd.Parameters.AddWithValue("@Koda", item.Koda);
                            cmd.Parameters.AddWithValue("@ImeArtikla", item.ImeArtikla);
                            cmd.Parameters.AddWithValue("@ME", item.ME);
                            cmd.Parameters.AddWithValue("@Teza", item.Teza);
                            cmd.Parameters.AddWithValue("@Kolicina", item.Kolicina);
                            cmd.Parameters.AddWithValue("@Popust", item.PopustArt);
                            cmd.Parameters.AddWithValue("@NakupnaCena", item.Cena);
                            cmd.Parameters.AddWithValue("@Vrednost", item.Vrednost);
                            cmd.Parameters.AddWithValue("@StDokumenta", st_dokumenta_artikli);

                            cmd.ExecuteNonQuery();
                        }
                        maxid++;
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InsertKupec()
        {
            if (tb_kupec1.Text == "" &&
                tb_kupec2.Text == "" &&
                tb_kupec3.Text == "" &&
                tb_kupec4.Text == "" &&
                tb_kupec5.Text == "" &&
                tb_kupec6.Text == "")

                st_kupca = 0;
            else
            {
                var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.ConnectionString = connectionString;
                        var queryGetId = "select max(id)+1 from kupci;";

                        var queryInsertKupec = "insert into kupci (id, kupec1, kupec2, kupec3, kupec4, kupec5, kupec6)" +
                            "values (@Id, @Kupec1, @Kupec2, @Kupec3, @Kupec4, @Kupec5, @Kupec6);";

                        connection.Open();

                        MySqlCommand commandgetid = new MySqlCommand(queryGetId, connection);
                        st_kupca = Convert.ToInt32(commandgetid.ExecuteScalar());

                        using (MySqlCommand cmd = new MySqlCommand(queryInsertKupec, connection))
                        {
                            cmd.Parameters.AddWithValue("@Id", st_kupca);
                            cmd.Parameters.AddWithValue("@Kupec1", tb_kupec1.Text);
                            cmd.Parameters.AddWithValue("@Kupec2", tb_kupec2.Text);
                            cmd.Parameters.AddWithValue("@Kupec3", tb_kupec3.Text);
                            cmd.Parameters.AddWithValue("@Kupec4", tb_kupec4.Text);
                            cmd.Parameters.AddWithValue("@Kupec5", tb_kupec5.Text);
                            cmd.Parameters.AddWithValue("@Kupec6", tb_kupec6.Text);

                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void InsertDokumenti()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.ConnectionString = connectionString;
                    var queryGetId = "select max(id)+1 from dokumenti;";

                    var queryInsertDokument = "insert into dokumenti (id, ustvarjeno, artikli_id, kupec_id, skupaj_cena, popust, popust_pri_artiklih, za_placilo, skupaj_teza, st_artiklov, ustvarjen_racun, ustvarjena_dobavnica, ustvarjena_ponudba, izbrisano)" +
                        "values (@Id, @Ustvarjeno, @ArtikliId, @KupecId, @SkupajCena, @Popust, @PopustPriArtiklih, @ZaPlacilo, @SkupajTeza, @StArtiklov, @UstvarjenRacun, @UstvarjenaDobavnica, @UstvarjenaPonudba, @Izbrisano);";

                    connection.Open();

                    MySqlCommand commandgetid = new MySqlCommand(queryGetId, connection);
                    int maxid = Convert.ToInt32(commandgetid.ExecuteScalar());

                    using (MySqlCommand cmd = new MySqlCommand(queryInsertDokument, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", maxid);
                        cmd.Parameters.AddWithValue("@Ustvarjeno", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ArtikliId", st_dokumenta_artikli);
                        cmd.Parameters.AddWithValue("@KupecId", st_kupca);
                        cmd.Parameters.AddWithValue("@SkupajCena", cena);
                        cmd.Parameters.AddWithValue("@Popust", popust);
                        cmd.Parameters.AddWithValue("@PopustPriArtiklih", popust_artikli);
                        cmd.Parameters.AddWithValue("@ZaPlacilo", za_placilo);
                        cmd.Parameters.AddWithValue("@SkupajTeza", skupaj_teza);
                        cmd.Parameters.AddWithValue("@StArtiklov", st_artiklov);
                        cmd.Parameters.AddWithValue("@UstvarjenRacun", 0);
                        cmd.Parameters.AddWithValue("@UstvarjenaDobavnica", 0);
                        cmd.Parameters.AddWithValue("@UstvarjenaPonudba", 0);
                        cmd.Parameters.AddWithValue("@Izbrisano", 0);

                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDokumenti()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.ConnectionString = connectionString;

                    var queryUpdateDokument = "update dokumenti set artikli_id = @ArtikliId, kupec_id = @KupecId, skupaj_cena = @SkupajCena, popust = @Popust, popust_pri_artiklih = @PopustPriArtiklih, za_placilo = @ZaPlacilo, skupaj_teza = @SkupajCena, st_artiklov = @StArtiklov where id = " + id_dokumenta_urejanje + ";";

                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(queryUpdateDokument, connection))
                    {
                        cmd.Parameters.AddWithValue("@ArtikliId", st_dokumenta_artikli);
                        cmd.Parameters.AddWithValue("@KupecId", st_kupca);
                        cmd.Parameters.AddWithValue("@SkupajCena", cena);
                        cmd.Parameters.AddWithValue("@Popust", popust);
                        cmd.Parameters.AddWithValue("@PopustPriArtiklih", popust_artikli);
                        cmd.Parameters.AddWithValue("@ZaPlacilo", za_placilo);
                        cmd.Parameters.AddWithValue("@SkupajTeza", skupaj_teza);
                        cmd.Parameters.AddWithValue("@StArtiklov", st_artiklov);

                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NapolniListView()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "select a.koda, a.ime_artikla, a.me, a.teza, a.kolicina, a.popust, a.nakupna_cena, a.vrednost " +
                            "from artikli_dokument a join dokumenti d on d.artikli_id = a.st_dokumenta where d.id = " + id_dokumenta_urejanje + ";";

                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArtikliLista list_items = new ArtikliLista();

                        if (reader["koda"] != DBNull.Value)
                            list_items.Koda = (Convert.ToString(reader["koda"]));
                        if (reader["ime_artikla"] != DBNull.Value)
                            list_items.ImeArtikla = (Convert.ToString(reader["ime_artikla"]));
                        if (reader["me"] != DBNull.Value)
                            list_items.ME= (Convert.ToString(reader["me"]));
                        if (reader["teza"] != DBNull.Value)
                            list_items.Teza = (Convert.ToInt32(reader["teza"]));
                        if (reader["kolicina"] != DBNull.Value)
                            list_items.Kolicina = (Convert.ToInt32(reader["kolicina"]));
                        if (reader["popust"] != DBNull.Value)
                            list_items.PopustArt = (Convert.ToDecimal(reader["popust"]));
                        if (reader["nakupna_cena"] != DBNull.Value)
                            list_items.Cena = (Convert.ToDecimal(reader["nakupna_cena"]));
                        if (reader["vrednost"] != DBNull.Value)
                            list_items.Vrednost = (Convert.ToDecimal(reader["vrednost"]));

                        collection_nakup.Add(list_items);
                    }
                    connection.Close();
                }
            }
        }
    }
}
