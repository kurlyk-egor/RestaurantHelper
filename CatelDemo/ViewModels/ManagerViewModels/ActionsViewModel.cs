using System.Threading.Tasks;
using Catel.MVVM;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ActionsViewModel : ViewModelBase
	{
		public ActionsViewModel()
		{
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
