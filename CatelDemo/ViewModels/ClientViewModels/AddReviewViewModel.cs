using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Catel.Data;
using RestaurantHelper.DAL;
using RestaurantHelper.Models;
using RestaurantHelper.Models.Actions;
using RestaurantHelper.Models.Reviews;

namespace RestaurantHelper.ViewModels.ClientViewModels
{
	using Catel.MVVM;
	using System.Threading.Tasks;

	public class AddReviewViewModel : ViewModelBase
	{
		private readonly UnitOfWork _unitOfWork = UnitOfWork.GetInstance();
		private readonly Action<ClientReview> _addOrEdit;
		public AddReviewViewModel(ClientReview review = null, User user = null)
		{
			if (review == null)
			{
				if (user != null)
				{
					review = new ClientReview {UserId = user.Id};
				}
				_addOrEdit = AddReview;
			}
			else
			{
				_addOrEdit = EditReview;
			}
			ClientReview = review;

			OkCommand = new Command(OnOkCommandExecute);
		}


		[Model]
		public ClientReview ClientReview
		{
			get { return GetValue<ClientReview>(ClientReviewProperty); }
			private set { SetValue(ClientReviewProperty, value); }
		}
		public static readonly PropertyData ClientReviewProperty = RegisterProperty("ClientReview", typeof(ClientReview));

		[ViewModelToModel("ClientReview")]
		public string Text
		{
			get { return GetValue<string>(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public static readonly PropertyData TextProperty = RegisterProperty("Text", typeof(string));
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}

		public Command OkCommand { get; private set; }
		private async void OnOkCommandExecute()
		{
			ClientReview.DateTime = DateTime.Now;
			ClientReview.Text = Text;
			_addOrEdit(ClientReview);
			await CloseViewModelAsync(true);
		}

		private void AddReview(ClientReview clientReview)
		{
			_unitOfWork.ClientReviews.Insert(clientReview);
			_unitOfWork.SaveChanges();
		}

		private void EditReview(ClientReview clientReview)
		{
			_unitOfWork.ClientReviews.Update(clientReview);
			_unitOfWork.SaveChanges();
		}
	}
}
