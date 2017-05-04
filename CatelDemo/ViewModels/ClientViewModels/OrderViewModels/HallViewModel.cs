﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.ViewModels;

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

	        DateTime today = DateTime.Now;
	        if (today.Hour >= 22)
	        {
		        today = today.AddDays(1);
	        }
		    MinimumDate = $"{today.Month}.{today.Day}.{today.Year}";

			DateTime maxDate = today.AddDays(10).Date;
			MaximumDate = $"{maxDate.Month}.{maxDate.Day}.{maxDate.Year}";

			AddAllTablesToObservableCollection();
        }

		// TODO: Register models with the vmpropmodel codesnippet
		// TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

		
        public bool IsEnabledLastTime
        {
            get { return GetValue<bool>(IsEnabledLastTimeProperty); }
            set { SetValue(IsEnabledLastTimeProperty, value); }
        }
        public static readonly PropertyData IsEnabledLastTimeProperty = RegisterProperty("IsEnabledLastTime", typeof(bool), false);

		public string StartFirstTime
		{
			get { return GetValue<string>(StartFirstTimeProperty); }
			set { SetValue(StartFirstTimeProperty, value); }
		}
		public static readonly PropertyData StartFirstTimeProperty = RegisterProperty("StartFirstTime", typeof(string), "8:00");

        public string StartLastTime
        {
            get { return GetValue<string>(StartLastTimeProperty); }
            set { SetValue(StartLastTimeProperty, value); }
        }
        public static readonly PropertyData StartLastTimeProperty = RegisterProperty("StartLastTime", typeof(string));

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
        public static readonly PropertyData MaximumDateProperty = RegisterProperty("MaximumDate", typeof(string));

        public Command TimeValuChangedCommand { get; private set; }
        private void OnTimeValuChangedCommandExecute()
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(200); // ожидание окончания события и непосредственного изменения свойства

                int currentFirstTime = DateTime.Parse(FirstTime).Hour;
	            //MessageBox.Show("change");
                StartLastTime = LastTime = $"{currentFirstTime + 1}:00";
                IsEnabledLastTime = (currentFirstTime != 23);
            });
            thread.Start();
        }

        public Command DateValueChangedCommand { get; private set; }
        private void OnDateValueChangedCommandExecute()
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(200); // ожидание окончания события и непосредственного изменения свойства

                if (DateTime.Parse(DateText).Date == DateTime.Today)
                {
                    int nowHour = DateTime.Now.Hour;
	                if (nowHour < 8) nowHour = 8;
	                else nowHour++;
                    StartFirstTime = FirstTime = $"{nowHour}:00";
                }
                else
                {
	                StartFirstTime = "8:00";
                }
			});
            thread.Start();
        }

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

		public string DateText
		{
			get { return GetValue<string>(DateTextProperty); }
			set { SetValue(DateTextProperty, value); }
		}
		public static readonly PropertyData DateTextProperty = RegisterProperty("DateText", typeof(string));

		public ObservableCollection<Table> Tables
		{
			get { return GetValue<ObservableCollection<Table>>(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}
		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof(ObservableCollection<Table>),
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

	    private int Hour() => DateTime.Now.Hour;
	    private int Day() => DateTime.Today.Day;
    }
}
