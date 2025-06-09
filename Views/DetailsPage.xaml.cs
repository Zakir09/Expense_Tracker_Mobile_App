namespace ExpenseTracker.Views;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(Controls.HistoryModel historyModel)
	{
		InitializeComponent();
		BindingContext = new ViewModel.DetailsPageVM(historyModel);
	}
}