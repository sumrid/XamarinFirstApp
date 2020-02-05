using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace FirstApp.ViewModels.Helper
{
    class LocationHelper
    {
        public IGeolocator locator = CrossGeolocator.Current; // ตัวหาตำแหน่ง
        public Position position; // เก็บตำแหน่ง

        public LocationHelper()
        {
            // Set event handler when location change
            locator.PositionChanged += Locator_PositionChanged;
        }

        void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            position = e.Position;
        }

        public async Task<Position> GetLocation(TimeSpan minimumTime, int minimumMeters)
        {
            position = await locator.GetPositionAsync(); // get current position
            if (!locator.IsListening)
            {
                await locator.StartListeningAsync(minimumTime, minimumMeters);
            }
            return position;
        }

        public async void StopListening()
        {
            await locator.StopListeningAsync();
        }
    }
}
