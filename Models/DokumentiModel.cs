using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blagajna_BogatajCementniIzdelki_v2.Models
{
    class DokumentiModel : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _ustvarjeno;
        private int _artikli_id;
        private string _kupec;
        //private decimal _skupaj_cena;
        //popust?
        private decimal _za_placilo;
        private int _st_artiklov;
        private string _ustvarjen_racun;
        private string _ustvarjena_dobavnica;
        private string _ustvarjena_ponudba;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        public DateTime Ustvarjeno
        {
            get { return _ustvarjeno; }
            set
            {
                if (value != _ustvarjeno)
                {
                    _ustvarjeno = value;
                    NotifyPropertyChanged("Ustvarjeno");
                }
            }
        }

        public int ArtikliId
        {
            get { return _artikli_id; }
            set
            {
                if (value != _artikli_id)
                {
                    _artikli_id = value;
                    NotifyPropertyChanged("ArtikliId");
                }
            }
        }

        public string Kupec
        {
            get { return _kupec; }
            set
            {
                if (value != _kupec)
                {
                    _kupec = value;
                    NotifyPropertyChanged("KupecId");
                }
            }
        }

        public decimal ZaPlacilo
        {
            get { return _za_placilo; }
            set
            {
                if (value != _za_placilo)
                {
                    _za_placilo = value;
                    NotifyPropertyChanged("ZaPlacilo");
                }
            }
        }

        public int StArtiklov
        {
            get { return _st_artiklov; }
            set
            {
                if (value != _st_artiklov)
                {
                    _st_artiklov = value;
                    NotifyPropertyChanged("StArtiklov");
                }
            }
        }

        public string UstvarjenRacun
        {
            get { return _ustvarjen_racun; }
            set
            {
                if (value != _ustvarjen_racun)
                {
                    _ustvarjen_racun = value;
                    NotifyPropertyChanged("UstvarjenRacun");
                }
            }
        }

        public string UstvarjenaDobavnica
        {
            get { return _ustvarjena_dobavnica; }
            set
            {
                if (value != _ustvarjena_dobavnica)
                {
                    _ustvarjena_dobavnica = value;
                    NotifyPropertyChanged("UstvarjenaDobavnica");
                }
            }
        }

        public string UstvarjenaPonudba
        {
            get { return _ustvarjena_ponudba; }
            set
            {
                if (value != _ustvarjena_ponudba)
                {
                    _ustvarjena_ponudba= value;
                    NotifyPropertyChanged("UstvarjenaPonudba");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
