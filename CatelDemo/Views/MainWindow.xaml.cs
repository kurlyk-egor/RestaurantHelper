using System.Windows.Input;

namespace RestaurantHelper.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

	    private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	    {
		    DragMove();
	    }
    }
}
