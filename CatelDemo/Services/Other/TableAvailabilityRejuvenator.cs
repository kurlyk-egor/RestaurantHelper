using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;
using Xceed.Wpf.Toolkit;

namespace RestaurantHelper.Services.Other
{
	/// <summary>
	/// класс пересчитывает доступные столики
	/// </summary>
	class TablesAvailabilityRejuvenator
	{
		private readonly IRepository<Table> _tableRepository;
		private readonly ObservableCollection<Table> _myTables;

		public TablesAvailabilityRejuvenator(ObservableCollection<Table> myTables)
		{
			_tableRepository = new Repository<Table>();
			_myTables = myTables;

			 ReFillTables();
		}

		public ObservableCollection<Table> FillAllTables()
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
			_tableRepository.RefreshRepository();
			((ICollection<Table>)_myTables).AddRange(_tableRepository.GetCollection());
		}
	}
}
