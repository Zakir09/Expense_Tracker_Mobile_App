namespace ExpenseTracker.Views;

public partial class RemindersPage : ContentPage
{
	public RemindersPage()
	{
		InitializeComponent();
		BindingContext = new ViewModel.RemindersPageVM();
	}

	private void SwipeItem_Invoked(object sender, EventArgs e)
	{

	}
}