using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FirstApp.ViewModels.Commands
{
    class NewExperienceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ExperiencesVM viewModel;

        public NewExperienceCommand(ExperiencesVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // logic ที่บอกว่าเมื่อมี event จะให้ทำอะไร
            viewModel.NewExperience();
        }
    }
}
