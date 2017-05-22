using RestaurantHelper.ViewModels.ManagerViewModels;

namespace RestaurantHelper.Views.ManagerViews
{
	using Catel.Windows;
	using ViewModels;

	public partial class AddDishView
	{
		public AddDishView()
			: this(null) { }

		public AddDishView(AddDishViewModel viewModel)
			: base(viewModel, DataWindowMode.Custom)
		{
			InitializeComponent();
		}
	}
}
