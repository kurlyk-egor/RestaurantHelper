using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using Catel.Collections;
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
				if (user != null) // иначе - отзыв уже создан и привязан к юзеру
				{
					review = new ClientReview {UserId = user.Id};
				}

				OneButtonMode = false;
				TwoButtonMode = true;
				_addOrEdit = AddReview;
			}
			else
			{
				OneButtonMode = true;
				TwoButtonMode = false;
				_addOrEdit = EditReview;
				// формальность, элемент будет скрыт. это чтобы была доступна кнопка сохранения
				SelectedOrder = review.Order;
			}
			ClientReview = review;
			GetMyOrders(); // наполнить комбобокс

			OkCommand = new Command(OnOkCommandExecute, OnOkCommandCanExecute);
		}


		public bool OneButtonMode
		{
			get { return GetValue<bool>(OneButtonModeProperty); }
			set { SetValue(OneButtonModeProperty, value); }
		}

		public static readonly PropertyData OneButtonModeProperty = RegisterProperty("OneButtonMode", typeof(bool));
		public bool TwoButtonMode
		{
			get { return GetValue<bool>(TwoButtonModeProperty); }
			set { SetValue(TwoButtonModeProperty, value); }
		}
		public static readonly PropertyData TwoButtonModeProperty = RegisterProperty("TwoButtonMode", typeof(bool));



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


		public FastObservableCollection<Order> MyOrders
		{
			get { return GetValue<FastObservableCollection<Order>>(MyOrdersProperty); }
			set { SetValue(MyOrdersProperty, value); }
		}
		public static readonly PropertyData MyOrdersProperty = RegisterProperty("MyOrders", typeof(FastObservableCollection<Order>),
			new FastObservableCollection<Order>());

		public Order SelectedOrder
		{
			get { return GetValue<Order>(SelectedOrderProperty); }
			set { SetValue(SelectedOrderProperty, value); }
		}
		public static readonly PropertyData SelectedOrderProperty = RegisterProperty("SelectedOrder", typeof(Order));


		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}

		protected override async Task CloseAsync()
		{
			await base.CloseAsync();
		}


		public Command OkCommand { get; private set; }

		private bool OnOkCommandCanExecute()
		{
			return SelectedOrder != null;
		}
		private async void OnOkCommandExecute()
		{
			ClientReview.DateTime = DateTime.Now;
			ClientReview.Text = Text;
			ClientReview.OrderId = SelectedOrder.Id;

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

		private void GetMyOrders()
		{
			// получаем все отзывы
			var reviews = _unitOfWork.ClientReviews.GetAll().ToList();

			// получаем заказы. далее для отображения ищем заказы определенного пользователя
			var orders = _unitOfWork.Orders.GetAll().ToList().Where(o => o.UserId == ClientReview.UserId)
				//такие, на которые еще нет отзывов
				.Where(o => !reviews.Exists(r => r.OrderId == o.Id));
			MyOrders.Clear();
			MyOrders.AddItems(orders);
		}
	}
}
