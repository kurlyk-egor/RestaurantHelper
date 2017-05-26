using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerMenuViewModel : ViewModelBase
	{
		private readonly ManagerMenuChanger _menuChanger;
		public ManagerMenuViewModel()
		{
			_menuChanger = new ManagerMenuChanger();

			AddDishCommand = new Command(OnAddDishCommandExecute);
			DeleteDishCommand = new Command(OnDeleteDishCommandExecute, OnAnyDishCommandCanExecute);
			EditDishCommand = new Command(OnEditDishCommandExecute, OnAnyDishCommandCanExecute);

			DishesCollectionRefresh();
		}

		private void DishesCollectionRefresh()
		{
			Dishes = _menuChanger.LoadAllDishes();
		}

		public FastObservableCollection<Dish> Dishes
		{
			get { return GetValue<FastObservableCollection<Dish>>(DishesProperty); }
			set { SetValue(DishesProperty, value); }
		}
		public static readonly PropertyData DishesProperty = RegisterProperty("Dishes", typeof(FastObservableCollection<Dish>), 
			new FastObservableCollection<Dish>());

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

		public Command DeleteDishCommand { get; private set; }
		private void OnDeleteDishCommandExecute()
		{
			_menuChanger.DeleteDish(SelectedDish);
			DishesCollectionRefresh();
		}

		public Command AddDishCommand { get; private set; }
		private void OnAddDishCommandExecute()
		{
			var visualizer = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
			var addDishVm = new AddDishViewModel();

			if (visualizer.ShowDialog(addDishVm) == true)
			{
				Dishes.Add(addDishVm.Dish);
			}
		}

		public Command EditDishCommand { get; private set; }
		private void OnEditDishCommandExecute()
		{
			var visualizer = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
			var addDishVm = new AddDishViewModel(SelectedDish);

			visualizer.ShowDialog(addDishVm);
		}

		private bool OnAnyDishCommandCanExecute()
		{
			return SelectedDish != null;
		}
	}
}
