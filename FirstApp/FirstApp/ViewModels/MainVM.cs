using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using SQLite;

using FirstApp.Models;
using FirstApp.ViewModels.Helper;

using Plugin.Permissions.Abstractions;

namespace FirstApp.ViewModels
{
    // เป็นคลาสที่ทำการ notify เมื่อ property มีการเปลี่ยนแปลง
    class MainVM : INotifyPropertyChanged
    {
        // สำหรับปล่อย event
        public event PropertyChangedEventHandler PropertyChanged;

        // รายการ property ต่างๆ ที่จะมีบน view
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title"); // ทำการแจ้งเตือนว่ามี property เปลี่ยนแปลง
                OnPropertyChanged("CanSave"); // เมื่อรับแจ้งเตือนก็จะอัพเคตค่าเองเลย
            }
        }

        private string query;
        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                if (!string.IsNullOrWhiteSpace(query))
                {
                    // get and show list
                    GetVenues();
                    ShowVenues = true;
                }
                else
                {
                    ShowVenues = false;
                }
                OnPropertyChanged("Query");
            }
        }

        private Venue selectedVenue;
        public Venue SelectedVenue
        {
            get { return selectedVenue; }
            set
            {
                selectedVenue = value;
                if (selectedVenue != null)
                {
                    ShowVenues = false;
                    Query = string.Empty;
                }
                OnPropertyChanged("SelectedVenue");
                OnPropertyChanged("ShowSelectedVenue");
            }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
                OnPropertyChanged("CanSave");
            }
        }

        public bool ShowSelectedVenue
        {
            get { return SelectedVenue != null; }
        }

        private bool showVenues;
        public bool ShowVenues
        {
            get { return showVenues; }
            set
            {
                showVenues = value;
                OnPropertyChanged("ShowVenues");
            }
        }

        public bool CanSave
        {
            get { return !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Content); }
        }

        // command for event binding
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        LocationHelper locationHelper;
        public ObservableCollection<Venue> Venues { get; set; }

        public MainVM()
        {
            // init command
            CancelCommand = new Command(CancelAction);
            SaveCommand = new Command<bool>(SaveAction, CanExecuteSave);

            // init helper
            locationHelper = new LocationHelper();

            // init list
            Venues = new ObservableCollection<Venue>();
        }

        public async void GetVenues()
        {
            var response = await Search.SearchRequest(locationHelper.position.Latitude, locationHelper.position.Longitude, 500, Query);

            Venues.Clear();
            foreach(var venue in response.venues)
            {
                Venues.Add(venue);
            }
        }

        public async void GetLocationPermission()
        {
            var status = await PermissionsHelper.GetPermission(Permission.LocationWhenInUse);
            
            if (status == PermissionStatus.Granted)
            {
                await locationHelper.GetLocation(TimeSpan.FromMinutes(30), 500);
            } else
            {
                await App.Current.MainPage.DisplayAlert("Access to location denied", "We don't have access to your location", "Ok");
            }
        }

        public void StopListeningLocationUpdates()
        {
            locationHelper.StopListening();
        }

        void CancelAction(object obj)
        {
            App.Current.MainPage.Navigation.PopAsync();
        }

        void SaveAction(bool obj)
        {
            Experience newExperience = new Experience()
            {
                Title = Title,
                Content = Content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                VenueName = SelectedVenue.name,
                VenueCategory = SelectedVenue.MainCategory,
                VenueLat = float.Parse(SelectedVenue.location.Coordinates.Split(',')[0]),
                VenueLng = float.Parse(SelectedVenue.location.Coordinates.Split(',')[1])
            };

            int insertedItems = 0;
            // added using SQLite;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Experience>();
                insertedItems = conn.Insert(newExperience);
            }
            // here the conn has been disposed of, hence closed
            if (insertedItems > 0)
            {
                Title = string.Empty;
                Content = string.Empty;
                SelectedVenue = null;
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "There was an error inserting the Experience, please try again", "Ok");
            }
        }

        bool CanExecuteSave(bool arg)
        {
            return arg;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
