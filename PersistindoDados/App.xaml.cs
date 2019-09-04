using System;
using MonkeyCache.SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PersistindoDados
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = "PokemonCache";

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
