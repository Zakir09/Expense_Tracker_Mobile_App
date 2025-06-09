using Microsoft.Maui.Handlers;

namespace ExpenseTracker.Views;

public partial class AddPage : ContentPage
{
	public AddPage(Controls.HistoryModel notModel = null)
	{
		InitializeComponent();
		BindingContext = new ViewModel.AddPageVM(notModel);
	} 

}