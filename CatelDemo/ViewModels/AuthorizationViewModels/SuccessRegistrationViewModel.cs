using System.Threading;
using System.Threading.Tasks;
using Catel.MVVM;

namespace CatelDemo.ViewModels.AuthorizationViewModels
{
    public class SuccessRegistrationViewModel : ViewModelBase
    {
        public SuccessRegistrationViewModel()
        {
            
        }

        private async void OnOnLoadedExecute()
        {
            // TODO: Handle command logic here
            Thread.Sleep(1000);
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
