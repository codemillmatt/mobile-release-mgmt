using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrettyWeather.Converters
{
    public class CurrentConditionsIconConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // these hex codes coorespond to font aweosome solid codes
            if (value is string condition)
            {
                if (condition == "clear-day")
                    return ((char)0xf185).ToString();
                //return ((char)0xf00d).ToString();

                if (condition == "clear-night")
                    return ((char)0xf186).ToString();
                //return ((char)0xf02e).ToString();

                if (condition == "rain")
                    return ((char)0xf740).ToString();
                //return ((char)0xf019).ToString();

                if (condition == "snow")
                    return ((char)0xf73b).ToString();
                //return ((char)0xf01b).ToString();

                if (condition == "sleet")
                    return ((char)0xf7ae).ToString();
                //return ((char)0xf0b5).ToString();

                if (condition == "wind")
                    return ((char)0xf72e).ToString();
                //return ((char)0xf050).ToString();

                if (condition == "fog")
                    return ((char)0xf75f).ToString();
                //return ((char)0xf014).ToString();

                if (condition == "cloudy")
                    return ((char)0xf0c2).ToString();
                //return ((char)0xf013).ToString();

                if (condition == "partly-cloudy-day")
                    return ((char)0xf6c4).ToString();
                //return ((char)0xf002).ToString();

                if (condition == "partly-cloudy-night")
                    return ((char)0xf6c3).ToString();
                //return ((char)0xf086).ToString();


            }

            return ((char)0xf0e7).ToString();
            //return ((char)0xf07b).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
