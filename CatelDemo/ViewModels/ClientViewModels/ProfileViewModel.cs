using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.ViewModels;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IViewModel _previousViewModel;
        private readonly UserRepository _repository;
        public ProfileViewModel(IViewModel previousViewModel, User user)
        {
            _previousViewModel = previousViewModel;
            User = user;
            _repository = UserRepository.GetRepositoryInstance();

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

        public Command SaveCommand { get; private set; }
        private bool OnSaveCommandCanExecute()
        {
            long outlong;
            bool result = !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password)
                          && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone)
                          && long.TryParse(Phone, out outlong);
            return result;
        }
        private void OnSaveCommandExecute()
        {
            _repository.Update(User);
            _repository.SaveChanges();
        }

        public Command BackCommand { get; private set; }
        private void OnBackCommandExecute()
        {
            var parent = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
            parent.ChangeCurrentPage(_previousViewModel);
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
