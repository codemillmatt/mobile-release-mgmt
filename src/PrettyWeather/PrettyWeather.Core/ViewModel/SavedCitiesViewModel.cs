using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PrettyWeather.Messaging;
using PrettyWeather.Model;
using PrettyWeather.Pages;
using Xamarin.Forms;
using PrettyWeather.Services;

namespace PrettyWeather.ViewModel
{
    public class SavedCitiesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool initialized = false;

        public ObservableCollection<CityInfo> SavedCities { get; set; }

        DataService dataSvc;

        public SavedCitiesViewModel()
        {
            dataSvc = new DataService();

            AddCityCommand = new Command(async () => await ExecuteAddCityCommand());
            SelectCityCommand = new Command(async () => await ExecuteSelectCityCommand());

            SavedCities = new ObservableCollection<CityInfo>();

            MessagingCenter.Subscribe<SearchCitySelectedMessage>(this, SearchCitySelectedMessage.Message,
                (msg) =>
                {
                    SavedCities.Add(msg.SelectedCity);

                    // Add the city to Cosmos
                    dataSvc.SaveCity(msg.SelectedCity);
                });
        }

        public void GetSavedCities()
        {
            if (!initialized)
            {
                var savedCities = dataSvc.GetSavedCities();

                foreach (var city in savedCities)
                {
                    SavedCities.Add(city);
                }
                
                initialized = true;
            }
        }

        private async Task ExecuteAddCityCommand()
        {
            await Shell.Current.Navigation.PushModalAsync(new AddCitySearchPage());
        }

        private async Task ExecuteSelectCityCommand()
        {
            var msg = new DisplayCitySelectedMessage { SelectedCity = SelectedCity };

            MessagingCenter.Send(msg, DisplayCitySelectedMessage.Message);

            await Shell.Current.Navigation.PopModalAsync();
        }

        public ICommand AddCityCommand { get; }
        public ICommand SelectCityCommand { get; }

        CityInfo selectedCity;
        public CityInfo SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCity)));
            }
        }

    }
}
