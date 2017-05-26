using System.Threading.Tasks;
using Catel.Data;
using Catel.MVVM;

namespace RestaurantHelper.ViewModels
{
    public class ShortMessageViewModel : ViewModelBase
    {
        public ShortMessageViewModel(string text)
        {
	        Text = text;
        }

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
    }
}
