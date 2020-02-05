using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using FirstApp.Models;
using FirstApp.ViewModels.Commands;

namespace FirstApp.ViewModels
{
    class ExperiencesVM : INotifyPropertyChanged
    {
        // ตัวแปรสำหรับส่ง event
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Experience> Experiences { get; set; }

        public ExperiencesVM()
        {
            NewExperienceCommand = new NewExperienceCommand(this); // init command
            Experiences = new ObservableCollection<Experience>(); // init list
            ReadExperiences();
        }

        // command สำหรับผูกกับ xaml
        public NewExperienceCommand NewExperienceCommand { get; set; }

        // command action
        public void NewExperience()
        {
            App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }

        public void ReadExperiences()
        {
            // local variable
            var experiences = Experience.GetExperiences();

            Experiences.Clear();
            foreach(var exp in experiences)
            {
                Experiences.Add(exp);
            }
        }
    }
}
