using System;

namespace ControlzEx.Standard
{
	internal static class ShellUtil
	{
		public static string GetPathFromShellItem(IShellItem item)
		{
			return item.GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING);
		}

		public static IShellItem2 GetShellItemForPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			Guid riid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
			object ppv;
			HRESULT hrLeft = NativeMethods.SHCreateItemFromParsingName(path, null, ref riid, out ppv);
			if (hrLeft == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || hrLeft == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
			{
				hrLeft = HRESULT.S_OK;
				ppv = null;
			}
			hrLeft.ThrowIfFailed();
			return (IShellItem2)ppv;
		}
	}
}
