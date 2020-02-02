using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FirstApp.Models;

namespace FirstApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExperiencePage : ContentPage
    {
        public ExperiencePage()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            // เป็นการสร้าง page ใหม่เข้า stack
            Navigation.PushAsync(new MainPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ReadExp();
        }

        private void ReadExp()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                // get data from table
                conn.CreateTable<Experience>();
                List<Experience> exps = conn.Table<Experience>().ToList();

                // set data to listView
                expListView.ItemsSource = exps;
            }
        }
    }
}