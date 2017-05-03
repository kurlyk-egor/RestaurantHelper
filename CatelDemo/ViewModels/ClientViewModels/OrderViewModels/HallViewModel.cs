using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Catel.Data;
using CatelDemo.Services.Database;
using CatelDemo.Services.Other;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace CatelDemo.ViewModels.ClientViewModels.OrderViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class HallViewModel : ViewModelBase
    {
        private readonly IViewModel _parentViewModel;
        private readonly IViewModel _rootViewModel;

        public HallViewModel(IViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            _rootViewModel = ViewModelManager.GetFirstOrDefaultInstance<MainWindowViewModel>();

            BackCommand = new Command(OnBackCommandExecute);
            NextCommand = new Command(OnNextCommandExecute, OnNextCommandCanExecute);
            TimeValuChangedCommand = new Command(OnTimeValuChangedCommandExecute);
            DateValueChangedCommand = new Command(OnDateValueChangedCommandExecute);
        }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
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

        public bool IsEnabledLastTime
        {
            get { return GetValue<bool>(IsEnabledLastTimeProperty); }
            set { SetValue(IsEnabledLastTimeProperty, value); }
        }
        public static readonly PropertyData IsEnabledLastTimeProperty = RegisterProperty("IsEnabledLastTime", typeof(bool), false);

        public string StartTime
        {
            get { return GetValue<string>(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }
        public static readonly PropertyData StartTimeProperty = RegisterProperty("StartTime", typeof(string));

        public string DateText
        {
            get { return GetValue<string>(DateTextProperty); }
            set { SetValue(DateTextProperty, value); }
        }
        public static readonly PropertyData DateTextProperty = RegisterProperty("DateText", typeof(string));

        public string MinimumDate
        {
            get { return GetValue<string>(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public static readonly PropertyData MinimumDateProperty = RegisterProperty("MinimumDate", typeof (string), $"{DateTime.Today.Month}.{DateTime.Today.Day}.{DateTime.Today.Year}");

        public Command TimeValuChangedCommand { get; private set; }
        private void OnTimeValuChangedCommandExecute()
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(200); // ожидание окончания события и непосредственного изменения свойства

                int currentFirstTime = DateTime.Parse(FirstTime).Hour;
                
                StartTime = LastTime = $"{(currentFirstTime + 1)}:00";
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
                    MessageBox.Show("Today");
                }

                else MessageBox.Show("{Xyeday");
            });
            thread.Start();
        }

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
            _rootViewModel.ChangePage(new MenuViewModel(this));
        }

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
