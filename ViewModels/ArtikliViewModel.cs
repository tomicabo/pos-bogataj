using Blagajna_BogatajCementniIzdelki_v2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blagajna_BogatajCementniIzdelki_v2.ViewModels
{
    class ArtikliViewModel
    {
        string sql_string_filter;

        public ObservableCollection<ArtikliModel> artikli = new ObservableCollection<ArtikliModel>();
        public ObservableCollection<ArtikliModel> Artikli
        {
            get;
            set;
        }

        public ArtikliViewModel(int kategorija, string filter)
        {
            if (kategorija == 0)
            {
                if (filter == "")
                {
                    sql_string_filter = "";
                }
                else if (filter != "")
                {
                    sql_string_filter = " where koda like '%" + filter + "%' or ime_artikla like '%" + filter + "%'";
                }
            }
            else if (kategorija != 0)
            {
                if (filter == "")
                {
                    sql_string_filter = " where kategorija = " + kategorija + ";";
                }
                else if (filter != "")
                {
                    sql_string_filter = " where (koda like '%" + filter + "%' or ime_artikla like '%" + filter + "%') and kategorija = " + kategorija + ";";
                }
            }

            DobiArtikle();
            Artikli = artikli;
        }

        public ObservableCollection<ArtikliModel> DobiArtikle()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            var query = "select * from artikli" + sql_string_filter + ";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArtikliModel artikli_temp = new ArtikliModel();
                        if (reader["id"] != DBNull.Value)
                            artikli_temp.Id = Convert.ToInt32(reader["id"]);
                        if (reader["koda"] != DBNull.Value)
                            artikli_temp.Koda= Convert.ToString(reader["koda"]);
                        if (reader["kategorija"] != DBNull.Value)
                            artikli_temp.Kategorija= Convert.ToInt32(reader["kategorija"]);
                        if (reader["ime_artikla"] != DBNull.Value)
                            artikli_temp.ImeArtikla = Convert.ToString(reader["ime_artikla"]);
                        if (reader["dimenzije"] != DBNull.Value)
                            artikli_temp.Dimenzije = Convert.ToString(reader["dimenzije"]);
                        if (reader["teza"] != DBNull.Value)
                            artikli_temp.Teza = Convert.ToInt32(reader["teza"]);
                        if (reader["cena"] != DBNull.Value)
                            artikli_temp.Cena= Convert.ToDecimal(reader["cena"]);
                        if (reader["teza"] != DBNull.Value)
                            artikli_temp.Teza = Convert.ToInt32(reader["teza"]);
                        if (reader["slika"] != DBNull.Value)
                        {
                            byte[] blob = reader["slika"] as byte[];

                            using (MemoryStream ms = new MemoryStream())
                            {
                                ms.Write(blob, 0, blob.Length);
                                Bitmap bm = (Bitmap)Image.FromStream(ms);
                                using (MemoryStream msJpg = new MemoryStream())
                                {
                                    bm.Save(msJpg, ImageFormat.Jpeg);
                                    artikli_temp.Slika = msJpg.GetBuffer();
                                }
                            }
                        }
                        artikli.Add(artikli_temp);
                    }
                }
                connection.Close();
                return artikli;
            }
        }
    }
}
