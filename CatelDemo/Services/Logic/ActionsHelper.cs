using System.Collections.Generic;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Actions;

namespace RestaurantHelper.Services.Logic
{
	public class ActionsHelper
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
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
				foreach (var discountAction in _unitOfWork.DiscountActions.GetAll())
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
				foreach (var amountAction in _unitOfWork.AmountExcessActions.GetAll())
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
				_unitOfWork.DiscountActions.Insert(ditem);
			}
			var aitem = action as AmountExcessAction;
			if (aitem != null)
			{
				_unitOfWork.AmountExcessActions.Insert(aitem);
			}
			_unitOfWork.SaveChanges();
		}

		public void RemoveAction(Action action)
		{
			if (action is AmountExcessAction)
			{
				_unitOfWork.AmountExcessActions.Delete(action.Id);
			}
			if (action is DiscountAction)
			{
				_unitOfWork.DiscountActions.Delete(action.Id);
			}
			_unitOfWork.SaveChanges();
		}

		public FastObservableCollection<Action> GetActions()
		{
			FillActionsList();
			FastObservableCollection <Action> actions = new FastObservableCollection<Action>();
			actions.AddItems(_actions);
			return actions;
		} 


		private void FillActionsList()
		{
			_actions.Clear();

			foreach (var discountAction in _unitOfWork.DiscountActions.GetAll())
			{
				_actions.Add(discountAction);
			}

			foreach (var amountExcessAction in _unitOfWork.AmountExcessActions.GetAll())
			{
				_actions.Add(amountExcessAction);
			}
			_actions.Sort((a1, a2) => a1.Id - a2.Id);
		}
	}
}
