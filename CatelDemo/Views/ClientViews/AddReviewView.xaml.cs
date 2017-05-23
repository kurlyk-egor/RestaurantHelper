using RestaurantHelper.ViewModels.ClientViewModels;

namespace RestaurantHelper.Views.ClientViews
{
	using Catel.Windows;
	using ViewModels;

	public partial class AddReviewView
	{
		public AddReviewView()
			: this(null) { }

		public AddReviewView(AddReviewViewModel viewModel)
			: base(viewModel, DataWindowMode.Custom)
		{
			InitializeComponent();
		}
	}
}
