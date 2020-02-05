using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FirstApp.Models;
using FirstApp.ViewModels;

namespace FirstApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExperiencePage : ContentPage
    {
        ExperiencesVM viewModel;
        public ExperiencePage()
        {
            InitializeComponent();

            // binding view model
            viewModel = new ExperiencesVM();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.ReadExperiences();
        }
    }
}