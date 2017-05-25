using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.Services.Other
{
	class ReviewsWithAnswersBinder
	{
		private const string NO_ANSWER = "Ответа еще нет.";
		private const string NO_SELECT = "Ничего не выбрано.";

		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public string GetManagerMessage(ClientReview review)
		{
			string answerString;
			var answer = _unitOfWork.ManagerAnswers.GetAll().ToList()
				.Find(a => a.ReviewId == review.Id);

			// не выбрано
			if (review == null) 
			{
				answerString = NO_SELECT;
			}
			// нет ответа
			else if (answer == null)
			{
				answerString = NO_ANSWER;
			}
			// есть ответ
			else
			{
				answerString = answer.Text;
			}
			return answerString;
		}

		public string GetClientMessage()
		{
			return "";
		}

		public void SaveAnswer(string adminAnswer, int reviewId)
		{
			// исключить несколько ответов на один и тот же отзыв
			var answer = _unitOfWork.ManagerAnswers.GetAll().ToList()
				.Find(a => a.ReviewId == reviewId);

			if (answer != null) // существует - обновить
			{
				answer.Text = adminAnswer;
				answer.DateTime = DateTime.Now;
				_unitOfWork.ManagerAnswers.Update(answer);
			}
			else            // не существует - добавить
			{
				_unitOfWork.ManagerAnswers.Insert(new ManagerAnswer
				{
					Text = adminAnswer,
					DateTime = DateTime.Now,
					ReviewId = reviewId
				});
			}
			_unitOfWork.SaveChanges();
		}
	}
}
