using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstApp.Views;

namespace FirstApp
{
    public partial class App : Application
    {
        public static string DatabasePath;

        public App(string databasePath)
        {
            InitializeComponent();

            // รับพาทมาจาก iOS and android
            DatabasePath = databasePath;

            // กำหนดหน้าแรกของ app
            // MainPage = new MainPage();
            MainPage = new NavigationPage(new ExperiencePage());
        }

        public App()
        {
            InitializeComponent();

            // กำหนดหน้าแรกของ app
            // MainPage = new MainPage();
            MainPage = new NavigationPage(new ExperiencePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
