using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Collections;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
	public class ActionsHelper
	{
		private readonly IRepository<DiscountAction> _discountRepository = new Repository<DiscountAction>();
		private readonly IRepository<AmountExcessAction> _amountExcessRepository = new Repository<AmountExcessAction>();
		private readonly List<Action> _actions = new List<Action>(); 

		public bool CanAddAction(Action action, out string message)
		{
			message = "";

			FillActionsList();

			foreach (var currentAction in _actions)
			{
				if (currentAction.Name == action.Name)
				{
					message = "Акция с таким названием уже существует";
				}
			}

			var discAction = action as DiscountAction;
			if (discAction != null)
			{
				foreach (var discountAction in _discountRepository.GetCollection())
				{
					if (discAction.DishId == discountAction.DishId)
					{
						message = "На этот товар уже установлена скидка";
					}
				}
			}

			var amntAction = action as AmountExcessAction;
			if (amntAction != null)
			{
				foreach (var amountAction in _amountExcessRepository.GetCollection())
				{
					if (amntAction.ExcessSum == amountAction.ExcessSum)
					{
						message = "За превышение данной суммы уже предусмотрен бонус";
					}
				}
			}


			return string.IsNullOrEmpty(message);
		}

		public void SaveAction(Action action)
		{
			var ditem = action as DiscountAction;
			if (ditem != null)
			{
				_discountRepository.Insert(ditem);
				_discountRepository.SaveChanges();
			}
			var aitem = action as AmountExcessAction;
			if (aitem != null)
			{
				_amountExcessRepository.Insert(aitem);
				_amountExcessRepository.SaveChanges();
			}
		}

		public void RemoveAction(Action action)
		{
			if (action is AmountExcessAction)
			{
				_amountExcessRepository.Delete((AmountExcessAction)action);
				_amountExcessRepository.SaveChanges();
			}
			if (action is DiscountAction)
			{
				_discountRepository.Delete((DiscountAction)action);
				_discountRepository.SaveChanges();
			}

		}

		public ObservableCollection<Action> GetActions()
		{
			FillActionsList();
			ObservableCollection < Action > actions = new ObservableCollection<Action>();
			((ICollection<Action>)actions).AddRange(_actions);
			return actions;
		} 


		private void FillActionsList()
		{
			_actions.Clear();

			foreach (var discountAction in _discountRepository.GetCollection())
			{
				_actions.Add(discountAction);
			}

			foreach (var amountExcessAction in _amountExcessRepository.GetCollection())
			{
				_actions.Add(amountExcessAction);
			}
		}
	}
}
