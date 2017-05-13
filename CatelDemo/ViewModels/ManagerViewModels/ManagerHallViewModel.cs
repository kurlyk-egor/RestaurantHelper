using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerHallViewModel : ViewModelBase
	{
		readonly TableRepository _tableRepository;
		public ManagerHallViewModel()
		{
			_tableRepository  = TableRepository.GetRepositoryInstance();
			AddAllTablesToObservableCollection();
			MoveTableUpCommand = new Command(OnMoveTableUpCommandExecute);
			MoveTableDownCommand = new Command(OnMoveTableDownCommandExecute);
			MoveTableLeftCommand = new Command(OnMoveTableLeftCommandExecute);
			MoveTableRightCommand = new Command(OnMoveTableRightCommandExecute);
			SaveTablesPositionsCommand = new Command(OnSaveTablesPositionsCommandExecute);
		}

		public ObservableCollection<Table> Tables
		{
			get { return GetValue<ObservableCollection<Table> >(TablesProperty); }
			set { SetValue(TablesProperty, value); }
		}
		public static readonly PropertyData TablesProperty = RegisterProperty("Tables", typeof(ObservableCollection<Table> ), new ObservableCollection<Table>());

		public Table SelectedItemTable
		{
			get { return GetValue<Table>(SelectedItemTableProperty); }
			set { SetValue(SelectedItemTableProperty, value); }
		}
		public static readonly PropertyData SelectedItemTableProperty = RegisterProperty("SelectedItemTable", typeof(Table));

		public Command MoveTableUpCommand { get; private set; }
		public Command MoveTableDownCommand { get; private set; }
		public Command MoveTableLeftCommand { get; private set; }
		public Command MoveTableRightCommand { get; private set; }

		public Command SaveTablesPositionsCommand { get; private set; }
		private void OnSaveTablesPositionsCommandExecute()
		{
			foreach (var table in Tables)
			{
				_tableRepository.GetItem(table.Id).Top =
					Tables.ToList().Find(t => t.Id == table.Id).Top;
				_tableRepository.GetItem(table.Id).Left =
					Tables.ToList().Find(t => t.Id == table.Id).Left;
			}
		}

		private void OnMoveTableUpCommandExecute()
		{
			SelectedItemTable.Top -= 10;
		}

		private void OnMoveTableDownCommandExecute()
		{
			SelectedItemTable.Top += 10;
		}

		private void OnMoveTableLeftCommandExecute()
		{
			SelectedItemTable.Left -= 10;
		}

		private void OnMoveTableRightCommandExecute()
		{
			SelectedItemTable.Left += 10;
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
			((ICollection<Table>)Tables).AddRange(_tableRepository.GetCollection());
		}
	}
}
