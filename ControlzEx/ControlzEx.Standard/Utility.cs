using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ControlzEx.Standard
{
	internal static class Utility
	{
		private class _UrlDecoder
		{
			private readonly Encoding _encoding;

			private readonly char[] _charBuffer;

			private readonly byte[] _byteBuffer;

			private int _byteCount;

			private int _charCount;

			public _UrlDecoder(int size, Encoding encoding)
			{
				_encoding = encoding;
				_charBuffer = new char[size];
				_byteBuffer = new byte[size];
			}

			public void AddByte(byte b)
			{
				_byteBuffer[_byteCount++] = b;
			}

			public void AddChar(char ch)
			{
				_FlushBytes();
				_charBuffer[_charCount++] = ch;
			}

			private void _FlushBytes()
			{
				if (_byteCount > 0)
				{
					_charCount += _encoding.GetChars(_byteBuffer, 0, _byteCount, _charBuffer, _charCount);
					_byteCount = 0;
				}
			}

			public string GetString()
			{
				_FlushBytes();
				if (_charCount > 0)
				{
					return new string(_charBuffer, 0, _charCount);
				}
				return "";
			}
		}

		private static readonly Random _randomNumberGenerator = new Random();

		private static readonly Version _osVersion = Environment.OSVersion.Version;

		private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;

		private static int s_bitDepth;

		public static bool IsOSVistaOrNewer => _osVersion >= new Version(6, 0);

		public static bool IsOSWindows7OrNewer => _osVersion >= new Version(6, 1);

		public static bool IsPresentationFrameworkVersionLessThan4 => _presentationFrameworkVersion < new Version(4, 0);

		private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
		{
			int i;
			for (i = 0; i < cb - 8; i += 8)
			{
				long num = Marshal.ReadInt64(left, i);
				long num2 = Marshal.ReadInt64(right, i);
				if (num != num2)
				{
					return false;
				}
			}
			for (; i < cb; i++)
			{
				byte num3 = Marshal.ReadByte(left, i);
				byte b = Marshal.ReadByte(right, i);
				if (num3 != b)
				{
					return false;
				}
			}
			return true;
		}

		public static Exception FailableFunction<T>(Func<T> function, out T result)
		{
			return FailableFunction(5, function, out result);
		}

		public static T FailableFunction<T>(Func<T> function)
		{
			T result;
			Exception ex = FailableFunction(function, out result);
			if (ex != null)
			{
				throw ex;
			}
			return result;
		}

		public static T FailableFunction<T>(int maxRetries, Func<T> function)
		{
			T result;
			Exception ex = FailableFunction(maxRetries, function, out result);
			if (ex != null)
			{
				throw ex;
			}
			return result;
		}

		public static Exception FailableFunction<T>(int maxRetries, Func<T> function, out T result)
		{
			int num = 0;
			while (true)
			{
				try
				{
					result = function();
					return null;
				}
				catch (Exception result2)
				{
					if (num == maxRetries)
					{
						result = default(T);
						return result2;
					}
				}
				num++;
			}
		}

		public static string GetHashString(string value)
		{
			using (MD5 mD = MD5.Create())
			{
				return mD.ComputeHash(Encoding.UTF8.GetBytes(value)).Aggregate(new StringBuilder(), (StringBuilder sb, byte b) => sb.Append(b.ToString("x2", CultureInfo.InvariantCulture))).ToString();
			}
		}

		public static Point GetPoint(IntPtr ptr)
		{
			int num = (int)(Environment.Is64BitProcess ? ptr.ToInt64() : ptr.ToInt32());
			short num2 = (short)num;
			short num3 = (short)((uint)num >> 16);
			return new Point((double)num2, (double)num3);
		}

		public static int GET_X_LPARAM(IntPtr lParam)
		{
			return LOWORD(lParam.ToInt32());
		}

		public static int GET_Y_LPARAM(IntPtr lParam)
		{
			return HIWORD(lParam.ToInt32());
		}

		public static int HIWORD(int i)
		{
			return (short)(i >> 16);
		}

		public static int LOWORD(int i)
		{
			return (short)(i & 0xFFFF);
		}

		public static bool AreStreamsEqual(Stream left, Stream right)
		{
			if (left == null)
			{
				return right == null;
			}
			if (right == null)
			{
				return false;
			}
			if (!left.CanRead || !right.CanRead)
			{
				throw new NotSupportedException("The streams can't be read for comparison");
			}
			if (left.Length == right.Length)
			{
				int num = (int)left.Length;
				left.Position = 0L;
				right.Position = 0L;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				byte[] array = new byte[512];
				byte[] array2 = new byte[512];
				GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				IntPtr left2 = gCHandle.AddrOfPinnedObject();
				GCHandle gCHandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
				IntPtr right2 = gCHandle2.AddrOfPinnedObject();
				try
				{
					while (num2 < num)
					{
						num4 = left.Read(array, 0, array.Length);
						num5 = right.Read(array2, 0, array2.Length);
						if (num4 != num5)
						{
							return false;
						}
						if (!_MemCmp(left2, right2, num4))
						{
							return false;
						}
						num2 += num4;
						num3 += num5;
					}
					return true;
				}
				finally
				{
					gCHandle.Free();
					gCHandle2.Free();
				}
			}
			return false;
		}

		public static bool GuidTryParse(string guidString, out Guid guid)
		{
			Verify.IsNeitherNullNorEmpty(guidString, "guidString");
			try
			{
				guid = new Guid(guidString);
				return true;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			guid = default(Guid);
			return false;
		}

		public static bool IsFlagSet(int value, int mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsFlagSet(uint value, uint mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsFlagSet(long value, long mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsFlagSet(ulong value, ulong mask)
		{
			return (value & mask) != 0;
		}

		public static bool IsInterfaceImplemented(Type objectType, Type interfaceType)
		{
			return objectType.GetInterfaces().Any((Type type) => type == interfaceType);
		}

		public static string SafeCopyFile(string sourceFileName, string destFileName, SafeCopyFileOptions options)
		{
			switch (options)
			{
			case SafeCopyFileOptions.PreserveOriginal:
				if (!File.Exists(destFileName))
				{
					File.Copy(sourceFileName, destFileName);
					return destFileName;
				}
				return null;
			case SafeCopyFileOptions.Overwrite:
				File.Copy(sourceFileName, destFileName, overwrite: true);
				return destFileName;
			case SafeCopyFileOptions.FindBetterName:
			{
				string directoryName = Path.GetDirectoryName(destFileName);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(destFileName);
				string extension = Path.GetExtension(destFileName);
				foreach (string item in GenerateFileNames(directoryName, fileNameWithoutExtension, extension))
				{
					if (!File.Exists(item))
					{
						File.Copy(sourceFileName, item);
						return item;
					}
				}
				return null;
			}
			default:
				throw new ArgumentException("Invalid enumeration value", "options");
			}
		}

		public static void SafeDeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				File.Delete(path);
			}
		}

		public static void SafeDispose<T>(ref T disposable) where T : IDisposable
		{
			IDisposable disposable2 = (IDisposable)(object)disposable;
			disposable = default(T);
			disposable2?.Dispose();
		}

		public static void GeneratePropertyString(StringBuilder source, string propertyName, string value)
		{
			if (source.Length != 0)
			{
				source.Append(' ');
			}
			source.Append(propertyName);
			source.Append(": ");
			if (string.IsNullOrEmpty(value))
			{
				source.Append("<null>");
			}
			else
			{
				source.Append('"');
				source.Append(value);
				source.Append('"');
			}
		}

		[Obsolete]
		public static string GenerateToString<T>(T @object) where T : struct
		{
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(", ");
				}
				object value = propertyInfo.GetValue(@object, null);
				string format = (value == null) ? "{0}: <null>" : "{0}: \"{1}\"";
				stringBuilder.AppendFormat(format, propertyInfo.Name, value);
			}
			return stringBuilder.ToString();
		}

		public static void CopyStream(Stream destination, Stream source)
		{
			destination.Position = 0L;
			if (source.CanSeek)
			{
				source.Position = 0L;
				destination.SetLength(source.Length);
			}
			byte[] array = new byte[4096];
			int num;
			do
			{
				num = source.Read(array, 0, array.Length);
				if (num != 0)
				{
					destination.Write(array, 0, num);
				}
			}
			while (array.Length == num);
			destination.Position = 0L;
		}

		public static string HashStreamMD5(Stream stm)
		{
			stm.Position = 0L;
			StringBuilder stringBuilder = new StringBuilder();
			using (MD5 mD = MD5.Create())
			{
				byte[] array = mD.ComputeHash(stm);
				for (int i = 0; i < array.Length; i++)
				{
					byte b = array[i];
					stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
				}
			}
			return stringBuilder.ToString();
		}

		public static void EnsureDirectory(string path)
		{
			if (!path.EndsWith("\\", StringComparison.Ordinal))
			{
				path += "\\";
			}
			path = Path.GetDirectoryName(path);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public static bool MemCmp(byte[] left, byte[] right, int cb)
		{
			GCHandle gCHandle = GCHandle.Alloc(left, GCHandleType.Pinned);
			IntPtr left2 = gCHandle.AddrOfPinnedObject();
			GCHandle gCHandle2 = GCHandle.Alloc(right, GCHandleType.Pinned);
			IntPtr right2 = gCHandle2.AddrOfPinnedObject();
			bool result = _MemCmp(left2, right2, cb);
			gCHandle.Free();
			gCHandle2.Free();
			return result;
		}

		public static string UrlDecode(string url)
		{
			if (url == null)
			{
				return null;
			}
			_UrlDecoder urlDecoder = new _UrlDecoder(url.Length, Encoding.UTF8);
			int length = url.Length;
			for (int i = 0; i < length; i++)
			{
				char c = url[i];
				switch (c)
				{
				case '+':
					urlDecoder.AddByte(32);
					break;
				case '%':
					if (i < length - 2)
					{
						if (url[i + 1] == 'u' && i < length - 5)
						{
							int num = _HexToInt(url[i + 2]);
							int num2 = _HexToInt(url[i + 3]);
							int num3 = _HexToInt(url[i + 4]);
							int num4 = _HexToInt(url[i + 5]);
							if (num >= 0 && num2 >= 0 && num3 >= 0 && num4 >= 0)
							{
								urlDecoder.AddChar((char)((num << 12) | (num2 << 8) | (num3 << 4) | num4));
								i += 5;
								break;
							}
						}
						else
						{
							int num5 = _HexToInt(url[i + 1]);
							int num6 = _HexToInt(url[i + 2]);
							if (num5 >= 0 && num6 >= 0)
							{
								urlDecoder.AddByte((byte)((num5 << 4) | num6));
								i += 2;
								break;
							}
						}
					}
					goto default;
				default:
					if ((c & 0xFF80) == 0)
					{
						urlDecoder.AddByte((byte)c);
					}
					else
					{
						urlDecoder.AddChar(c);
					}
					break;
				}
			}
			return urlDecoder.GetString();
		}

		public static string UrlEncode(string url)
		{
			if (url == null)
			{
				return null;
			}
			byte[] array = Encoding.UTF8.GetBytes(url);
			bool flag = false;
			int num = 0;
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				if (b == 32)
				{
					flag = true;
				}
				else if (!_UrlEncodeIsSafe(b))
				{
					num++;
					flag = true;
				}
			}
			if (flag)
			{
				byte[] array3 = new byte[array.Length + num * 2];
				int num2 = 0;
				array2 = array;
				foreach (byte b2 in array2)
				{
					if (_UrlEncodeIsSafe(b2))
					{
						array3[num2++] = b2;
					}
					else if (b2 == 32)
					{
						array3[num2++] = 43;
					}
					else
					{
						array3[num2++] = 37;
						array3[num2++] = _IntToHex((b2 >> 4) & 0xF);
						array3[num2++] = _IntToHex(b2 & 0xF);
					}
				}
				array = array3;
			}
			return Encoding.ASCII.GetString(array);
		}

		private static bool _UrlEncodeIsSafe(byte b)
		{
			if (!_IsAsciiAlphaNumeric(b))
			{
				switch (b)
				{
				case 33:
				case 39:
				case 40:
				case 41:
				case 42:
				case 45:
				case 46:
				case 95:
					return true;
				default:
					return false;
				}
			}
			return true;
		}

		private static bool _IsAsciiAlphaNumeric(byte b)
		{
			if ((b < 97 || b > 122) && (b < 65 || b > 90))
			{
				if (b >= 48)
				{
					return b <= 57;
				}
				return false;
			}
			return true;
		}

		private static byte _IntToHex(int n)
		{
			if (n <= 9)
			{
				return (byte)(n + 48);
			}
			return (byte)(n - 10 + 65);
		}

		private static int _HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return h - 48;
			}
			if (h >= 'a' && h <= 'f')
			{
				return h - 97 + 10;
			}
			if (h >= 'A' && h <= 'F')
			{
				return h - 65 + 10;
			}
			return -1;
		}

		public static string MakeValidFileName(string invalidPath)
		{
			return invalidPath.Replace('\\', '_').Replace('/', '_').Replace(':', '_')
				.Replace('*', '_')
				.Replace('?', '_')
				.Replace('"', '_')
				.Replace('<', '_')
				.Replace('>', '_')
				.Replace('|', '_');
		}

		public static IEnumerable<string> GenerateFileNames(string directory, string primaryFileName, string extension)
		{
			Verify.IsNeitherNullNorEmpty(directory, "directory");
			Verify.IsNeitherNullNorEmpty(primaryFileName, "primaryFileName");
			primaryFileName = MakeValidFileName(primaryFileName);
			int num;
			for (int i = 0; i <= 50; i = num)
			{
				if (i == 0)
				{
					yield return Path.Combine(directory, primaryFileName) + extension;
				}
				else if (40 >= i)
				{
					yield return Path.Combine(directory, primaryFileName) + " (" + i.ToString((IFormatProvider)null) + ")" + extension;
				}
				else
				{
					yield return Path.Combine(directory, primaryFileName) + " (" + _randomNumberGenerator.Next(41, 9999) + ")" + extension;
				}
				num = i + 1;
			}
		}

		public static bool TryFileMove(string sourceFileName, string destFileName)
		{
			if (!File.Exists(destFileName))
			{
				try
				{
					File.Move(sourceFileName, destFileName);
				}
				catch (IOException)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static void SafeDestroyIcon(ref IntPtr hicon)
		{
			IntPtr intPtr = hicon;
			hicon = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				NativeMethods.DestroyIcon(intPtr);
			}
		}

		public static void SafeDeleteObject(ref IntPtr gdiObject)
		{
			IntPtr intPtr = gdiObject;
			gdiObject = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				NativeMethods.DeleteObject(intPtr);
			}
		}

		public static void SafeDestroyWindow(ref IntPtr hwnd)
		{
			IntPtr hwnd2 = hwnd;
			hwnd = IntPtr.Zero;
			if (NativeMethods.IsWindow(hwnd2))
			{
				NativeMethods.DestroyWindow(hwnd2);
			}
		}

		public static void SafeDisposeImage(ref IntPtr gdipImage)
		{
			IntPtr intPtr = gdipImage;
			gdipImage = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				NativeMethods.GdipDisposeImage(intPtr);
			}
		}

		public static void SafeCoTaskMemFree(ref IntPtr ptr)
		{
			IntPtr intPtr = ptr;
			ptr = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		public static void SafeFreeHGlobal(ref IntPtr hglobal)
		{
			IntPtr intPtr = hglobal;
			hglobal = IntPtr.Zero;
			if (IntPtr.Zero != intPtr)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		public static void SafeRelease<T>(ref T comObject) where T : class
		{
			T val = comObject;
			comObject = null;
			if (val != null)
			{
				Marshal.ReleaseComObject(val);
			}
		}

		public static byte[] GetBytesFromBitmapSource(BitmapSource bmp)
		{
			int pixelWidth = bmp.PixelWidth;
			int pixelHeight = bmp.PixelHeight;
			int num = pixelWidth * ((bmp.Format.BitsPerPixel + 7) / 8);
			byte[] array = new byte[pixelHeight * num];
			bmp.CopyPixels(array, num, 0);
			return array;
		}

		public static BitmapSource GenerateBitmapSource(ImageSource img)
		{
			return GenerateBitmapSource(img, img.Width, img.Height);
		}

		public static BitmapSource GenerateBitmapSource(ImageSource img, double renderWidth, double renderHeight)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawImage(img, new Rect(0.0, 0.0, renderWidth, renderHeight));
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		public static BitmapSource GenerateBitmapSource(UIElement element, double renderWidth, double renderHeight, bool performLayout)
		{
			if (performLayout)
			{
				element.Measure(new Size(renderWidth, renderHeight));
				element.Arrange(new Rect(new Size(renderWidth, renderHeight)));
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96.0, 96.0, PixelFormats.Pbgra32);
			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawRectangle(new VisualBrush(element), null, new Rect(0.0, 0.0, renderWidth, renderHeight));
			}
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		public static void SaveToPng(BitmapSource source, string fileName)
		{
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
			using (FileStream stream = File.Create(fileName))
			{
				pngBitmapEncoder.Save(stream);
			}
		}

		private static int _GetBitDepth()
		{
			if (s_bitDepth == 0)
			{
				using (SafeDC hdc = SafeDC.GetDesktop())
				{
					s_bitDepth = NativeMethods.GetDeviceCaps(hdc, DeviceCap.BITSPIXEL) * NativeMethods.GetDeviceCaps(hdc, DeviceCap.PLANES);
				}
			}
			return s_bitDepth;
		}

		public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
		{
			return _GetBestMatch(frames, _GetBitDepth(), width, height);
		}

		private static int _MatchImage(BitmapFrame frame, int bitDepth, int width, int height, int bpp)
		{
			return 2 * _WeightedAbs(bpp, bitDepth, fPunish: false) + _WeightedAbs(frame.PixelWidth, width, fPunish: true) + _WeightedAbs(frame.PixelHeight, height, fPunish: true);
		}

		private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
		{
			int num = valueHave - valueWant;
			if (num < 0)
			{
				num = (fPunish ? (-2) : (-1)) * num;
			}
			return num;
		}

		private static BitmapFrame _GetBestMatch(IList<BitmapFrame> frames, int bitDepth, int width, int height)
		{
			int num = 2147483647;
			int num2 = 0;
			int index = 0;
			bool flag = frames[0].Decoder is IconBitmapDecoder;
			for (int i = 0; i < frames.Count; i++)
			{
				if (num == 0)
				{
					break;
				}
				int num3 = flag ? frames[i].Thumbnail.Format.BitsPerPixel : frames[i].Format.BitsPerPixel;
				if (num3 == 0)
				{
					num3 = 8;
				}
				int num4 = _MatchImage(frames[i], bitDepth, width, height, num3);
				if (num4 < num)
				{
					index = i;
					num2 = num3;
					num = num4;
				}
				else if (num4 == num && num2 < num3)
				{
					index = i;
					num2 = num3;
				}
			}
			return frames[index];
		}

		public static int RGB(Color c)
		{
			return c.B | (c.G << 8) | (c.R << 16);
		}

		public static int AlphaRGB(Color c)
		{
			return c.B | (c.G << 8) | (c.R << 16) | (c.A << 24);
		}

		public static Color ColorFromArgbDword(uint color)
		{
			return Color.FromArgb((byte)((uint)((int)color & -16777216) >> 24), (byte)((color & 0xFF0000) >> 16), (byte)((color & 0xFF00) >> 8), (byte)(color & 0xFF));
		}

		public static bool AreImageSourcesEqual(ImageSource left, ImageSource right)
		{
			if (left == null)
			{
				return right == null;
			}
			if (right == null)
			{
				return false;
			}
			BitmapSource bmp = GenerateBitmapSource(left);
			BitmapSource bmp2 = GenerateBitmapSource(right);
			byte[] bytesFromBitmapSource = GetBytesFromBitmapSource(bmp);
			byte[] bytesFromBitmapSource2 = GetBytesFromBitmapSource(bmp2);
			if (bytesFromBitmapSource.Length != bytesFromBitmapSource2.Length)
			{
				return false;
			}
			return MemCmp(bytesFromBitmapSource, bytesFromBitmapSource2, bytesFromBitmapSource.Length);
		}

		public static IntPtr GenerateHICON(ImageSource image, Size dimensions)
		{
			if (image == null)
			{
				return IntPtr.Zero;
			}
			BitmapFrame bitmapFrame = image as BitmapFrame;
			if (bitmapFrame != null)
			{
				bitmapFrame = GetBestMatch(bitmapFrame.Decoder.Frames, (int)dimensions.Width, (int)dimensions.Height);
			}
			else
			{
				Rect rectangle = new Rect(0.0, 0.0, dimensions.Width, dimensions.Height);
				double num = dimensions.Width / dimensions.Height;
				double num2 = image.Width / image.Height;
				if (image.Width <= dimensions.Width && image.Height <= dimensions.Height)
				{
					rectangle = new Rect((dimensions.Width - image.Width) / 2.0, (dimensions.Height - image.Height) / 2.0, image.Width, image.Height);
				}
				else if (num > num2)
				{
					double num3 = image.Width / image.Height * dimensions.Width;
					rectangle = new Rect((dimensions.Width - num3) / 2.0, 0.0, num3, dimensions.Height);
				}
				else if (num < num2)
				{
					double num4 = image.Height / image.Width * dimensions.Height;
					rectangle = new Rect(0.0, (dimensions.Height - num4) / 2.0, dimensions.Width, num4);
				}
				DrawingVisual drawingVisual = new DrawingVisual();
				DrawingContext drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawImage(image, rectangle);
				drawingContext.Close();
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)dimensions.Width, (int)dimensions.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				renderTargetBitmap.Render(drawingVisual);
				bitmapFrame = BitmapFrame.Create(renderTargetBitmap);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(bitmapFrame);
				pngBitmapEncoder.Save(memoryStream);
				using (ManagedIStream stream = new ManagedIStream(memoryStream))
				{
					IntPtr bitmap = IntPtr.Zero;
					try
					{
						if (NativeMethods.GdipCreateBitmapFromStream(stream, out bitmap) != 0)
						{
							return IntPtr.Zero;
						}
						if (NativeMethods.GdipCreateHICONFromBitmap(bitmap, out IntPtr hbmReturn) != 0)
						{
							return IntPtr.Zero;
						}
						return hbmReturn;
					}
					finally
					{
						SafeDisposeImage(ref bitmap);
					}
				}
			}
		}

		public static void AddDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component != null)
			{
				DependencyPropertyDescriptor.FromProperty(property, component.GetType()).AddValueChanged(component, listener);
			}
		}

		public static void RemoveDependencyPropertyChangeListener(object component, DependencyProperty property, EventHandler listener)
		{
			if (component != null)
			{
				DependencyPropertyDescriptor.FromProperty(property, component.GetType()).RemoveValueChanged(component, listener);
			}
		}

		public static bool IsNonNegative(this Thickness thickness)
		{
			if (!thickness.Top.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Left.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Bottom.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!thickness.Right.IsFiniteAndNonNegative())
			{
				return false;
			}
			return true;
		}

		public static bool IsValid(this CornerRadius cornerRadius)
		{
			if (!cornerRadius.TopLeft.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.TopRight.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.BottomLeft.IsFiniteAndNonNegative())
			{
				return false;
			}
			if (!cornerRadius.BottomRight.IsFiniteAndNonNegative())
			{
				return false;
			}
			return true;
		}
	}
}
