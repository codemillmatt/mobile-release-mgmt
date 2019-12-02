using PrettyWeather.Pages;
using Xamarin.Forms;

namespace PrettyWeather
{
    public partial class App : Application
    {        
        public App()
        {
            InitializeComponent();

            MainPage = new AppShellPage();
        }

        protected override void OnStart()
        {            
            Routing.RegisterRoute("saved-cities", typeof(SavedCitiesPage));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
