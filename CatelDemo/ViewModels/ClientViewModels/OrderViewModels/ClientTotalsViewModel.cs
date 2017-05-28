using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Logic;

namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
    public class ClientTotalsViewModel : ViewModelBase
    {
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly User _user;
	    private readonly Reservation _reservation;
	    private readonly IViewModel _rootViewModel;
	    private readonly OrderedSumCalculator _sumCalculator;

		public ClientTotalsViewModel(User user, Reservation reservation, FastObservableCollection<OrderedDish> orderedDishes)
        {
			OrderedDishes = orderedDishes;

			_user = user;
			_reservation = reservation;
			_sumCalculator = new OrderedSumCalculator(orderedDishes);

			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

			BackCommand = new Command(OnBackCommandExecute);
			OrderCommand = new Command(OnOrderCommandExecute);

			FillViewModelProperties();
        }

	    public string UserLogin
		{
			get { return GetValue<string>(UserLoginProperty); }
			set { SetValue(UserLoginProperty, value); }
		}
		public static readonly PropertyData UserLoginProperty = RegisterProperty("UserLogin", typeof(string));

		public int TableNumber
		{
			get { return GetValue<int>(TableNumberProperty); }
			set { SetValue(TableNumberProperty, value); }
		}
		public static readonly PropertyData TableNumberProperty = RegisterProperty("TableNumber", typeof(int));

		public string VisitDate
		{
			get { return GetValue<string>(VisitDateProperty); }
			set { SetValue(VisitDateProperty, value); }
		}
		public static readonly PropertyData VisitDateProperty = RegisterProperty("VisitDate", typeof(string));

		public string FirstTime
		{
			get { return GetValue<string>(FirstTimeProperty); }
			set { SetValue(FirstTimeProperty, value); }
		}
		public static readonly PropertyData FirstTimeProperty = RegisterProperty("FirstTime", typeof(string));

		public string LastTime
		{
			get { return GetValue<string>(LastTimeProperty); }
			set { SetValue(LastTimeProperty, value); }
		}
		public static readonly PropertyData LastTimeProperty = RegisterProperty("LastTime", typeof(string));

		public FastObservableCollection<OrderedDish> OrderedDishes
		{
			get { return GetValue<FastObservableCollection<OrderedDish>>(OrderedDishesProperty); }
			set { SetValue(OrderedDishesProperty, value); }
		}
		public static readonly PropertyData OrderedDishesProperty = RegisterProperty("OrderedDishes", typeof(FastObservableCollection<OrderedDish>), 
			new FastObservableCollection<OrderedDish>());

		public int TotalSum
		{
			get { return GetValue<int>(TotalSumProperty); }
			set { SetValue(TotalSumProperty, value); }
		}
		public static readonly PropertyData TotalSumProperty = RegisterProperty("TotalSum", typeof(int));


		public BonusAction Bonus
		{
			get { return GetValue<BonusAction>(BonusProperty); }
			set { SetValue(BonusProperty, value); }
		}
		public static readonly PropertyData BonusProperty = RegisterProperty("Bonus", typeof(BonusAction));


		public string BonusHeaderString
		{
			get { return GetValue<string>(BonusHeaderStringProperty); }
			set { SetValue(BonusHeaderStringProperty, value); }
		}
		public static readonly PropertyData BonusHeaderStringProperty = RegisterProperty("BonusHeaderString", typeof(string));

		public string BonusInfo
		{
			get { return GetValue<string>(BonusInfoProperty); }
			set { SetValue(BonusInfoProperty, value); }
		}
		public static readonly PropertyData BonusInfoProperty = RegisterProperty("BonusInfo", typeof(string));


	    public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new ClientMenuViewModel(_user, _reservation, OrderedDishes));
		}


		public Command OrderCommand { get; private set; }
		private void OnOrderCommandExecute()
		{
			SaveAllToDataBase();
			_rootViewModel.ChangePage(new ClientMainViewModel(_user));
		}


	    protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }

		private void FillViewModelProperties()
		{
			UserLogin = _user.Login;

			TableNumber = _reservation.TableId;

			VisitDate = _reservation.Day.ToLongDateString();
			FirstTime = _reservation.FirstTime.ToShortTimeString();
			LastTime = _reservation.LastTime.ToShortTimeString();

			TotalSum = _sumCalculator.GetCurrentOrderedSum();

			var bonuses = _unitOfWork.BonusActions.GetAll().Where(b => b.ExcessSum < TotalSum);
			var bonus = bonuses.OrderBy(b => b.ExcessSum).LastOrDefault();

			if (bonus != null)
			{
				BonusHeaderString = $"{bonus.Dish.Name} | ЗАКАЗ > {bonus.ExcessSum}";
				BonusInfo = bonus.Description;
			}
			else
			{
				BonusInfo = "Извините, но за Ваш заказ бонус не предусмотрен.";
			}
		}

		private void SaveAllToDataBase()
		{
			_unitOfWork.Reservations.Insert(_reservation);
			_unitOfWork.SaveChanges();
			// получаем индекс только что созданной брони, который сгенерировала БД
			var reservationId = _unitOfWork.Reservations.GetAll().Max(r => r.Id);

			Order order = new Order
			{
				UserId = _user.Id,
				ReservationId = reservationId
			};

			_unitOfWork.Orders.Insert(order);
			_unitOfWork.SaveChanges();
			// получаем индекс только что созданного заказа, который сгенерировала БД
			var orderId = _unitOfWork.Orders.GetAll().Max(o => o.Id);

			foreach (var dish in OrderedDishes)
			{
				dish.OrderId = orderId;
				_unitOfWork.OrderedDishes.Insert(dish);
			}

			// TODO: добавить бонусное блюдо
			_unitOfWork.SaveChanges();
		}
	}
}
