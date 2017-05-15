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
using RestaurantHelper.Services.Other;
using Xceed.Wpf.Toolkit;

namespace RestaurantHelper.ViewModels.ManagerViewModels
{
	public class ManagerHallViewModel : ViewModelBase
	{
		private readonly TableMoveHelper _tableMoveHelper;
		public ManagerHallViewModel()
		{
			_tableMoveHelper = new TableMoveHelper(Tables);
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

		public int MoveCounter	
		{
			get { return GetValue<int>(MoveCounterProperty); }
			set { SetValue(MoveCounterProperty, value); }
		}
		public static readonly PropertyData MoveCounterProperty = RegisterProperty("MoveCounter", typeof(int), 1);

		public Command MoveTableUpCommand { get; private set; }
		public Command MoveTableDownCommand { get; private set; }
		public Command MoveTableLeftCommand { get; private set; }
		public Command MoveTableRightCommand { get; private set; }
		public Command SaveTablesPositionsCommand { get; private set; }
		private void OnSaveTablesPositionsCommandExecute()
		{
			_tableMoveHelper.SaveCurrentTables();
			MessageBox.Show("Сохранено!");
		}

		private void OnMoveTableUpCommandExecute()
		{
			_tableMoveHelper.MoveTableUp(SelectedItemTable, MoveCounter);
		}

		private void OnMoveTableDownCommandExecute()
		{
			_tableMoveHelper.MoveTableDown(SelectedItemTable, MoveCounter);
		}

		private void OnMoveTableLeftCommandExecute()
		{
			_tableMoveHelper.MoveTableLeft(SelectedItemTable, MoveCounter);
		}

		private void OnMoveTableRightCommandExecute()
		{
			_tableMoveHelper.MoveTableRight(SelectedItemTable, MoveCounter);
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
			Tables = _tableMoveHelper.FillAllTables();
		}
	}
}
