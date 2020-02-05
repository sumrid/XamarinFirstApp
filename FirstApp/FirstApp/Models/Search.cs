﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using FirstApp.Helpers;

namespace FirstApp.Models
{
    public class Meta
    {
        public int code { get; set; }
        public string requestId { get; set; }
    }

    public class LabeledLatLng
    {
        public string label { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public string crossStreet { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public IList<LabeledLatLng> labeledLatLngs { get; set; }
        public int distance { get; set; }
        public string postalCode { get; set; }
        public string cc { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public IList<string> formattedAddress { get; set; }

        private string coordinates;
        public string Coordinates
        {
            get { return $"{lat:0.000}, {lng:0.000}"; }
        }
    }

    public class Icon
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public string shortName { get; set; }
        public Icon icon { get; set; }
        public bool primary { get; set; }
    }

    public class VenuePage
    {
        public string id { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public IList<Category> categories { get; set; }
        public VenuePage venuePage { get; set; }

        // add to the Venue class
        private string mainCategory;
        public string MainCategory
        {
            // added using System.Linq;
            get { return categories.FirstOrDefault()?.name; }
        }
    }

    public class Response
    {
        public IList<Venue> venues { get; set; }
    }

    public class Example
    {
        public Meta meta { get; set; }
        public Response response { get; set; }
    }

    public class Search
    {
        public Meta meta { get; set; }
        public Response response { get; set; }

        public static async Task<Response> SearchRequest(double lat, double lng, int radius, string query, int limit = 3)
        {
            string url = $"https://api.foursquare.com/v2/venues/search?ll={lat},{lng}&radius={radius}&query={query}&limit={limit}&client_id={Helpers.Constants.FOURSQR_CLIENT_ID}&client_secret={Helpers.Constants.FOURSQR_CLIENT_SECRET}&v={DateTime.Now.ToString("yyyyMMdd")}";

            using (HttpClient client = new HttpClient())
            {
                // made the method async
                string json = await client.GetStringAsync(url);

                return JsonConvert.DeserializeObject<Search>(json).response;
            }
        }
    }

}
