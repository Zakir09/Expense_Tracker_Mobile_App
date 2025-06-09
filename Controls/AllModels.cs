using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ExpenseTracker.Controls
{
	class AllModels
	{
		internal static string CURRENT_BUDGET_DB_ID = "CURRENT_BUDGET_DB_ID";
		internal static string HISTORY_DB_ID = "HISTORY_DB_ID"; 
		internal static string NOTIFICATION_DB_ID = "NOTIFICATION_DB_ID";

		internal static List<HistoryModel>? GetHistoryList()
		{
			List<HistoryModel>? HistoryList = new List<HistoryModel>();
			if(Preferences.Get(HISTORY_DB_ID, "") != "")
			{
				string mraw = Preferences.Get(HISTORY_DB_ID, "");
				HistoryList = JsonConvert.DeserializeObject<List<HistoryModel>>(mraw);
			}
			return HistoryList;
		}

		internal static List<NotificationModel>? GetNotList()
		{ 
			List<NotificationModel>? NotList  = new List<NotificationModel>();
			if (Preferences.Get(NOTIFICATION_DB_ID, "") != "")
			{
				string mraw = Preferences.Get(NOTIFICATION_DB_ID, "");
				NotList = JsonConvert.DeserializeObject<List<NotificationModel>>(mraw);
			}
			return NotList;
		}
	}

	public class NotificationModel
    {
        public string id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public DateTime dateTime { get; set; }
    }

	public class HistoryModel : NotificationModel
	{
		public string AmountString { get
			{
				string symbol = isIncome ? "+" : "-";  
				return symbol + "£" + amount?.ToString("N2");
			} }
		public decimal? amount { get; set; }
		public string imageUrl { get; set; }
		public bool isIncome { get; set; } 
		public ImageSource? PhotoImageSource { get; set; } 
	}
}
