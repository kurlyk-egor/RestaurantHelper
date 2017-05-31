using System.Collections.ObjectModel;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class ManagerReportsViewModel : ViewModelBase
	{
		private ReportsCreator _creator;

		public ManagerReportsViewModel()
		{
			PeriodSelectionChangedCommand = new Command(OnPeriodSelectionChangedExecute);
			OrderSelectionChangedCommand = new Command(OnOrderSelectionChangedExecute);
			OrderedDishSelectionChangedCommand = new Command(OnOrderedDishSelectionChangedExecute);
			BonusDishSelectionChangedCommand = new Command(OnBonusDishSelectionChangedExecute);

			RefreshCollections();
		}

		public FastObservableCollection<Period> Periods
		{
			get { return GetValue<FastObservableCollection<Period>>(PeriodsProperty); }
			set { SetValue(PeriodsProperty, value); }
		}

		public static readonly PropertyData PeriodsProperty = RegisterProperty("Periods", typeof (FastObservableCollection<Period>),
			new FastObservableCollection<Period> {Period.Today, Period.Week, Period.Month});

		public Period SelectedPeriod
		{
			get { return GetValue<Period>(SelectedPeriodProperty); }
			set { SetValue(SelectedPeriodProperty, value); }
		}
		public static readonly PropertyData SelectedPeriodProperty = RegisterProperty("SelectedPeriod", typeof(Period), Period.Today);


		public FastObservableCollection<Order> Orders
		{
			get { return GetValue<FastObservableCollection<Order>>(OrdersProperty); }
			set { SetValue(OrdersProperty, value); }
		}
		public static readonly PropertyData OrdersProperty = RegisterProperty("Orders", typeof(FastObservableCollection<Order>), 
			new FastObservableCollection<Order>());

		public Order SelectedOrder
		{
			get { return GetValue<Order>(SelectedOrderProperty); }
			set { SetValue(SelectedOrderProperty, value); }
		}
		public static readonly PropertyData SelectedOrderProperty = RegisterProperty("SelectedOrder", typeof(Order), null);


		public FastObservableCollection<OrderedDish> OrderedDishes
		{
			get { return GetValue<FastObservableCollection<OrderedDish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(FastObservableCollection<OrderedDish>), 
			new FastObservableCollection<OrderedDish>());

		/// <summary>
			/// Gets or sets the property value.
			/// </summary>
		public OrderedDish SelectedOrderedDish
		{
			get { return GetValue<OrderedDish>(SelectedOrderedDishProperty); }
			set { SetValue(SelectedOrderedDishProperty, value); }
		}
		public static readonly PropertyData SelectedOrderedDishProperty = RegisterProperty("SelectedOrderedDish", typeof(OrderedDish));

		public FastObservableCollection<OrderedDish> BonusDishes
		{
			get { return GetValue<FastObservableCollection<OrderedDish>>(BonusDishesProperty); }
			set { SetValue(BonusDishesProperty, value); }
		}
		public static readonly PropertyData BonusDishesProperty = RegisterProperty("BonusDishes", typeof(FastObservableCollection<OrderedDish>),
			new FastObservableCollection<OrderedDish>());

		public OrderedDish SelectedBonusDish
		{
			get { return GetValue<OrderedDish>(SelectedBonusDishProperty); }
			set { SetValue(SelectedBonusDishProperty, value); }
		}
		public static readonly PropertyData SelectedBonusDishProperty = RegisterProperty("SelectedBonusDish", typeof(OrderedDish));


		/// <summary>
		/// доходы
		/// </summary>
		public int Incomes
		{
			get { return GetValue<int>(IncomesProperty); }
			set { SetValue(IncomesProperty, value); }
		}
		public static readonly PropertyData IncomesProperty = RegisterProperty("Incomes", typeof(int));

		/// <summary>
		/// расходы
		/// </summary>
		public int Costs
		{
			get { return GetValue<int>(CostsProperty); }
			set { SetValue(CostsProperty, value); }
		}
		public static readonly PropertyData CostsProperty = RegisterProperty("Costs", typeof(int));

		/// <summary>
		/// прибыль
		/// </summary>
		public int Profit
		{
			get { return GetValue<int>(ProfitProperty); }
			set { SetValue(ProfitProperty, value); }
		}
		public static readonly PropertyData ProfitProperty = RegisterProperty("Profit", typeof(int));

		/// <summary>
		/// количество клиентов за период
		/// </summary>
		public int ClientsCount
		{
			get { return GetValue<int>(ClientsCountProperty); }
			set { SetValue(ClientsCountProperty, value); }
		}
		public static readonly PropertyData ClientsCountProperty = RegisterProperty("ClientsCount", typeof(int));

		/// <summary>
		/// всего заказано блюд за период
		/// </summary>
		public int DishesCount
		{
			get { return GetValue<int>(DishesCountProperty); }
			set { SetValue(DishesCountProperty, value); }
		}
		public static readonly PropertyData DishesCountProperty = RegisterProperty("DishesCount", typeof(int));



		public Command PeriodSelectionChangedCommand { get; private set; }
		private void OnPeriodSelectionChangedExecute()
		{
			RefreshCollections();
		}

		public Command OrderSelectionChangedCommand { get; private set; }
		private void OnOrderSelectionChangedExecute()
		{
			
		}

		public Command OrderedDishSelectionChangedCommand { get; private set; }
		private void OnOrderedDishSelectionChangedExecute()
		{
			
		}

		public Command BonusDishSelectionChangedCommand { get; private set; }
		private void OnBonusDishSelectionChangedExecute()
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

		private void RefreshCollections()
		{
			_creator = new ReportsCreator(SelectedPeriod);
			_creator.CreateOrders(Orders);
			_creator.CreateOrderedDishes(OrderedDishes);
			_creator.CreateBonusDishes(BonusDishes);

			Incomes = _creator.GetIncomes();
			Costs = _creator.GetCosts();
			Profit = Incomes - Costs;
			DishesCount = _creator.GetDishesCount();
			ClientsCount = _creator.GetClientsCount();
		}
	}
}
