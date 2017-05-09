using System.Threading.Tasks;
using Catel.MVVM;
using RestaurantHelper.Models;

namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IViewModel _parentViewModel;
	    private readonly Reservation _reservation;

	    public MenuViewModel(IViewModel parentViewModel, Reservation reservation)
	    {
		    _parentViewModel = parentViewModel;
		    _reservation = reservation;
	    }

		



	    // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
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
