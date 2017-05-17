using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerMenuViewModel : ViewModelBase
	{
		private readonly ManagerMenuChanger _menuChanger;
		public ManagerMenuViewModel()
		{
			_menuChanger = new ManagerMenuChanger();
			Dishes = _menuChanger.LoadAllDishes();
		}

		public ObservableCollection<Dish> Dishes
		{
			get { return GetValue<ObservableCollection<Dish>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(ObservableCollection<Dish>), 
			new ObservableCollection<Dish>());

		public Dish SelectedDish
		{
			get { return GetValue<Dish>(SelectedDishProperty); }
			set { SetValue(SelectedDishProperty, value); }
		}
		public static readonly PropertyData SelectedDishProperty = RegisterProperty("SelectedDish", typeof(Dish));


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
