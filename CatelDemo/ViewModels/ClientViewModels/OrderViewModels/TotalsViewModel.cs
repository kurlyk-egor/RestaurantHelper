namespace CatelDemo.ViewModels.ClientViewModels.OrderViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class TotalsViewModel : ViewModelBase
    {
        private readonly IViewModel _parentViewModel;

        public TotalsViewModel(IViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
        }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

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
