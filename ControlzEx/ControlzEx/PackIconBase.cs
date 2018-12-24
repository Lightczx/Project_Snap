using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlzEx
{
	public abstract class PackIconBase : Control
	{
		internal abstract void UpdateData();
	}
	public abstract class PackIconBase<TKind> : PackIconBase
	{
		private static Lazy<IDictionary<TKind, string>> _dataIndex;

		public static readonly DependencyProperty KindProperty = DependencyProperty.Register("Kind", typeof(TKind), typeof(PackIconBase<TKind>), new PropertyMetadata(default(TKind), KindPropertyChangedCallback));

		private static readonly DependencyPropertyKey DataPropertyKey = DependencyProperty.RegisterReadOnly("Data", typeof(string), typeof(PackIconBase<TKind>), new PropertyMetadata(""));

		public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

		public TKind Kind
		{
			get
			{
				return (TKind)GetValue(KindProperty);
			}
			set
			{
				SetValue(KindProperty, value);
			}
		}

		[TypeConverter(typeof(GeometryConverter))]
		public string Data
		{
			get
			{
				return (string)GetValue(DataProperty);
			}
			private set
			{
				SetValue(DataPropertyKey, value);
			}
		}

		protected PackIconBase(Func<IDictionary<TKind, string>> dataIndexFactory)
		{
			if (dataIndexFactory == null)
			{
				throw new ArgumentNullException("dataIndexFactory");
			}
			if (_dataIndex == null)
			{
				_dataIndex = new Lazy<IDictionary<TKind, string>>(dataIndexFactory);
			}
		}

		private static void KindPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			((PackIconBase)dependencyObject).UpdateData();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UpdateData();
		}

		internal override void UpdateData()
		{
			string value = null;
			if (_dataIndex.Value != null)
			{
				_dataIndex.Value.TryGetValue(Kind, out value);
			}
			Data = value;
		}
	}
}
