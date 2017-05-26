using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;

namespace RestaurantHelper.ViewModels.AuthorizationViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly IViewModel _parentViewModel;
        private readonly IViewModel _previousViewModel;

	    public RegistrationViewModel(IViewModel parentViewModel, IViewModel previousViewModel, User user = null)
        {
            _parentViewModel = parentViewModel;
            _previousViewModel = previousViewModel;

		    BackCommand = new Command(OnBackCommandExecute);
            RegistrationCommand = new Command(OnRegistrationCommandExecute, OnRegistrationCommandCanExecute);

            if (user == null)
            {
                user = new User();
            }
            User = user;
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

        [ViewModelToModel("User")]
        public string Password
        {
            get { return GetValue<string>(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly PropertyData PasswordProperty = RegisterProperty("Password", typeof(string));

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


        public Command BackCommand { get; private set; }
        private void OnBackCommandExecute()
        {
            _parentViewModel.ChangePage(_previousViewModel);
        }


        public Command RegistrationCommand { get; private set; }

	    private bool OnRegistrationCommandCanExecute()
	    {
		    return Password != null && Password.Length > 1;
	    }
		private void OnRegistrationCommandExecute()
        {
            _unitOfWork.Users.Insert(User);
            _unitOfWork.SaveChanges();

			_parentViewModel.ChangePageWithDialog(new ShortMessageViewModel("Успешная регистрация!"), 1000, _previousViewModel);
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
