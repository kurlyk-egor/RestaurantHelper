using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.ViewModels;
using RestaurantHelper.Models;
using RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
    public class ClientMainViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly IViewModel _parentViewModel;
        public ClientMainViewModel(User user)
        {
            OrderCommand = new Command(OnOrderCommandExecute);
            MyOrdersCommand = new Command(OnMyOrdersCommandExecute);
            ProfileCommand = new Command(OnProfileCommandExecute);
            ReviewsCommand = new Command(OnReviewsCommandExecute);
            ExitCommand = new Command(OnExitCommandExecute);

            _user = user;
            AuthorizedString = $"Авторизован: {_user.Login}";
            _parentViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
        }

        public string AuthorizedString
        {
            get { return GetValue<string>(AuthorizedStringProperty); }
            set { SetValue(AuthorizedStringProperty, value); }
        }
        public static readonly PropertyData AuthorizedStringProperty = RegisterProperty("AuthorizedString", typeof(string));


        public Command OrderCommand { get; private set; }
        public Command MyOrdersCommand { get; private set; }
        public Command ProfileCommand { get; private set; }
        public Command ReviewsCommand { get; private set; }
        public Command ExitCommand { get; private set; }


        private void OnOrderCommandExecute()
        {
            _parentViewModel.ChangePage(new HallViewModel(_user));
        }

        private void OnMyOrdersCommandExecute()
        {
            _parentViewModel.ChangePage(new MyOrdersViewModel(this, _user));
        }

        private void OnProfileCommandExecute()
        {
            _parentViewModel.ChangePage(new ProfileViewModel(this, _user));
        }

        private void OnReviewsCommandExecute()
        {
            _parentViewModel.ChangePage(new ClientReviewsViewModel(this, _user));
        }

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
