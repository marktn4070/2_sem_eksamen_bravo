using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _2_sem_eksamen_bravo.ViewModels;

namespace _2_sem_eksamen_bravo.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Customer")
            {
                viewModel.SelectedViewModel = new CustomerViewModel();
            }
            else if (parameter.ToString() == "Messages")
            {
                viewModel.SelectedViewModel = new MessagesViewModel();
            }
            else if (parameter.ToString() == "SendMessage")
            {
                viewModel.SelectedViewModel = new SendMessageViewModel();
            }
            else if (parameter.ToString() == "Search_test")
            {
                viewModel.SelectedViewModel = new Search_testModel();
            }
        }
    }
}
