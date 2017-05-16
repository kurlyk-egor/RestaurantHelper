using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.Models;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;
using Xceed.Wpf.Toolkit;

namespace RestaurantHelper.Services.Other
{
	class TableMoveHelper
	{
		private const int MAX_TABLE_TOP = 335;
		private const int MAX_TABLE_LEFT = 500;
		private const int MIN_TABLE_TOP = 10;
		private const int MIN_TABLE_LEFT = 10;

		private readonly IRepositoryBase<Table> _tableRepository;
		private readonly ObservableCollection<Table> _myTables; 

		public TableMoveHelper(ObservableCollection<Table> myTables)
		{
			_tableRepository = new RepositoryBase<Table>();
			_myTables = myTables;
		}

		public ObservableCollection<Table> FillAllTables()
		{
			((ICollection<Table>)_myTables).AddRange(_tableRepository.GetCollection());
			return _myTables;
		}

		public void SaveCurrentTables()
		{
			foreach (var table in _myTables)
			{
				_tableRepository.GetItem(table.Id).Top =
					_myTables.ToList().Find(t => t.Id == table.Id).Top;
				_tableRepository.GetItem(table.Id).Left =
					_myTables.ToList().Find(t => t.Id == table.Id).Left;
			}

			_tableRepository.SaveChanges();
		}

		public void MoveTableUp(Table table, int units)
		{
			if (table.Top - units < MIN_TABLE_TOP)
			{
				table.Top = MIN_TABLE_TOP;
				return;
			}
			table.Top -= units;
		}

		public void MoveTableDown(Table table, int units)
		{
			if (table.Top + units > MAX_TABLE_TOP)
			{
				table.Top = MAX_TABLE_TOP;
				return;
			}
			table.Top += units;
		}

		public void MoveTableLeft(Table table, int units)
		{
			if (table.Left - units < MIN_TABLE_LEFT)
			{
				table.Left = MIN_TABLE_LEFT;
				return;
			}
			table.Left -= units;
		}

		public void MoveTableRight(Table table, int units)
		{
			if (table.Left + units > MAX_TABLE_LEFT)
			{
				table.Left = MAX_TABLE_LEFT;
				return;
			}
			table.Left += units;
		}
	}
}
