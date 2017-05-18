using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Services.Database;
using RestaurantHelper.Services.Interfaces;

namespace RestaurantHelper.Services.Other
{
	public class ActionsFilter
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
					if (discAction.DiscountedDishId == discountAction.DiscountedDishId)
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

		private void FillActionsList()
		{
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
