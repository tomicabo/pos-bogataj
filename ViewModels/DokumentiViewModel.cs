using Blagajna_BogatajCementniIzdelki_v2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blagajna_BogatajCementniIzdelki_v2.ViewModels
{
    class DokumentiViewModel
    {
        string sql_string_filter;

        public ObservableCollection<DokumentiModel> dokumenti = new ObservableCollection<DokumentiModel>();
        public ObservableCollection<DokumentiModel> Dokumenti
        {
            get;
            set;
        }

        public DokumentiViewModel(int tip)
        {
            if (tip == 0)
                sql_string_filter = "";
            else if (tip == 1)
                sql_string_filter = "and ustvarjen_racun != 0";
            else if (tip == 2)
                sql_string_filter = "and ustvarjena_dobavnica != 0";
            else if (tip == 3)
                sql_string_filter = "and ustvarjena_ponudba != 0";

            DobiDokumente();
            Dokumenti = dokumenti;
        }

        public ObservableCollection<DokumentiModel> DobiDokumente()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "select d.id, d.ustvarjeno, d.artikli_id, k.kupec1, d.za_placilo, d.st_artiklov, d.ustvarjen_racun, d.ustvarjena_dobavnica, d.ustvarjena_ponudba " +
                            "from dokumenti d join kupci k on d.kupec_id = k.id where izbrisano != 1 " + sql_string_filter + ";";

                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DokumentiModel dokumenti_temp = new DokumentiModel();
                        if (reader["id"] != DBNull.Value)
                            dokumenti_temp.Id = Convert.ToInt32(reader["id"]);
                        if (reader["ustvarjeno"] != DBNull.Value)
                            dokumenti_temp.Ustvarjeno = Convert.ToDateTime(reader["ustvarjeno"]);
                        if (reader["artikli_id"] != DBNull.Value)
                            dokumenti_temp.ArtikliId = Convert.ToInt32(reader["artikli_id"]);
                        if (reader["kupec1"] != DBNull.Value)
                            dokumenti_temp.Kupec = Convert.ToString(reader["kupec1"]);
                        if (reader["za_placilo"] != DBNull.Value)
                            dokumenti_temp.ZaPlacilo = Convert.ToDecimal(reader["za_placilo"]);
                        if (reader["st_artiklov"] != DBNull.Value)
                            dokumenti_temp.StArtiklov = Convert.ToInt32(reader["st_artiklov"]);
                        if (reader["ustvarjen_racun"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(reader["ustvarjen_racun"]) == 0)
                                dokumenti_temp.UstvarjenRacun = "";
                            else
                                dokumenti_temp.UstvarjenRacun = Convert.ToInt32(reader["ustvarjen_racun"]).ToString();
                        }
                        if (reader["ustvarjena_dobavnica"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(reader["ustvarjena_dobavnica"]) == 0)
                                dokumenti_temp.UstvarjenaDobavnica = "";
                            else
                                dokumenti_temp.UstvarjenaDobavnica = Convert.ToInt32(reader["ustvarjena_dobavnica"]).ToString();
                        }
                        if (reader["ustvarjena_ponudba"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(reader["ustvarjena_ponudba"]) == 0)
                                dokumenti_temp.UstvarjenaPonudba = "";
                            else
                                dokumenti_temp.UstvarjenaPonudba = Convert.ToInt32(reader["ustvarjena_ponudba"]).ToString();
                        }

                        dokumenti.Add(dokumenti_temp);
                    }
                }
                connection.Close();
                return dokumenti;
            }
        }
    }
}
