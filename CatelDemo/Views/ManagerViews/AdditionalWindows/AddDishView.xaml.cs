using Catel.Windows;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;

namespace RestaurantHelper.Views.ManagerViews.AdditionalWindows
{
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
