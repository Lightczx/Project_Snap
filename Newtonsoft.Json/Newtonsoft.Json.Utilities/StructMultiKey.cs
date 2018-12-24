using System;

namespace Newtonsoft.Json.Utilities
{
	internal readonly struct StructMultiKey<T1, T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		public readonly T1 Value1;

		public readonly T2 Value2;

		public StructMultiKey(T1 v1, T2 v2)
		{
			Value1 = v1;
			Value2 = v2;
		}

		public override int GetHashCode()
		{
			T1 value = Value1;
			ref T1 reference = ref value;
			T1 val = default(T1);
			int num;
			if (val == null)
			{
				val = reference;
				reference = ref val;
				if (val == null)
				{
					num = 0;
					goto IL_0038;
				}
			}
			num = reference.GetHashCode();
			goto IL_0038;
			IL_0038:
			T2 value2 = Value2;
			ref T2 reference2 = ref value2;
			T2 val2 = default(T2);
			int num2;
			if (val2 == null)
			{
				val2 = reference2;
				reference2 = ref val2;
				if (val2 == null)
				{
					num2 = 0;
					goto IL_0070;
				}
			}
			num2 = reference2.GetHashCode();
			goto IL_0070;
			IL_0070:
			return num ^ num2;
		}

		public override bool Equals(object obj)
		{
			object obj2;
			if (!((obj2 = obj) is StructMultiKey<T1, T2>))
			{
				return false;
			}
			StructMultiKey<T1, T2> other = (StructMultiKey<T1, T2>)obj2;
			return Equals(other);
		}

		public bool Equals(StructMultiKey<T1, T2> other)
		{
			if (object.Equals(Value1, other.Value1))
			{
				return object.Equals(Value2, other.Value2);
			}
			return false;
		}
	}
}
