using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Blagajna_BogatajCementniIzdelki_v2
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is byte[])
            {
                byte[] byteBlob = value as byte[];
                //byte[] byteBlob = new byte[16 * 1024];
                MemoryStream ms = new MemoryStream(byteBlob);
                BitmapImage bmi = new BitmapImage();

                bmi.BeginInit();
                bmi.StreamSource = ms;
                bmi.EndInit();
                //bmi.Source(ms);
                //bmi.UriSource = new Uri(ms.ToString());

                return bmi;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
