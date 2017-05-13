using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Other;
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
			ValidateFieldsCommand = new Command(OnValidateFieldsCommandExecute);
			TryEnterCommand = new Command(OnTryEnterCommandExecute);//, OnTryEnterCommandCanExecute);
        }

        public string Login
        {
            get { return GetValue<string>(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string));

		public string LoginErrorInfo
		{
			get { return GetValue<string>(LoginErrorInfoProperty); }
			set { SetValue(LoginErrorInfoProperty, value); }
		}
		public static readonly PropertyData LoginErrorInfoProperty = RegisterProperty("LoginErrorInfo", typeof(string), string.Empty);

        public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string));


		public bool IsEnabledEnterButton
		{
			get { return GetValue< bool>(IsEnabledEnterButtonProperty); }
			set { SetValue(IsEnabledEnterButtonProperty, value); }
		}
		public static readonly PropertyData IsEnabledEnterButtonProperty = RegisterProperty("IsEnabledEnterButton", typeof( bool), false);



        public Command BackCommand { get; private set; }

        private void OnBackCommandExecute()
        {
            _parentViewModel.ChangePage(_previousViewModel);
        }

        public Command TryEnterCommand { get; private set; }


		public Command ValidateFieldsCommand { get; private set; }

		private void OnValidateFieldsCommandExecute()
		{
			// TODO: сделать нормальную валидацию
			IsEnabledEnterButton = User.IsValidLogin(Login);
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
