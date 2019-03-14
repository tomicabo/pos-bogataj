using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blagajna_BogatajCementniIzdelki_v2.Models
{
    class ArtikliModel : INotifyPropertyChanged
    {
        private int _id;
        private string _koda;
        private int _kategorija;
        private string _ime_artikla;
        private string _dimenzije;
        private int _teza;
        private decimal _cena;
        private byte[] _slika;

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

        public string Koda
        {
            get { return _koda; }
            set
            {
                if (value != _koda)
                {
                    _koda = value;
                    NotifyPropertyChanged("Koda");
                }
            }
        }

        public int Kategorija
        {
            get { return _kategorija; }
            set
            {
                if (value != _kategorija)
                {
                    _kategorija = value;
                    NotifyPropertyChanged("Kategorija");
                }
            }
        }

        public string ImeArtikla
        {
            get { return _ime_artikla; }
            set
            {
                if (value != _ime_artikla)
                {
                    _ime_artikla = value;
                    NotifyPropertyChanged("ImeArtikla");
                }
            }
        }

        public string Dimenzije
        {
            get { return _dimenzije; }
            set
            {
                if (value != _dimenzije)
                {
                    _dimenzije = value;
                    NotifyPropertyChanged("Dimenzije");
                }
            }
        }

        public int Teza
        {
            get { return _teza; }
            set
            {
                if (value != _teza)
                {
                    _teza = value;
                    NotifyPropertyChanged("Teza");
                }
            }
        }

        public decimal Cena
        {
            get { return _cena; }
            set
            {
                if (value != _cena)
                {
                    _cena = value;
                    NotifyPropertyChanged("Cena");
                }
            }
        }

        public byte[] Slika
        {
            get { return _slika; }
            set
            {
                if (value != _slika)
                {
                    _slika = value;
                    NotifyPropertyChanged("Slika");
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
