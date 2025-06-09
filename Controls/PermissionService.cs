using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Controls
{
	public class PermissionService
	{ 
		public static async Task<bool> RequestCameraPermissionAsync()
		{
			var status = await Permissions.RequestAsync<Permissions.Camera>();
			return status == PermissionStatus.Granted;
		}

		public static async Task<bool> RequestStoragePermissionAsync()
		{
			var status = await Permissions.RequestAsync<Permissions.StorageRead>();
			var statusWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
			return status == PermissionStatus.Granted && statusWrite == PermissionStatus.Granted;
		}
		  
		public static async Task<bool> RequestNetworkPermissionAsync()
		{
			var status = await Permissions.RequestAsync<Permissions.NetworkState>();
			return status == PermissionStatus.Granted;
		}
		 
	}
}
