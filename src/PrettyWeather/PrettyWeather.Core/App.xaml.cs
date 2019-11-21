//using Microsoft.AppCenter;
//using Microsoft.AppCenter.Analytics;
//using Microsoft.AppCenter.Crashes;
using PrettyWeather.Pages;
using Xamarin.Forms;

namespace PrettyWeather
{
    public partial class App : Application
    {
        readonly string iOSAppCenter = "1f555f3f-2d28-4ea6-8242-a017ca8bc1c8";
        readonly string androidAppCenter = "47db8bf5-4f22-4f2a-ad39-806eee20dfbc";

        public App()
        {
            InitializeComponent();

            MainPage = new AppShellPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //AppCenter.Start($"ios={iOSAppCenter};android={androidAppCenter};",
            //    typeof(Analytics), typeof(Crashes));

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
