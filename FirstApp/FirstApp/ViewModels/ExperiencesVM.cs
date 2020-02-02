using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using FirstApp.ViewModels.Commands;

namespace FirstApp.ViewModels
{
    class ExperiencesVM : INotifyPropertyChanged
    {
        // ตัวแปรสำหรับส่ง event
        public event PropertyChangedEventHandler PropertyChanged;
        
        // command สำหรับผูกกับ xaml
        public NewExperienceCommand NewExperienceCommand { get; set; }

        public ExperiencesVM()
        {
            NewExperienceCommand = new NewExperienceCommand(this);
        }

        // command action
        public void NewExperience()
        {
            App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
