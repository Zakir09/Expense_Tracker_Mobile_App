using ExpenseTracker.ViewModel;

namespace ExpenseTracker.Views;

public partial class HistoryPage : ContentPage
{
	HistoryPageVM vM;
	public HistoryPage()
	{
		InitializeComponent();
		vM = new HistoryPageVM();
		BindingContext = vM;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		vM.GetHistory();
	}
}