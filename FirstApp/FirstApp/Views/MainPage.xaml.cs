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
using FirstApp.ViewModels.Helper;

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
        Position position;
        MainVM viewModel;

        public MainPage()
        {
            InitializeComponent();

            // use view model for binding
            viewModel = new MainVM();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.GetLocationPermission();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.StopListeningLocationUpdates();
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
