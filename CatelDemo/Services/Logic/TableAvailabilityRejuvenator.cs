using System.Linq;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;

namespace RestaurantHelper.Services.Logic
{
	/// <summary>
	/// класс пересчитывает доступные столики
	/// </summary>
	class TablesAvailabilityRejuvenator
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly FastObservableCollection<Table> _myTables;

		public TablesAvailabilityRejuvenator(FastObservableCollection<Table> myTables)
		{
			_myTables = myTables;
			 ReFillTables();
		}

		public FastObservableCollection<Table> FillAllTables()
		{
			return _myTables;
		}


		public void RefreshTablesCollection()
		{
			var checker = new TablesAvailabilityChecker(_myTables.ToList());
			var newTablesList = checker.GetAvailableNowTables();

			foreach (var newTable in newTablesList)
			{
				int id = newTable.Id;
				_myTables.Single(t => t.Id == id).Availability = newTable.Availability;
			}
		}

		public int GetFreeTablesCount()
		{
			return _myTables.Count(t => t.Availability);
		}

		public int GetBusyTablesCount()
		{
			return _myTables.Count(t => !t.Availability);
		}

		private void ReFillTables()
		{
			_myTables.Clear();
			_myTables.AddItems(_unitOfWork.Tables.GetAll());
		}
	}
}
