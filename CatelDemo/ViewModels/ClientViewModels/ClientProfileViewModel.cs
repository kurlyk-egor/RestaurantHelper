using System.Threading.Tasks;
using System.Windows;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.ViewModels;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
    public class ClientProfileViewModel : ViewModelBase
    {
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
	    private AuthorizationChecker _authorizationChecker;
		private readonly IViewModel _rootViewModel;
		private readonly IViewModel _parentViewModel;
        public ClientProfileViewModel(IViewModel previousViewModel, User user)
        {
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			_parentViewModel = previousViewModel;
			_authorizationChecker = new AuthorizationChecker(user);
            User = user;

            SaveCommand = new Command(OnSaveCommandExecute, OnSaveCommandCanExecute);
            BackCommand = new Command(OnBackCommandExecute);
        }

        [Model]
        public User User
        {
            get { return GetValue<User>(UserProperty); }
            private set { SetValue(UserProperty, value); }
        }
        public static readonly PropertyData UserProperty = RegisterProperty("User", typeof(User));


        [ViewModelToModel("User")]
        public string Login
        {
            get { return GetValue<string>(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly PropertyData LoginProperty = RegisterProperty("Login", typeof(string));


        public string OldPassword
        {
            get { return GetValue<string>(OldPasswordProperty); }
            set { SetValue(OldPasswordProperty, value); }
        }
        public static readonly PropertyData OldPasswordProperty = RegisterProperty("OldPassword", typeof(string));

		public string NewPassword
		{
			get { return GetValue<string>(NewPasswordProperty); }
			set { SetValue(NewPasswordProperty, value); }
		}
		public static readonly PropertyData NewPasswordProperty = RegisterProperty("NewPassword", typeof(string));


		[ViewModelToModel("User")]
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));


        [ViewModelToModel("User")]
        public string Phone
        {
            get { return GetValue<string>(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }
        public static readonly PropertyData PhoneProperty = RegisterProperty("Phone", typeof(string));


	    public Command SaveCommand { get; private set; }

	    private bool OnSaveCommandCanExecute()
	    {
		    return OldPassword != null && OldPassword.Length > 2 && 
					NewPassword != null && NewPassword.Length > 2;
	    }
		private void OnSaveCommandExecute()
		{
			// хэш-код введенного пароля совпал с сохраненным
			if (_authorizationChecker.GetHashPassword(OldPassword) == User.Password)
			{
				User.Password = _authorizationChecker.GetHashPassword(NewPassword);
				_unitOfWork.Users.Update(User);
				_unitOfWork.SaveChanges();

				_rootViewModel.ChangePageWithDialog(new ShortMessageViewModel("Сохранено!"), 1000, _parentViewModel);
			}
			else
			{
				_rootViewModel.ChangePageWithDialog(new ShortMessageViewModel("Неверно указан существующий пароль!"), 1222);
			}
        }

        public Command BackCommand { get; private set; }
        private void OnBackCommandExecute()
        {
            _rootViewModel.ChangePage(_parentViewModel);
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
