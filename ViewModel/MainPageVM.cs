using ExpenseTracker.Controls;
using ExpenseTracker.Views;

namespace ExpenseTracker.ViewModel
{
    public class MainPageVM : BaseViewModel
    {
		private double? progress = 0; 
		private double budget = 0;
		private string totalBalance; 
		private string available;
		private string spent;
		private string currentBudget;
		
		public double Budget { get => budget; set { SetProperty(ref budget, value); } }
		public double? Progress { get => progress; set { SetProperty(ref progress, value); } }
		public string TotalBalance { get => totalBalance; set { SetProperty(ref totalBalance, value); } } 
		public string Available { get => available; set { SetProperty(ref available, value); } }
		public string Spent { get => spent; set { SetProperty(ref spent, value); } }
		public string CurrentBudget { get => currentBudget; set { SetProperty(ref currentBudget, value); } }

		public System.Windows.Input.ICommand OnButtomTab { get; protected set; }

		public MainPageVM() 
		{
			RunCommands();
			CalculateBudget();
		}

		private void RunCommands()
		{
			OnButtomTab = new Command<string>((string par) =>
			{ 
				switch (par)
				{ 
					case "home":
						Application.Current.MainPage = new NavigationPage(new MainPage());
						break;
					case "add":
						Application.Current.MainPage = new NavigationPage(new AddPage());
						break;
					case "history":
						Application.Current.MainPage = new NavigationPage(new HistoryPage());
						break;
					case "reminders":
						Application.Current.MainPage = new NavigationPage(new RemindersPage());
						break;
					case "edit":
						EditBudget();
						break;
				}
			}); 
		}

		private void CalculateBudget()
		{
			string curr = "£"; double? income = 0; double? expense = 0;
			var histories = AllModels.GetHistoryList();
			if(histories!=null && histories.Count>0)
			{
				if (histories.Where(p => p.isIncome == true).FirstOrDefault() != null)
					income = (double?)histories.Where(p => p.isIncome == true).Sum(p => p.amount);
				if (histories.Where(p => p.isIncome == false).FirstOrDefault() != null)
					expense = (double?)histories.Where(p => p.isIncome == false).Sum(p => p.amount);
			} 
			Progress = (expense / income) * 100; TotalBalance = curr + income?.ToString("N2");
			Available = curr + (income - expense)?.ToString("N2"); Spent = curr + expense?.ToString("N2");
			CurrentBudget = Preferences.Get(AllModels.CURRENT_BUDGET_DB_ID, "0") + "%";
			double.TryParse(CurrentBudget.Replace("%", ""), out double budget); Budget = budget;
		}

		private async void EditBudget()
		{
			string? budgetEntered = await Application.Current.MainPage.DisplayPromptAsync("Current Budget", "Enter Current Budget in Percentage", "OK", "Cancel", "20", 3, Keyboard.Numeric, Budget.ToString());
			if(!string.IsNullOrEmpty(budgetEntered))
			{
				if (double.TryParse(budgetEntered.Trim(), out double percent))
				{
					Preferences.Set(AllModels.CURRENT_BUDGET_DB_ID, percent);
					CalculateBudget();
				}
			}
		}
	}
}
