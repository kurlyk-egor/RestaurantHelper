using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Views;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;
using RestaurantHelper.Services.Other.HallPickersHelpers;


namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
	public class HallViewModel : ViewModelBase
	{
		private readonly IViewModel _parentViewModel;
		private readonly User _user;
		private readonly IViewModel _rootViewModel;
		private readonly TableRepository _tableRepository;
		private readonly ReservationRepository _reservationRepository;
		private readonly TablesAvailabilityChecker _availabilityChecker;

		public HallViewModel(IViewModel parentViewModel, User user)
		{
			_parentViewModel = parentViewModel;
			_user = user;
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			_tableRepository = TableRepository.GetRepositoryInstance();
			_reservationRepository = ReservationRepository.GetRepositoryInstance();
			_availabilityChecker = new TablesAvailabilityChecker(_tableRepository.GetCollection());

			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);

			TimeValuChangedCommand = new Command(OnTimeValuChangedCommandExecute);
			DateValueChangedCommand = new Command(OnDateValueChangedCommandExecute);
			TableSelectionChanged = new Command(OnTableSelectionChangedExecute);

			DateMinMaxHelper helper = new DateMinMaxHelper();
			MinimumDate = helper.Minimum;
			MaximumDate = helper.Maximum;

			AddAllTablesToObservableCollection();
			TableReservations.Clear();
			ReservationsListRefresh();
		}


		#region Additional Properties and Commands

		[ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
		public bool IsEnabledTimePickers
		{
			get { return GetValue<bool>(IsEnabledTimePickersProperty); }
			set { SetValue(IsEnabledTimePickersProperty, value); }
		}
		public static readonly PropertyData IsEnabledTimePickersProperty = RegisterProperty("IsEnabledTimePickers", typeof (bool), false);

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

		public Visibility CaptionVisibility
		{
			get { return GetValue<Visibility>(CaptionVisibilityProperty); }
			set { SetValue(CaptionVisibilityProperty, value); }
		}
		public static readonly PropertyData CaptionVisibilityProperty = RegisterProperty("CaptionVisibility", typeof(Visibility), Visibility.Visible);


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

			ReservationsListRefresh(); // дата установлена
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

		public ObservableCollection<Table> Tables
		{
			get { return GetValue<ObservableCollection<Table>>(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}

		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof (ObservableCollection<Table>),
			new ObservableCollection<Table>());

		public Table SelectedItemTable
		{
			get { return GetValue<Table>(SelectedItemTableProperty); }
			set { SetValue(SelectedItemTableProperty, value); }
		}

		public static readonly PropertyData SelectedItemTableProperty = RegisterProperty("SelectedItemTable", typeof(Table));

		public ObservableCollection<Reservation> TableReservations
		{
			get { return GetValue<ObservableCollection<Reservation>>(TableReservationsProperty); }
			set { SetValue(TableReservationsProperty, value); }
		}
		public static readonly PropertyData TableReservationsProperty = RegisterProperty("TableReservations", typeof(ObservableCollection<Reservation>), 
			new ObservableCollection<Reservation>());


		public Command TableSelectionChanged { get; private set; }
		private void OnTableSelectionChangedExecute()
		{
			CaptionVisibility = Visibility.Collapsed;
			ReservationsListRefresh();
		}

		public Command BackCommand { get; private set; }
		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(_parentViewModel);
		}


		public Command NextCommand { get; private set; }
		private bool OnNextCommandCanExecute()
		{
			ReservationsListRefresh();
			bool result = false;
			if (!string.IsNullOrEmpty(FirstTime) && !string.IsNullOrEmpty(LastTime) && !string.IsNullOrEmpty(DateText))
			{
				_availabilityChecker.FillAvailabilities(DateTime.Parse(FirstTime), DateTime.Parse(LastTime),
					DateTime.Parse(DateText));
				result = true;
			}
			if (result)
			{
				result = SelectedItemTable != null && SelectedItemTable.Availability;
			}
			return result;
		}
		private void OnNextCommandExecute()
		{
			Reservation reservation = new Reservation(_user.Id, SelectedItemTable.Number,
				DateTime.Parse(FirstTime), DateTime.Parse(LastTime), DateTime.Parse(DateText).Date);

			_reservationRepository.Insert(reservation);
			// TODO: сохранять изменения только после подтверждения заказа!
			_reservationRepository.SaveChanges();

			_rootViewModel.ChangePage(new MenuViewModel(this, reservation));
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

		private void AddAllTablesToObservableCollection()
		{
			((ICollection<Table>) Tables).AddRange(_tableRepository.GetCollection());
		}

		private void ReservationsListRefresh()
		{
			TableReservations.Clear();
			_availabilityChecker.ResetValues();
			if (!string.IsNullOrEmpty(FirstTime) && !string.IsNullOrEmpty(LastTime) && !string.IsNullOrEmpty(DateText))
			{
				_availabilityChecker.FillAvailabilities(DateTime.Parse(FirstTime), DateTime.Parse(LastTime), DateTime.Parse(DateText));

				((ICollection<Reservation>)TableReservations).AddRange(_availabilityChecker
					.GetDaylyReservationsForTable(DateTime.Parse(DateText), SelectedItemTable.Number));
			}
		}
	}
}
