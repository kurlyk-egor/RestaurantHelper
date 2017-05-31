using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Logic;
using RestaurantHelper.ViewModels.ManagerViewModels.AdditionalWindows;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerHallViewModel : ViewModelBase
	{
		private const string TOOL_TIP_MESSAGE = "Довавить бронь текущему столику";

		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly IViewModel _root;
		private readonly TablesAvailabilityRejuvenator _tablesAvailabilityRejuvenator;
		private readonly AdminReservationsCreator _timeSelector;
		private readonly ClientsForTableSelector _selector;
		private readonly Dispatcher _dispatcher;

		public ManagerHallViewModel()
		{
			_tablesAvailabilityRejuvenator = new TablesAvailabilityRejuvenator(Tables);
			_root = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			_timeSelector = new AdminReservationsCreator();
			_selector =  new ClientsForTableSelector();

			// присваивание в конструкторе должно гарантировать поток UI
			_dispatcher = Dispatcher.CurrentDispatcher;

			AddAllTablesToFastObservableCollection();
			RenewPropertiesStart();
			RenewTooltipMessageStart();

			DeleteReservationCommand = new Command(OnDeleteReservationCommandExecute, OnDeleteReservationCommandCanExecute);
			ReservationSelectionChangedCommand = new Command(OnReservationSelectionChangedCommandExecute);
			TableSelectionChangedCommand = new Command(OnTableSelectionChangedCommandExecute);
			AddReservationCommand = new Command(OnAddReservationCommandExecute);
		}

		public FastObservableCollection<Table> Tables
		{
			get { return GetValue<FastObservableCollection<Table> >(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}
		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof(FastObservableCollection<Table> ), 
			new FastObservableCollection<Table>());

		public Table SelectedItemTable
		{
			get { return GetValue<Table>(SelectedItemTableProperty); }
			set { SetValue(SelectedItemTableProperty, value); }
		}
		public static readonly PropertyData SelectedItemTableProperty = RegisterProperty("SelectedItemTable", typeof(Table));


		public int FreeTablesCount
		{
			get { return GetValue<int>(FreeTablesCountProperty); }
			set { SetValue(FreeTablesCountProperty, value); }
		}
		public static readonly PropertyData FreeTablesCountProperty = RegisterProperty("FreeTablesCount", typeof(int));

		public FastObservableCollection<Reservation> TableReservations
		{
			get { return GetValue<FastObservableCollection<Reservation>>(TableReservationsProperty); }
			set { SetValue(TableReservationsProperty, value); }
		}	
		public static readonly PropertyData TableReservationsProperty = RegisterProperty("TableReservations", typeof(FastObservableCollection<Reservation>), 
			new FastObservableCollection<Reservation>());

		public Reservation SelectedReservation
		{
			get { return GetValue<Reservation>(SelectedReservationProperty); }
			set { SetValue(SelectedReservationProperty, value); }
		}
		public static readonly PropertyData SelectedReservationProperty = RegisterProperty("SelectedReservation", typeof(Reservation));


		public int BusyTablesCount
		{
			get { return GetValue<int>(BusyTablesCountProperty); }
			set { SetValue(BusyTablesCountProperty, value); }
		}
		public static readonly PropertyData BusyTablesCountProperty = RegisterProperty("BusyTablesCount", typeof(int));

		public string ToolTipText
		{
			get { return GetValue<string>(ToolTipTextProperty); }
			set { SetValue(ToolTipTextProperty, value); }
		}
		public static readonly PropertyData ToolTipTextProperty = RegisterProperty("ToolTipText", typeof(string), TOOL_TIP_MESSAGE);

		public bool IsEnabledAddButton
		{
			get { return GetValue<bool>(IsEnabledAddButtonProperty); }
			set { SetValue(IsEnabledAddButtonProperty, value); }
		}
		public static readonly PropertyData IsEnabledAddButtonProperty = RegisterProperty("IsEnabledAddButton", typeof (bool), false);

		public string TimeString
		{
			get { return GetValue<string>(TimeStringProperty); }
			set { SetValue(TimeStringProperty, value); }
		}
		public static readonly PropertyData TimeStringProperty = RegisterProperty("TimeString", typeof(string));

		public string ReservationClientName
		{
			get { return GetValue<string>(ReservationClientNameProperty); }
			set { SetValue(ReservationClientNameProperty, value); }
		}
		public static readonly PropertyData ReservationClientNameProperty = RegisterProperty("ReservationClientName", typeof(string));

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void AddAllTablesToFastObservableCollection()
		{
			Tables = _tablesAvailabilityRejuvenator.FillAllTables();
		}


		public Command DeleteReservationCommand { get; private set; }
		private bool OnDeleteReservationCommandCanExecute()
		{
			return SelectedReservation != null;
		}
		private void OnDeleteReservationCommandExecute()
		{
			if (! _timeSelector.CanReservationFree(SelectedReservation))
			{
				_root.ChangePageWithDialog(new ShortMessageViewModel("Вы не можете снять клиентскую бронь"), 2000);
				return;
			}
			_unitOfWork.Reservations.Delete(SelectedReservation.Id);
			_unitOfWork.SaveChanges();

			// имитация изменения выбора столика чтобы обновить текущий список броней
			OnTableSelectionChangedCommandExecute();
		}

		public Command AddReservationCommand { get; private set; }
		private void OnAddReservationCommandExecute()
		{
			if (SelectedItemTable != null && SelectedItemTable.Availability)
			{
				var visualizer = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
				bool? b = visualizer.ShowDialog(new AddReservationViewModel(SelectedItemTable));
				if (b != true)
				{
					_root.ChangePageWithDialog(new ShortMessageViewModel("Отмена..."), 800);
				}
				else
				{
					// имитация изменения выбора столика чтобы обновить текущий список броней
					OnTableSelectionChangedCommandExecute();
				}
			}
		}

		public Command TableSelectionChangedCommand { get; private set; }
		private void OnTableSelectionChangedCommandExecute()
		{
			TableReservations.Clear();
			if (SelectedItemTable != null)
			{
				var tableChecker = new TablesAvailabilityChecker(Tables);
				var reservations = tableChecker.GetTodayReservationsForTable(SelectedItemTable.Number);

				foreach (var reserv in reservations)
				{
					TableReservations.Add(reserv);
				}
			}
		}


		public Command ReservationSelectionChangedCommand { get; private set; }
		private void OnReservationSelectionChangedCommandExecute()
		{
			ReservationClientName = string.Empty;

			if (SelectedReservation != null)
			{
				var order = _unitOfWork.Orders.GetAll().FirstOrDefault(o => o.ReservationId == SelectedReservation.Id);
				if (order == null)
				{
					ReservationClientName = "Администратор";
				}
				else
				{
					ReservationClientName = order.User.Login;
				}
			}
		}

		private  void RenewPropertiesStart()
		{
			var thread = new Thread(() =>
			{
				while (true)
				{
					_dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
					{
						_tablesAvailabilityRejuvenator.RefreshTablesCollection();
						FreeTablesCount = _tablesAvailabilityRejuvenator.GetFreeTablesCount();
						BusyTablesCount = _tablesAvailabilityRejuvenator.GetBusyTablesCount();
						TimeString = DateTime.Now.ToShortTimeString();
					});

					Thread.Sleep(TimeSpan.FromSeconds(2));
				}
			});

			thread.IsBackground = true;
			thread.Start();
		}

		private void RenewTooltipMessageStart()
		{
			var thread = new Thread(() =>
			{
				while (true)
				{
					_dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
					{
						if (SelectedItemTable == null)
						{
							ToolTipText = "Столик не выбран";
							IsEnabledAddButton = false;
						}
						else if (!SelectedItemTable.Availability)
						{
							ToolTipText = "Выбранный столик недоступен";
							IsEnabledAddButton = false;
						}
						else if (_timeSelector.IsReservationNotAvailable(SelectedItemTable.Id))
						{
							ToolTipText = "Невозможно забронировать столик сейчас. Уже существуют другие брони!";
							IsEnabledAddButton = false;
						}
						else if (_timeSelector.IsIllegalReservaionTime(SelectedItemTable.Id))
						{
							ToolTipText = "В данное время невозможно добавить бронь";
							IsEnabledAddButton = false;
						}
						else
						{
							ToolTipText = TOOL_TIP_MESSAGE;
							IsEnabledAddButton = true;
						}
					});

					Thread.Sleep(200);
				}
			});

			thread.IsBackground = true;
			thread.Start();
		}
	}
}
