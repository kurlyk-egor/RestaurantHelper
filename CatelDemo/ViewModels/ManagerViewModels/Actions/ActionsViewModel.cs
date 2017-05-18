using System.Collections.Generic;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.ViewModels.ManagerViewModels.Actions
{
	public class ActionsViewModel : ViewModelBase
	{
		private readonly List<IViewModel> _actionPages = new List<IViewModel> { new DiscountViewModel(), new AmountExcessViewModel(), new DeleteActionsViewModel()};

		public ActionsViewModel()
		{
			SelectionChangedCommand = new Command(OnSelectionChangedCommandExecute);
			AddActionCommand = new Command(OnAddActionCommandExecute);
			DeleteActionCommand = new Command(OnDeleteActionCommandExecute);
		}

		public FastObservableCollection <ActionType> ActionTypes
		{
			get { return GetValue<FastObservableCollection<ActionType>>(ActionTypesProperty); }
			set { SetValue(ActionTypesProperty, value); }
		}
		public static readonly PropertyData ActionTypesProperty = RegisterProperty("ActionTypes", typeof(FastObservableCollection<ActionType>),
			new FastObservableCollection<ActionType> { ActionType.Discount, ActionType.AmountExcess } );

		public  ActionType SelectedActionType
		{
			get { return GetValue<ActionType>(SelectedActionTypeProperty); }
			set { SetValue(SelectedActionTypeProperty, value); }
		}
		public static readonly PropertyData SelectedActionTypeProperty = RegisterProperty("SelectedActionType", typeof(ActionType),
			 ActionType.Discount);

		public IViewModel ActionsPage
		{
			get { return GetValue<IViewModel>(ActionsPageProperty); }
			set { SetValue(ActionsPageProperty, value); }
		}
		public static readonly PropertyData ActionsPageProperty = RegisterProperty("ActionsPage", typeof(IViewModel),
			new DiscountViewModel());


		public Command SelectionChangedCommand { get; private set; }
		private void OnSelectionChangedCommandExecute()
		{
			switch (SelectedActionType)
			{
				case ActionType.AmountExcess:
					SetAddAmountExcessProperties();
					break;
				case ActionType.Discount:
					SetAddDiscountProperties();
					break;
			}
		}


		public Command AddActionCommand { get; private set; }
		private void OnAddActionCommandExecute()
		{
			SetAddDiscountProperties();
		}


		public Command DeleteActionCommand { get; private set; }
		private void OnDeleteActionCommandExecute()
		{
			SetDeleteActionProperties();
		}

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		private void SetDeleteActionProperties()
		{
			ActionsPage = _actionPages.Find(vm => vm is DeleteActionsViewModel);
			SelectedActionType = 0;
		}

		private void SetAddDiscountProperties()
		{
			ActionsPage = _actionPages.Find(vm => vm is DiscountViewModel);
			SelectedActionType = ActionType.Discount;
		}

		private void SetAddAmountExcessProperties()
		{
			ActionsPage = _actionPages.Find(vm => vm is AmountExcessViewModel);
			SelectedActionType = ActionType.AmountExcess;
		}
	}
}
