using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;
using RestaurantHelper.ViewModels.ClientViewModels;
using RestaurantHelper.ViewModels.ManagerViewModels;
using Xceed.Wpf.Toolkit;

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

        public Command BackCommand { get; private set; }

        private void OnBackCommandExecute()
        {
            _parentViewModel.ChangePage(_previousViewModel);
        }

        public Command TryEnterCommand { get; private set; }

	    private bool OnTryEnterCommandCanExecute()
	    {
		    return Password != null && Password.Length > 1;
	    }
		private void OnTryEnterCommandExecute()
        {
            _authorizationChecker = new AuthorizationChecker(new User {Login = Login, Password = Password});

	        if (_authorizationChecker.IsAdmin())
	        {
		        _parentViewModel.ChangePage(new ManagerMainViewModel());
	        }
            else if (_authorizationChecker.IsMatchUser())
            {
                User user = _authorizationChecker.GetUser();
                //TODO: надо бы сделать какую нибудь заставку для успешной авторизации
                _parentViewModel.ChangePage(new ClientMainViewModel(user));
            }
            else if (_authorizationChecker.IsExistsLogin())
            {
                MessageBox.Show("Неверный пароль!");
            }
            else
            {
				MessageBox.Show("Пользователь не зарегистрирован");
            }
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
