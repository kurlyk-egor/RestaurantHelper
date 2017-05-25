using System.Collections.ObjectModel;
using Catel.Collections;
using Catel.Data;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Other;

namespace RestaurantHelper.ViewModels.ManagerViewModels.Actions
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class DeleteActionsViewModel : ViewModelBase
	{
		private readonly ActionsHelper _actionsHelper;
		public DeleteActionsViewModel()
		{
			_actionsHelper = new ActionsHelper();

			Actions.Clear();
			Actions = _actionsHelper.GetActions();

			DeleteActionCommand = new Command(OnDeleteActionCommandExecute, OnDeleteActionCommandCanExecute);
		}
		
		public FastObservableCollection<Action> Actions
		{
			get { return GetValue<FastObservableCollection<Action>>(ActionsProperty); }
			set { SetValue(ActionsProperty, value); }
		}
		public static readonly PropertyData ActionsProperty = RegisterProperty("Actions", typeof(FastObservableCollection<Action>), 
			new FastObservableCollection<Action>());


		public Action SelectedAction
		{
			get { return GetValue<Action>(SelectedActionProperty); }
			set { SetValue(SelectedActionProperty, value); }
		}
		public static readonly PropertyData SelectedActionProperty = RegisterProperty("SelectedAction", typeof(Action));


		public Command DeleteActionCommand { get; private set; }
		private bool OnDeleteActionCommandCanExecute()
		{
			return SelectedAction != null;
		}
		private void OnDeleteActionCommandExecute()
		{
			_actionsHelper.RemoveAction(SelectedAction);
			Actions.Clear();
			Actions = _actionsHelper.GetActions();
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
