using RestaurantHelper.ViewModels.ManagerViewModels;

namespace RestaurantHelper.Views.ManagerViews
{
	using Catel.Windows;
	using ViewModels;

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
