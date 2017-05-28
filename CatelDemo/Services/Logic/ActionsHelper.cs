using System.Collections.Generic;
using System.Linq;
using Catel.Collections;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
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

			var discount = action as DiscountAction;
			if (discount != null)
			{
				if(_unitOfWork.DiscountActions.GetAll().ToList().Exists(b => b.DishId == discount.DishId))
				{
						message = "На этот товар уже установлена скидка";
				}
			}

			var bonus = action as BonusAction;
			if (bonus != null)
			{
				if(_unitOfWork.BonusActions.GetAll().ToList().Exists(b => b.ExcessSum == bonus.ExcessSum))
				{
					message = "За превышение данной суммы уже предусмотрен бонус";
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
			var bitem = action as BonusAction;
			if (bitem != null)
			{
				_unitOfWork.BonusActions.Insert(bitem);
			}
			_unitOfWork.SaveChanges();
		}

		public void RemoveAction(Action action)
		{
			if (action is BonusAction)
			{
				_unitOfWork.BonusActions.Delete(action.Id);
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

		/// <summary>
		/// метод пересчитывает вычисляемое поле модели
		/// </summary>
		public void CalculateDiscountsExisting(FastObservableCollection<Dish> dishes)
		{
			 dishes.ForEach(dish =>
			 {
				 // устанавливаем логическое значение
				 dish.IsDiscounted = _unitOfWork.DiscountActions.GetAll().ToList().Exists(da => da.DishId == dish.Id);
			 });
		}

		private void FillActionsList()
		{
			_actions.Clear();

			foreach (var discountAction in _unitOfWork.DiscountActions.GetAll())
			{
				_actions.Add(discountAction);
			}

			foreach (var bonusAction in _unitOfWork.BonusActions.GetAll())
			{
				_actions.Add(bonusAction);
			}
			_actions.Sort((a1, a2) => a1.Id - a2.Id);
		}
	}
}
