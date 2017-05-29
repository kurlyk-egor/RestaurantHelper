using System;
using System.Linq;
using RestaurantHelper.DAL;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.Services.Logic
{
	class ReviewsWithAnswersBinder
	{
		private const string NO_ANSWER = "Вы еще не ответили на этот отзыв.";
		private const string NO_SELECT = "Ничего не выбрано.";

		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();

		public bool CanPerformCommands(ClientReview review, bool isMyReviews, out string toolTipMessage)
		{
			if (review == null)
			{
				toolTipMessage = "Не выбран отзыв";
				return false;
			}
			if (!isMyReviews)
			{
				toolTipMessage = "Удалять/редактировать отзывы можно только во вкладке 'МОИ ОТЗЫВЫ'";
				return false;
			}
			if (IsExistAnswer(review))
			{
				toolTipMessage = "Администратор уже ответил на этот отзыв, его нельзя изменить";
				return false;
			}
			toolTipMessage = "Изменить/удалить выбранный отзыв";
			return true;
		}

		public string GetManagerMessage(ClientReview review)
		{
			// не выбрано
			if (review == null)
			{
				return NO_SELECT;
			}

			var answer = _unitOfWork.ManagerAnswers.GetAll()?.ToList()
				.Find(a => a.ReviewId == review.Id);

			var answerString = answer == null ? NO_ANSWER : answer.Text;
			return answerString;
		}

		public string GetClientMessage(ClientReview review)
		{
			string answerString = GetManagerMessage(review);

			if (answerString == NO_ANSWER)
			{
				return "Администратор еще не ответил.";
			}
			return answerString;
		}

		public void SaveAnswer(string adminAnswer, int reviewId)
		{
			// исключить несколько ответов на один и тот же отзыв
			var answer = _unitOfWork.ManagerAnswers.GetAll()?.ToList()
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

		private bool IsExistAnswer(ClientReview review)
		{
			return _unitOfWork.ManagerAnswers.GetAll().ToList()
				.Exists(a => a.ReviewId == review.Id);
		}
	}
}
