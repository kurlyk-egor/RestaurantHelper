using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Views;
using RestaurantHelper.DAL;
using RestaurantHelper.DAL.Repositories;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;
using RestaurantHelper.Services.Logic.HallPickersHelpers;


namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
	public class ClientHallViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly User _user;
		private readonly FastObservableCollection<OrderedDish> _orderedDishes;
		private readonly IViewModel _rootViewModel;
		private readonly Reservation _reservation;
		private readonly TablesAvailabilityChecker _availabilityChecker;

		public ClientHallViewModel(User user, Reservation reservation = null, FastObservableCollection<OrderedDish> orderedDishes = null)
		{
			_user = user;
			_orderedDishes = orderedDishes;
			_reservation = new Reservation();
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			
			_availabilityChecker = new TablesAvailabilityChecker(Tables);

			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);
			TimeValuChangedCommand = new Command(OnTimeValuChangedCommandExecute);
			DateValueChangedCommand = new Command(OnDateValueChangedCommandExecute);
			TableSelectionChanged = new Command(OnTableSelectionChangedExecute);

			DateMinMaxHelper helper = new DateMinMaxHelper();
			MinimumDate = helper.Minimum;
			MaximumDate = helper.Maximum;

			RefreshTablesCollection();
			ReservationsListRefresh();

			if (reservation != null)
			{
				SetViewModelProperties(reservation);
			}
		}

		#region Additional Properties and Commands

		[ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
		public bool IsEnabledTimePickers
		{
			get { return GetValue<bool>(IsEnabledTimePickersProperty); }
			set { SetValue(IsEnabledTimePickersProperty, value); }
		}
		public static readonly PropertyData IsEnabledTimePickersProperty = RegisterProperty("IsEnabledTimePickers", typeof(bool), false);

		public string StartFirstTime
		{
			get { return GetValue<string>(StartFirstTimeProperty); }
			set { SetValue(StartFirstTimeProperty, value); }
		}
		public static readonly PropertyData StartFirstTimeProperty = RegisterProperty("StartFirstTime", typeof (string), "8:00");

		public string StartLastTime
		{
			get { return GetValue<string>(StartLastTimeProperty); }
			set { SetValue(StartLastTimeProperty, value); }
		}
		public static readonly PropertyData StartLastTimeProperty = RegisterProperty("StartLastTime", typeof (string));

		public string MinimumDate
		{
			get { return GetValue<string>(MinimumDateProperty); }
			set { SetValue(MinimumDateProperty, value); }
		}
		public static readonly PropertyData MinimumDateProperty = RegisterProperty("MinimumDate", typeof (string));

		public string MaximumDate
		{
			get { return GetValue<string>(MaximumDateProperty); }
			set { SetValue(MaximumDateProperty, value); }
		}
		public static readonly PropertyData MaximumDateProperty = RegisterProperty("MaximumDate", typeof (string));
	
		public bool ErrorVisibility
		{
			get { return GetValue<bool>(ErrorVisibilityProperty); }
			set { SetValue(ErrorVisibilityProperty, value); }
		}
		public static readonly PropertyData ErrorVisibilityProperty = RegisterProperty("ErrorVisibility", typeof(bool), false);

		public string ConflictOrderInfo
		{
			get { return GetValue<string>(ConflictOrderInfoProperty); }
			set { SetValue(ConflictOrderInfoProperty, value); }
		}
		public static readonly PropertyData ConflictOrderInfoProperty = RegisterProperty("ConflictOrderInfo", typeof(string));


		public Command TimeValuChangedCommand { get; private set; }
		private void OnTimeValuChangedCommandExecute() // выбрали какое то время
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(200); // ожидание непосредственного изменения свойства

				LastTimePickerHelper helper = new LastTimePickerHelper(FirstTime);
				StartLastTime = helper.StartLastTime;
				LastTime = helper.LastTime;
				IsEnabledTimePickers = true;
			});
			thread.Start();
		}

		public Command DateValueChangedCommand { get; private set; }
		private void OnDateValueChangedCommandExecute() // выбрали дату
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(200); // ожидание непосредственного изменения свойства

				FirstTimePickerHelper helper = new FirstTimePickerHelper(DateText);
				StartFirstTime = helper.StartFirstTime;
				FirstTime = helper.FirstTime;
			});
			thread.Start();
		}
		#endregion



		#region View Model Properties and Commands

		public string FirstTime
		{
			get { return GetValue<string>(FirstTimeProperty); }
			set { SetValue(FirstTimeProperty, value); }
		}

		public static readonly PropertyData FirstTimeProperty = RegisterProperty("FirstTime", typeof (string));

		public string LastTime
		{
			get { return GetValue<string>(LastTimeProperty); }
			set { SetValue(LastTimeProperty, value); }
		}

		public static readonly PropertyData LastTimeProperty = RegisterProperty("LastTime", typeof (string));

		public string DateText
		{
			get { return GetValue<string>(DateTextProperty); }
			set { SetValue(DateTextProperty, value); }
		}

		public static readonly PropertyData DateTextProperty = RegisterProperty("DateText", typeof (string));

		public FastObservableCollection<Table> Tables
		{
			get { return GetValue<FastObservableCollection<Table>>(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}
		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof (FastObservableCollection<Table>),
			new FastObservableCollection<Table>());

		public Table SelectedItemTable
		{
			get { return GetValue<Table>(SelectedItemTableProperty); }
			set { SetValue(SelectedItemTableProperty, value); }
		}
		public static readonly PropertyData SelectedItemTableProperty = RegisterProperty("SelectedItemTable", typeof(Table));

		public FastObservableCollection<Reservation> TableReservations
		{
			get { return GetValue<FastObservableCollection<Reservation>>(TableReservationsProperty); }
			set { SetValue(TableReservationsProperty, value); }
		}
		public static readonly PropertyData TableReservationsProperty = RegisterProperty("TableReservations", typeof(FastObservableCollection<Reservation>), 
			new FastObservableCollection<Reservation>());


		public Command TableSelectionChanged { get; private set; }
		private void OnTableSelectionChangedExecute()
		{
			ReservationsListRefresh();
		}

		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(new ClientMainViewModel(_user));
		}

		public Command NextCommand { get; private set; }

		private bool OnNextCommandCanExecute()
		{
			ReservationsListRefresh();
			CheckOtherClientOrders();

			bool result = false;
			if (!string.IsNullOrEmpty(FirstTime) && !string.IsNullOrEmpty(LastTime) && !string.IsNullOrEmpty(DateText))
			{
				_availabilityChecker.SetTablesAvailabilities(FirstTime, LastTime, DateText);
				result = true;
			}
			if (result)
			{
				result = SelectedItemTable != null && SelectedItemTable.Availability && !ErrorVisibility;
			}
			return result;
		}
		private void OnNextCommandExecute()
		{
			SaveReservation();
			_rootViewModel.ChangePage(new ClientMenuViewModel(_user, _reservation, _orderedDishes));
		}

		#endregion


		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void RefreshTablesCollection()
		{
			Tables.Clear();
			Tables.AddItems(_unitOfWork.Tables.GetAll());
			_availabilityChecker.ResetValues();
		}

		private void ReservationsListRefresh()
		{
			TableReservations.Clear();
			if (!string.IsNullOrEmpty(FirstTime) && !string.IsNullOrEmpty(LastTime) && !string.IsNullOrEmpty(DateText))
			{
				_availabilityChecker.SetTablesAvailabilities(FirstTime, LastTime, DateText);

				if (SelectedItemTable != null)
				{
					TableReservations.AddItems(_availabilityChecker.GetDaylyReservationsForTable(DateText, SelectedItemTable.Number));
				}
			}
		}

		private void CheckOtherClientOrders()
		{
			string info;
			ErrorVisibility = _availabilityChecker.IsErrorClientReservation(_user, DateText, FirstTime, LastTime, out info);
			ConflictOrderInfo = info;

		}

		private void SetViewModelProperties(Reservation reservation)
		{	
			FirstTimePickerHelper fHelper = new FirstTimePickerHelper(reservation.Day.ToShortDateString());
			StartFirstTime = fHelper.StartFirstTime;
			FirstTime = reservation.FirstTime.ToShortTimeString();

			LastTimePickerHelper lHelper = new LastTimePickerHelper(FirstTime);
			StartLastTime = lHelper.StartLastTime;
			LastTime = reservation.LastTime.ToShortTimeString();

			DateText = reservation.Day.ToShortDateString();
			SelectedItemTable = Tables.FirstOrDefault(table => table.Id == reservation.TableId);
			IsEnabledTimePickers = true;
		}

		private void SaveReservation()
		{
			_reservation.FirstTime = DateTime.Parse(FirstTime);
			_reservation.LastTime = DateTime.Parse(LastTime);
			_reservation.Day = DateTime.Parse(DateText);
			_reservation.TableId = SelectedItemTable.Number;
		}
	}
}
