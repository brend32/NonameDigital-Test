using System.Security.Cryptography;
using System.Text;

namespace Utils
{
	public static class HashUtils
	{
		public static byte[] BytesMD5(this string msg)
		{
			var mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			return mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(msg));
		}

		public static byte[] BytesSHA256(this string msg)
		{
			var mD5CryptoServiceProvider = new SHA256CryptoServiceProvider();
			return mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(msg));
		}

		public static string MD5(this string msg)
		{
			return msg.BytesMD5().StringFromBytes();
		}

		public static string SHA256(this string msg)
		{
			return msg.BytesSHA256().StringFromBytes();
		}
		
		public static string StringFromBytes(this byte[] bytes)
		{
			var stringBuilder = new StringBuilder();
			for (var i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append(bytes[i].ToString("X2"));
			}
			return stringBuilder.ToString().ToLower();
		}
	}
}