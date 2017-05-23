using System.Threading;
using System.Threading.Tasks;
using Catel.MVVM;

namespace RestaurantHelper.ViewModels.AuthorizationViewModels
{
    public class SuccessRegistrationViewModel : ViewModelBase
    {
        public SuccessRegistrationViewModel()
        {
        }

        private async void OnOnLoadedExecute()
        {
            Thread.Sleep(800);
            await CloseAsync();
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
