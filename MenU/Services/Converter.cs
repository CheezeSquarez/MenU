using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;


namespace MenU.Services
{
    class Converter
    {
    }
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            byte[] byteArray;
            try
            {
                byteArray = Client.DownloadData(value.ToString());
            }
            catch (System.Net.WebException e)
            {
                return null;
            }

            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
