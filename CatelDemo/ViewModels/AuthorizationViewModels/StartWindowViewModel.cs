using System.Threading.Tasks;
using Catel.MVVM;

namespace CatelDemo.ViewModels.AuthorizationViewModels
{
    public class StartWindowViewModel : ViewModelBase
    {
        private readonly IViewModel _parentViewModel;

        public StartWindowViewModel(IViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;

            EnterCommand = new Command(OnEnterCommandExecute);
            RegistrationCommand = new Command(OnRegistrationCommandExecute);
            ExitCommand = new Command(OnExitCommandExecute);
        }

        public Command EnterCommand { get; private set; }
        private void OnEnterCommandExecute()
        {
            _parentViewModel.ChangePage(new EnterViewModel(_parentViewModel, this));
        }
        public Command RegistrationCommand { get; private set; }

        private void OnRegistrationCommandExecute()
        {
            _parentViewModel.ChangePage(new RegistrationViewModel(_parentViewModel, this));
        }

        public Command ExitCommand { get; private set; }
        private async void OnExitCommandExecute()
        {
            await _parentViewModel.CloseViewModelAsync(true);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
