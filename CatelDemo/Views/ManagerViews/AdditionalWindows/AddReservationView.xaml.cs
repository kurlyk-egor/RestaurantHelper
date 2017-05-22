using Catel.Windows;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;

namespace RestaurantHelper.Views.ManagerViews.AdditionalWindows
{
	public partial class AddReservationView
	{
		public AddReservationView()
			: this(null) { }

		public AddReservationView(AddReservationViewModel viewModel)
			: base(viewModel, DataWindowMode.Custom)
		{
			InitializeComponent();
		}
	}
}
