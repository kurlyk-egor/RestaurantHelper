using System.Threading;
using System.Threading.Tasks;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.ViewModels.AuthorizationViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IViewModel _parentViewModel;
        private readonly IViewModel _previousViewModel;


        public RegistrationViewModel(IViewModel parentViewModel, IViewModel previousViewModel, User user = null)
        {
            _parentViewModel = parentViewModel;
            _previousViewModel = previousViewModel;
            _userRepository = new Repository<User>();

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


        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
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


        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        public Command BackCommand { get; private set; }
        private void OnBackCommandExecute()
        {
            // TODO: Handle command logic here
            _parentViewModel.ChangePage(_previousViewModel);
        }


        public Command RegistrationCommand { get; private set; }
        private bool OnRegistrationCommandCanExecute()
        {
            bool result = true;
            long number;

            if (string.IsNullOrWhiteSpace(Phone) || !long.TryParse(Phone, out number))
            {
                result = false;
            }
            if (string.IsNullOrWhiteSpace(Login))
            {
                result = false;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                result = false;
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                result = false;
            }
            return result;
        }
        private async void OnRegistrationCommandExecute()
        {
            _userRepository.Insert(User);
            _userRepository.SaveChanges();
            
            var resolver = this.GetDependencyResolver();
            var visualizer = resolver.Resolve<IUIVisualizerService>();
            var successRegistration = new SuccessRegistrationViewModel();
            visualizer.Show(successRegistration);

            Thread.Sleep(1500);
            _parentViewModel.ChangePage(_previousViewModel);
            await successRegistration.CloseViewModelAsync(true);
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
