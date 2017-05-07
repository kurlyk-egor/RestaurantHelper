using System.Threading.Tasks;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Other;
using RestaurantHelper.ViewModels.ClientViewModels;

namespace RestaurantHelper.ViewModels.AuthorizationViewModels
{
    public class EnterViewModel : ViewModelBase
    {
        private readonly IViewModel _parentViewModel;
        private readonly IViewModel _previousViewModel;
        private AuthorizationChecker _authorizationChecker;

        public EnterViewModel(IViewModel parentViewModel, IViewModel previousViewModel)
        {
            _parentViewModel = parentViewModel;
            _previousViewModel = previousViewModel;

            BackCommand = new Command(OnBackCommandExecute);
            TryEnterCommand = new Command(OnTryEnterCommandExecute, OnTryEnterCommandCanExecute);
        }

        // TODO: Register models with the vmpropmodel codesnippet


        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        public string Login
        {
            get { return GetValue<string>(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string));

        public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string));

        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        public Command BackCommand { get; private set; }

        private void OnBackCommandExecute()
        {
            _parentViewModel.ChangePage(_previousViewModel);
        }

        public Command TryEnterCommand { get; private set; }

        private bool OnTryEnterCommandCanExecute()
        {
            return !(string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password));
        }

        private void OnTryEnterCommandExecute()
        {
            // TODO: Handle command logic here
            var locator = this.GetServiceLocator();
            var messageService = locator.ResolveType<IMessageService>();

            _authorizationChecker = new AuthorizationChecker(new User {Login = Login, Password = Password});
            

            if (_authorizationChecker.IsMatchUser())
            {
                User user = _authorizationChecker.GetUser();
                messageService.ShowAsync("Успешная авторизация!");
                _parentViewModel.ChangePage(new ClientMainViewModel(user));
            }
            else if (_authorizationChecker.IsExistsLogin())
            {
                messageService.ShowAsync("Неверный пароль!");
            }
            else
            {
                messageService.ShowAsync("Пользователь не зарегистрирован");
            }
        }
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
