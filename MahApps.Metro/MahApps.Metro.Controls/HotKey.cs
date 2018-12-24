using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.Text;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class HotKey : IEquatable<HotKey>
	{
		private readonly Key _key;

		private readonly ModifierKeys _modifierKeys;

		public Key Key => _key;

		public ModifierKeys ModifierKeys => _modifierKeys;

		public HotKey(Key key, ModifierKeys modifierKeys = ModifierKeys.None)
		{
			_key = key;
			_modifierKeys = modifierKeys;
		}

		public override bool Equals(object obj)
		{
			if (obj is HotKey)
			{
				return Equals((HotKey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((int)_key * 397) ^ (int)_modifierKeys;
		}

		public bool Equals(HotKey other)
		{
			if (_key == other._key)
			{
				return _modifierKeys == other._modifierKeys;
			}
			return false;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((_modifierKeys & ModifierKeys.Alt) == ModifierKeys.Alt)
			{
				stringBuilder.Append(GetLocalizedKeyStringUnsafe(18));
				stringBuilder.Append("+");
			}
			if ((_modifierKeys & ModifierKeys.Control) == ModifierKeys.Control)
			{
				stringBuilder.Append(GetLocalizedKeyStringUnsafe(17));
				stringBuilder.Append("+");
			}
			if ((_modifierKeys & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				stringBuilder.Append(GetLocalizedKeyStringUnsafe(16));
				stringBuilder.Append("+");
			}
			if ((_modifierKeys & ModifierKeys.Windows) == ModifierKeys.Windows)
			{
				stringBuilder.Append("Windows+");
			}
			stringBuilder.Append(GetLocalizedKeyString(_key));
			return stringBuilder.ToString();
		}

		private static string GetLocalizedKeyString(Key key)
		{
			if (key >= Key.BrowserBack && key <= Key.LaunchApplication2)
			{
				return key.ToString();
			}
			return GetLocalizedKeyStringUnsafe(KeyInterop.VirtualKeyFromKey(key)) ?? key.ToString();
		}

		private static string GetLocalizedKeyStringUnsafe(int key)
		{
			long num = key & 0xFFFF;
			StringBuilder stringBuilder = new StringBuilder(256);
			long num2 = NativeMethods.MapVirtualKey((uint)num, NativeMethods.MapType.MAPVK_VK_TO_VSC);
			num2 <<= 16;
			if (num == 45 || num == 46 || num == 144 || (33 <= num && num <= 40))
			{
				num2 |= 0x1000000;
			}
			if (UnsafeNativeMethods.GetKeyNameText((int)num2, stringBuilder, 256) <= 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}
	}
}
