using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Explicit)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct HRESULT
	{
		[FieldOffset(0)]
		private readonly uint _value;

		public static readonly HRESULT S_OK = new HRESULT(0);

		public static readonly HRESULT S_FALSE = new HRESULT(1);

		public static readonly HRESULT E_PENDING = new HRESULT(2147483658u);

		public static readonly HRESULT E_NOTIMPL = new HRESULT(2147500033u);

		public static readonly HRESULT E_NOINTERFACE = new HRESULT(2147500034u);

		public static readonly HRESULT E_POINTER = new HRESULT(2147500035u);

		public static readonly HRESULT E_ABORT = new HRESULT(2147500036u);

		public static readonly HRESULT E_FAIL = new HRESULT(2147500037u);

		public static readonly HRESULT E_UNEXPECTED = new HRESULT(2147549183u);

		public static readonly HRESULT STG_E_INVALIDFUNCTION = new HRESULT(2147680257u);

		public static readonly HRESULT OLE_E_ADVISENOTSUPPORTED = new HRESULT(2147745795u);

		public static readonly HRESULT DV_E_FORMATETC = new HRESULT(2147745892u);

		public static readonly HRESULT DV_E_TYMED = new HRESULT(2147745897u);

		public static readonly HRESULT DV_E_CLIPFORMAT = new HRESULT(2147745898u);

		public static readonly HRESULT DV_E_DVASPECT = new HRESULT(2147745899u);

		public static readonly HRESULT REGDB_E_CLASSNOTREG = new HRESULT(2147746132u);

		public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT(2147749635u);

		public static readonly HRESULT DESTS_E_NORECDOCS = new HRESULT(2147749636u);

		public static readonly HRESULT DESTS_E_NOTALLCLEARED = new HRESULT(2147749637u);

		public static readonly HRESULT E_ACCESSDENIED = new HRESULT(2147942405u);

		public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(2147942414u);

		public static readonly HRESULT E_INVALIDARG = new HRESULT(2147942487u);

		public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT(2147942934u);

		public static readonly HRESULT COR_E_OBJECTDISPOSED = new HRESULT(2148734498u);

		public static readonly HRESULT WC_E_GREATERTHAN = new HRESULT(3222072867u);

		public static readonly HRESULT WC_E_SYNTAX = new HRESULT(3222072877u);

		public static readonly HRESULT WINCODEC_ERR_GENERIC_ERROR = E_FAIL;

		public static readonly HRESULT WINCODEC_ERR_INVALIDPARAMETER = E_INVALIDARG;

		public static readonly HRESULT WINCODEC_ERR_OUTOFMEMORY = E_OUTOFMEMORY;

		public static readonly HRESULT WINCODEC_ERR_NOTIMPLEMENTED = E_NOTIMPL;

		public static readonly HRESULT WINCODEC_ERR_ABORTED = E_ABORT;

		public static readonly HRESULT WINCODEC_ERR_ACCESSDENIED = E_ACCESSDENIED;

		public static readonly HRESULT WINCODEC_ERR_VALUEOVERFLOW = INTSAFE_E_ARITHMETIC_OVERFLOW;

		public static readonly HRESULT WINCODEC_ERR_WRONGSTATE = Make(severe: true, Facility.WinCodec, 12036);

		public static readonly HRESULT WINCODEC_ERR_VALUEOUTOFRANGE = Make(severe: true, Facility.WinCodec, 12037);

		public static readonly HRESULT WINCODEC_ERR_UNKNOWNIMAGEFORMAT = Make(severe: true, Facility.WinCodec, 12039);

		public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDVERSION = Make(severe: true, Facility.WinCodec, 12043);

		public static readonly HRESULT WINCODEC_ERR_NOTINITIALIZED = Make(severe: true, Facility.WinCodec, 12044);

		public static readonly HRESULT WINCODEC_ERR_ALREADYLOCKED = Make(severe: true, Facility.WinCodec, 12045);

		public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTFOUND = Make(severe: true, Facility.WinCodec, 12096);

		public static readonly HRESULT WINCODEC_ERR_PROPERTYNOTSUPPORTED = Make(severe: true, Facility.WinCodec, 12097);

		public static readonly HRESULT WINCODEC_ERR_PROPERTYSIZE = Make(severe: true, Facility.WinCodec, 12098);

		public static readonly HRESULT WINCODEC_ERR_CODECPRESENT = Make(severe: true, Facility.WinCodec, 12099);

		public static readonly HRESULT WINCODEC_ERR_CODECNOTHUMBNAIL = Make(severe: true, Facility.WinCodec, 12100);

		public static readonly HRESULT WINCODEC_ERR_PALETTEUNAVAILABLE = Make(severe: true, Facility.WinCodec, 12101);

		public static readonly HRESULT WINCODEC_ERR_CODECTOOMANYSCANLINES = Make(severe: true, Facility.WinCodec, 12102);

		public static readonly HRESULT WINCODEC_ERR_INTERNALERROR = Make(severe: true, Facility.WinCodec, 12104);

		public static readonly HRESULT WINCODEC_ERR_SOURCERECTDOESNOTMATCHDIMENSIONS = Make(severe: true, Facility.WinCodec, 12105);

		public static readonly HRESULT WINCODEC_ERR_COMPONENTNOTFOUND = Make(severe: true, Facility.WinCodec, 12112);

		public static readonly HRESULT WINCODEC_ERR_IMAGESIZEOUTOFRANGE = Make(severe: true, Facility.WinCodec, 12113);

		public static readonly HRESULT WINCODEC_ERR_TOOMUCHMETADATA = Make(severe: true, Facility.WinCodec, 12114);

		public static readonly HRESULT WINCODEC_ERR_BADIMAGE = Make(severe: true, Facility.WinCodec, 12128);

		public static readonly HRESULT WINCODEC_ERR_BADHEADER = Make(severe: true, Facility.WinCodec, 12129);

		public static readonly HRESULT WINCODEC_ERR_FRAMEMISSING = Make(severe: true, Facility.WinCodec, 12130);

		public static readonly HRESULT WINCODEC_ERR_BADMETADATAHEADER = Make(severe: true, Facility.WinCodec, 12131);

		public static readonly HRESULT WINCODEC_ERR_BADSTREAMDATA = Make(severe: true, Facility.WinCodec, 12144);

		public static readonly HRESULT WINCODEC_ERR_STREAMWRITE = Make(severe: true, Facility.WinCodec, 12145);

		public static readonly HRESULT WINCODEC_ERR_STREAMREAD = Make(severe: true, Facility.WinCodec, 12146);

		public static readonly HRESULT WINCODEC_ERR_STREAMNOTAVAILABLE = Make(severe: true, Facility.WinCodec, 12147);

		public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDPIXELFORMAT = Make(severe: true, Facility.WinCodec, 12160);

		public static readonly HRESULT WINCODEC_ERR_UNSUPPORTEDOPERATION = Make(severe: true, Facility.WinCodec, 12161);

		public static readonly HRESULT WINCODEC_ERR_INVALIDREGISTRATION = Make(severe: true, Facility.WinCodec, 12170);

		public static readonly HRESULT WINCODEC_ERR_COMPONENTINITIALIZEFAILURE = Make(severe: true, Facility.WinCodec, 12171);

		public static readonly HRESULT WINCODEC_ERR_INSUFFICIENTBUFFER = Make(severe: true, Facility.WinCodec, 12172);

		public static readonly HRESULT WINCODEC_ERR_DUPLICATEMETADATAPRESENT = Make(severe: true, Facility.WinCodec, 12173);

		public static readonly HRESULT WINCODEC_ERR_PROPERTYUNEXPECTEDTYPE = Make(severe: true, Facility.WinCodec, 12174);

		public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDSIZE = Make(severe: true, Facility.WinCodec, 12175);

		public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYREQUEST = Make(severe: true, Facility.WinCodec, 12176);

		public static readonly HRESULT WINCODEC_ERR_UNEXPECTEDMETADATATYPE = Make(severe: true, Facility.WinCodec, 12177);

		public static readonly HRESULT WINCODEC_ERR_REQUESTONLYVALIDATMETADATAROOT = Make(severe: true, Facility.WinCodec, 12178);

		public static readonly HRESULT WINCODEC_ERR_INVALIDQUERYCHARACTER = Make(severe: true, Facility.WinCodec, 12179);

		public Facility Facility => GetFacility((int)_value);

		public int Code => GetCode((int)_value);

		public bool Succeeded => (int)_value >= 0;

		public bool Failed => (int)_value < 0;

		public HRESULT(uint i)
		{
			_value = i;
		}

		public HRESULT(int i)
		{
			_value = (uint)i;
		}

		public static explicit operator int(HRESULT hr)
		{
			return (int)hr._value;
		}

		public static HRESULT Make(bool severe, Facility facility, int code)
		{
			return new HRESULT((uint)((severe ? (-2147483648) : 0) | ((int)facility << 16) | code));
		}

		public static Facility GetFacility(int errorCode)
		{
			return (Facility)((errorCode >> 16) & 0x1FFF);
		}

		public static int GetCode(int error)
		{
			return error & 0xFFFF;
		}

		public override string ToString()
		{
			FieldInfo[] fields = typeof(HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType == typeof(HRESULT) && (HRESULT)fieldInfo.GetValue(null) == this)
				{
					return fieldInfo.Name;
				}
			}
			if (Facility == Facility.Win32)
			{
				fields = typeof(Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public);
				foreach (FieldInfo fieldInfo2 in fields)
				{
					if (fieldInfo2.FieldType == typeof(Win32Error) && (HRESULT)(Win32Error)fieldInfo2.GetValue(null) == this)
					{
						return "HRESULT_FROM_WIN32(" + fieldInfo2.Name + ")";
					}
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", new object[1]
			{
				_value
			});
		}

		public override bool Equals(object obj)
		{
			try
			{
				return ((HRESULT)obj)._value == _value;
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
		{
			return hrLeft._value == hrRight._value;
		}

		public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
		{
			return !(hrLeft == hrRight);
		}

		public void ThrowIfFailed()
		{
			ThrowIfFailed(null);
		}

		public void ThrowIfFailed(string message)
		{
			if (Failed)
			{
				if (string.IsNullOrEmpty(message))
				{
					message = ToString();
				}
				Exception ex = Marshal.GetExceptionForHR((int)_value, new IntPtr(-1));
				if (ex.GetType() == typeof(COMException))
				{
					Facility facility = Facility;
					ex = ((facility != Facility.Win32) ? ((ExternalException)new COMException(message, (int)_value)) : ((ExternalException)new Win32Exception(Code, message)));
				}
				else
				{
					ConstructorInfo constructor = ex.GetType().GetConstructor(new Type[1]
					{
						typeof(string)
					});
					if (null != constructor)
					{
						ex = (constructor.Invoke(new object[1]
						{
							message
						}) as Exception);
					}
				}
				throw ex;
			}
		}

		public static void ThrowLastError()
		{
			((HRESULT)Win32Error.GetLastError()).ThrowIfFailed();
		}
	}
}
