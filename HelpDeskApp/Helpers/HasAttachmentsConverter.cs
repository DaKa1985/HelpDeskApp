using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Helpers
{
    public class HasAttachmentsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> attachments)
                return attachments.Count > 0 ? "✅ Ja" : "❌ Nein";

            return "❌ Nein";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
