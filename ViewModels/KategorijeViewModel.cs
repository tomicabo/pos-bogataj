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
    class KategorijeViewModel
    {
        public ObservableCollection<KategorijeModel> kategorije = new ObservableCollection<KategorijeModel>();
        public ObservableCollection<KategorijeModel> Kategorije
        {
            get;
            set;
        }

        public KategorijeViewModel()
        {
            DobiKategorije();
            Kategorije = kategorije;
        }

        public ObservableCollection<KategorijeModel> DobiKategorije()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "select * from kategorije_artikla";

                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        KategorijeModel kategorije_temp = new KategorijeModel();
                        if (reader["id"] != DBNull.Value)
                            kategorije_temp.Id = Convert.ToInt32(reader["id"]);
                        if (reader["ime_kategorije"] != DBNull.Value)
                            kategorije_temp.ImeKategorije = Convert.ToString(reader["ime_kategorije"]);
                        
                        kategorije.Add(kategorije_temp);
                    }
                }
                connection.Close();
                return kategorije;
            }
        }
    }
}
