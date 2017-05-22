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
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerHallViewModel : ViewModelBase
	{
		private const string TOOL_TIP_MESSAGE = "Довавить бронь текущему столику";

		private readonly TablesAvailabilityRejuvenator _tablesAvailabilityRejuvenator;
		private readonly AdminReservationsCreator _timeSelector;
		private readonly ClientsForTableSelector _selector;
		private readonly Dispatcher _dispatcher;

		public ManagerHallViewModel()
		{
			_tablesAvailabilityRejuvenator = new TablesAvailabilityRejuvenator(Tables);
			_timeSelector = new AdminReservationsCreator();
			_selector =  new ClientsForTableSelector();

			// присваивание в конструкторе должно гарантировать поток UI
			_dispatcher = Dispatcher.CurrentDispatcher;

			AddAllTablesToObservableCollection();
			RenewPropertiesStart();
			RenewTooltipMessageStart();

			DeleteReservationCommand = new Command(OnDeleteReservationCommandExecute, OnDeleteReservationCommandCanExecute);
			TableSelectionChangedCommand = new Command(OnTableSelectionChangedCommandExecute);
			AddReservationCommand = new Command(OnAddReservationCommandExecute);
		}

		public ObservableCollection<Table> Tables
		{
			get { return GetValue<ObservableCollection<Table> >(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}
		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof(ObservableCollection<Table> ), 
			new ObservableCollection<Table>());

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

		public ObservableCollection<Reservation> TableReservations
		{
			get { return GetValue<ObservableCollection<Reservation>>(TableReservationsProperty); }
			set { SetValue(TableReservationsProperty, value); }
		}	
		public static readonly PropertyData TableReservationsProperty = RegisterProperty("TableReservations", typeof(ObservableCollection<Reservation>), 
			new ObservableCollection<Reservation>());

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

		public string SelectedTableClientName
		{
			get { return GetValue<string>(SelectedTableClientNameProperty); }
			set { SetValue(SelectedTableClientNameProperty, value); }
		}
		public static readonly PropertyData SelectedTableClientNameProperty = RegisterProperty("SelectedTableClientName", typeof(string));

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
			Tables = _tablesAvailabilityRejuvenator.FillAllTables();
		}


		public Command DeleteReservationCommand { get; private set; }
		private bool OnDeleteReservationCommandCanExecute()
		{
			return SelectedReservation != null;
		}
		private void OnDeleteReservationCommandExecute()
		{
			var reservationsRepo = new Repository<Reservation>();
			reservationsRepo.Delete(SelectedReservation);
			reservationsRepo.SaveChanges();

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
					MessageBox.Show("Бронь не была добавлена!");
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
				var tableChecker = new TablesAvailabilityChecker(Tables.ToList());
				var reservations = tableChecker.GetTodayReservationsForTable(SelectedItemTable.Number);
				var userName = _selector.GetUserNameForSelectedTable(SelectedItemTable.Id);
				SelectedTableClientName = userName;

				foreach (var reserv in reservations)
				{
					TableReservations.Add(reserv);
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
