using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MahApps.Metro.Controls.Dialogs
{
	public class LoginDialogData
	{
		public string Username
		{
			get;
			internal set;
		}

		public string Password
		{
			[SecurityCritical]
			get
			{
				IntPtr intPtr = Marshal.SecureStringToBSTR(SecurePassword);
				try
				{
					return Marshal.PtrToStringBSTR(intPtr);
				}
				finally
				{
					Marshal.ZeroFreeBSTR(intPtr);
				}
			}
		}

		public SecureString SecurePassword
		{
			get;
			internal set;
		}

		public bool ShouldRemember
		{
			get;
			internal set;
		}
	}
}
