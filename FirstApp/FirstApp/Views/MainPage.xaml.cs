using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Newtonsoft.Json;
using FirstApp.Models;
using FirstApp.ViewModels;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace FirstApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        // property
        IGeolocator locator = CrossGeolocator.Current;
        Position position;
        MainVM viewModel;

        public MainPage()
        {
            InitializeComponent();

            // use view model for binding
            viewModel = new MainVM();
            BindingContext = viewModel;

            // set event handler when location change
            locator.PositionChanged += Locator_PositionChanged;
        }

        private async void GetLocationPermission()
        {
            // ขอสิทธิจากผู้ใช้
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            if(status != PermissionStatus.Granted)
            {
                if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                {
                    await DisplayAlert("Need your permission", "We need to access your location", "Ok");
                }

                // ขอ permission จริงๆ
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                if (results.ContainsKey(Permission.LocationWhenInUse))
                    status = results[Permission.LocationWhenInUse];
            }

            // Already granted (maybe), go on
            if (status == PermissionStatus.Granted)
            {
                // Granted! Get the location
                GetLocation();
            }
            else
            {
                await DisplayAlert("Access to location denied", "We don't have access to your location", "Ok");
            }
        }

        private async void GetLocation()
        {
            position = await locator.GetPositionAsync(); // use plugin to get location
            if (!locator.IsListening)
            {
                await locator.StartListeningAsync(TimeSpan.FromMinutes(30), 500); // รอการเปลี่ยนค่า
            }
        }

        void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            // on change
            // set new position
            position = e.Position;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetLocationPermission();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            locator.StopListeningAsync(); // stop listening
        }

        async void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.Query))
            {
                string url = $"https://api.foursquare.com/v2/venues/search?ll={position.Latitude},{position.Longitude}&radius=500&query={viewModel.Query}&limit=3&client_id={Helpers.Constants.FOURSQR_CLIENT_ID}&client_secret={Helpers.Constants.FOURSQR_CLIENT_SECRET}&v={DateTime.Now.ToString("yyyyMMdd")}";
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(url);

                    // json to object
                    Search searchResult = JsonConvert.DeserializeObject<Search>(json);
                    viewModel.ShowVenues = true;
                    venuesListView.ItemsSource = searchResult.response.venues;
                }
            } else
            {
                viewModel.ShowVenues = false;
            }
        }
    }
}
