using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blagajna_BogatajCementniIzdelki_v2.Models
{
    class KategorijeModel : INotifyPropertyChanged
    {
        private int _id;
        private string _ime_kategorije;

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

        public string ImeKategorije
        {
            get { return _ime_kategorije; }
            set
            {
                if (value != _ime_kategorije)
                {
                    _ime_kategorije = value;
                    NotifyPropertyChanged("ImeKategorije");
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
