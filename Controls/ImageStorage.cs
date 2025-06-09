using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Controls
{
	public class ImageStorage
	{
		public static async Task<string> SaveImageToLocalStorage(string imageName, byte[] imageData)
		{
			try
			{
				string imagePath = Path.Combine(FileSystem.AppDataDirectory, imageName);
				using (FileStream stream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
				{
					await stream.WriteAsync(imageData, 0, imageData.Length);
				}
				return imagePath;
			}
			catch (System.Exception ex)
			{
				return "";
			}
		}
		internal static void DeleteImageFromLocalStorage(string imageName)
		{
			try
			{
				string imagePath = Path.Combine(FileSystem.AppDataDirectory, imageName);
				if (File.Exists(imagePath)) File.Delete(imagePath);
			}
			catch (System.Exception)
			{

			}
		}
		public static byte[] LoadImageFromLocalStorage(string imageName)
		{
			try
			{
				string imagePath = Path.Combine(FileSystem.AppDataDirectory, imageName);
				if (File.Exists(imagePath))
					return File.ReadAllBytes(imagePath);
			}
			catch (System.Exception)
			{

			}
			return null;
		}
		public static byte[] ConvertStreamToByteArray(Stream stream)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					stream.CopyTo(memoryStream);
					return memoryStream.ToArray();
				}
			}
			catch (System.Exception ex)
			{
				return null;
			}
		}

	}
}
