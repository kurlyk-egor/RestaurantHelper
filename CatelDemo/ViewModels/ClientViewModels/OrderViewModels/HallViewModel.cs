using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using Catel.MVVM.Views;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Other.HallPickersHelpers;


namespace RestaurantHelper.ViewModels.ClientViewModels.OrderViewModels
{
	public class HallViewModel : ViewModelBase
	{
		private readonly IViewModel _parentViewModel;
		private readonly IViewModel _rootViewModel;
		private readonly TableRepository _tableRepository;

		public HallViewModel(IViewModel parentViewModel)
		{
			_parentViewModel = parentViewModel;
			_rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();
			_tableRepository = TableRepository.GetRepositoryInstance();

			BackCommand = new Command(OnBackCommandExecute);
			NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);
			TimeValuChangedCommand = new Command(OnTimeValuChangedCommandExecute);
			DateValueChangedCommand = new Command(OnDateValueChangedCommandExecute);

			DateMinMaxHelper helper = new DateMinMaxHelper();
			MinimumDate = helper.Minimum;
			MaximumDate = helper.Maximum;

			AddAllTablesToObservableCollection();
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

		public Command TimeValuChangedCommand { get; private set; }

		private void OnTimeValuChangedCommandExecute()
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

		private void OnDateValueChangedCommandExecute()
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


		public Command BackCommand { get; private set; }

		private void OnBackCommandExecute()
		{
			_rootViewModel.ChangePage(_parentViewModel);
		}

		public Command NextCommand { get; private set; }

		private bool OnNextCommandCanExecute()
		{
			// TODO: добавить обработчик доступности кнопки
			return true;
		}

		private void OnNextCommandExecute()
		{
			// TODO: включить переход
			//_rootViewModel.ChangePage(new MenuViewModel(this));
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
	}
}
