using System.Collections.Generic;
using System.Windows.Documents;
using Catel.Data;
using CatelDemo.ViewModels.ClientViewModels.OrderViewModels;

namespace CatelDemo.ViewModels.ClientViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    // родительская страница для создания заказа
    public class OrderMainViewModel : ViewModelBase
    {
        private readonly IViewModel _hallViewModel;
        private readonly IViewModel _menuViewModel;
        private readonly IViewModel _totalsViewModel;

        public OrderMainViewModel()
        {
            _hallViewModel = new HallViewModel(this);
            _menuViewModel = new MenuViewModel(this);
            _totalsViewModel = new TotalsViewModel(this);
            SetCurrentHallPage();
        }

        public void SetCurrentHallPage()
        {
            CurrentOrderPage = _hallViewModel;
        }

        public void SetCurrentMenuPage()
        {
            CurrentOrderPage = _menuViewModel;
        }

        public void SetCurrentTotalsPage()
        {
            CurrentOrderPage = _totalsViewModel;
        }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        public IViewModel CurrentOrderPage
        {
            get { return GetValue<IViewModel>(CurrentOrderPageProperty); }
            set { SetValue(CurrentOrderPageProperty, value); }
        }
        public static readonly PropertyData CurrentOrderPageProperty = RegisterProperty("CurrentOrderPage", typeof(IViewModel));

        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
