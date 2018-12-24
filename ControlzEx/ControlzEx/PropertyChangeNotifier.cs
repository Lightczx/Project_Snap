using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace ControlzEx
{
	public sealed class PropertyChangeNotifier : DependencyObject, IDisposable
	{
		private WeakReference _propertySource;

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyChangeNotifier), new FrameworkPropertyMetadata(null, OnPropertyChanged));

		public DependencyObject PropertySource
		{
			get
			{
				try
				{
					return _propertySource.IsAlive ? (_propertySource.Target as DependencyObject) : null;
				}
				catch
				{
					return null;
				}
			}
		}

		[Description("Returns/sets the value of the property")]
		[Category("Behavior")]
		[Bindable(true)]
		public object Value
		{
			get
			{
				return GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public bool RaiseValueChanged
		{
			get;
			set;
		} = true;


		public event EventHandler ValueChanged;

		public PropertyChangeNotifier(DependencyObject propertySource, string path)
			: this(propertySource, new PropertyPath(path))
		{
		}

		public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
			: this(propertySource, new PropertyPath(property))
		{
		}

		public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
		{
			if (propertySource == null)
			{
				throw new ArgumentNullException("propertySource");
			}
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			_propertySource = new WeakReference(propertySource);
			Binding binding = new Binding
			{
				Path = property,
				Mode = BindingMode.OneWay,
				Source = propertySource
			};
			BindingOperations.SetBinding(this, ValueProperty, binding);
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PropertyChangeNotifier propertyChangeNotifier = (PropertyChangeNotifier)d;
			if (propertyChangeNotifier.RaiseValueChanged)
			{
				propertyChangeNotifier.ValueChanged?.Invoke(propertyChangeNotifier.PropertySource, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			BindingOperations.ClearBinding(this, ValueProperty);
		}
	}
}
