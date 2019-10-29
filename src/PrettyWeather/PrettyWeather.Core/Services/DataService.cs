using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrettyWeather.Model;
using Xamarin.Essentials;

namespace PrettyWeather.Services
{
    public class DataService
    {
        static HttpClient httpClient = new HttpClient();

        readonly string functionsUrl = "https://prettyweather.azurewebsites.net/api/GetCurrentConditions";

        public async Task<WeatherInfo> GetWeatherInfo(double latitude, double longitude)
        {
            try
            {
                var infoJson = await httpClient.GetStringAsync($"{functionsUrl}?lat={latitude}&long={longitude}");

                var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(infoJson);

                return weatherInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return null;
        }

        public IEnumerable<CityInfo> GetSavedCities()
        {            
            var cityJson = Preferences.Get("savedcities", string.Empty);

            if (!string.IsNullOrWhiteSpace(cityJson))
                return JsonConvert.DeserializeObject<List<CityInfo>>(cityJson);

            return new List<CityInfo>();            
        }

        public void SaveCity(CityInfo city)
        {
            try
            {
                List<CityInfo> cityInfos = new List<CityInfo>();

                var cityJson = Preferences.Get("savedcities", string.Empty);

                if (!string.IsNullOrWhiteSpace(cityJson))
                    cityInfos.AddRange(JsonConvert.DeserializeObject<List<CityInfo>>(cityJson));

                cityInfos.Add(city);

                Preferences.Set("savedcities", JsonConvert.SerializeObject(cityInfos));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
        }

    }
}