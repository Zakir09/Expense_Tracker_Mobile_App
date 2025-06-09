
namespace ExpenseTracker.Controls
{
	public interface INotificationScheduler
	{
		void ScheduleNotification(DateTime dateTime, string title, string description);
	}
}
