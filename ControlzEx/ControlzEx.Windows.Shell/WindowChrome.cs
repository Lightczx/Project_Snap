using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace ControlzEx.Windows.Shell
{
	public class WindowChrome : Freezable
	{
		private struct _SystemParameterBoundProperty
		{
			public string SystemParameterPropertyName
			{
				get;
				set;
			}

			public DependencyProperty DependencyProperty
			{
				get;
				set;
			}
		}

		public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, _OnChromeChanged));

		public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ResizeGripDirectionProperty = DependencyProperty.RegisterAttached("ResizeGripDirection", typeof(ResizeGripDirection), typeof(WindowChrome), new FrameworkPropertyMetadata(ResizeGripDirection.None, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => (double)value >= 0.0);

		public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness)), (object value) => ((Thickness)value).IsNonNegative());

		public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}, (DependencyObject d, object o) => _CoerceGlassFrameThickness((Thickness)o)));

		public static readonly DependencyProperty UseAeroCaptionButtonsProperty = DependencyProperty.Register("UseAeroCaptionButtons", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}));

		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowChrome), new PropertyMetadata(default(CornerRadius), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => ((CornerRadius)value).IsValid());

		public static readonly DependencyProperty SacrificialEdgeProperty = DependencyProperty.Register("SacrificialEdge", typeof(SacrificialEdge), typeof(WindowChrome), new PropertyMetadata(SacrificialEdge.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), _IsValidSacrificialEdge);

		private static readonly SacrificialEdge SacrificialEdge_All = SacrificialEdge.Left | SacrificialEdge.Top | SacrificialEdge.Right | SacrificialEdge.Bottom;

		private static readonly List<_SystemParameterBoundProperty> _BoundProperties = new List<_SystemParameterBoundProperty>
		{
			new _SystemParameterBoundProperty
			{
				DependencyProperty = CornerRadiusProperty,
				SystemParameterPropertyName = "WindowCornerRadius"
			},
			new _SystemParameterBoundProperty
			{
				DependencyProperty = CaptionHeightProperty,
				SystemParameterPropertyName = "WindowCaptionHeight"
			},
			new _SystemParameterBoundProperty
			{
				DependencyProperty = ResizeBorderThicknessProperty,
				SystemParameterPropertyName = "WindowResizeBorderThickness"
			},
			new _SystemParameterBoundProperty
			{
				DependencyProperty = GlassFrameThicknessProperty,
				SystemParameterPropertyName = "WindowNonClientFrameThickness"
			}
		};

		public static Thickness GlassFrameCompleteThickness => new Thickness(-1.0);

		public double CaptionHeight
		{
			get
			{
				return (double)GetValue(CaptionHeightProperty);
			}
			set
			{
				SetValue(CaptionHeightProperty, value);
			}
		}

		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)GetValue(ResizeBorderThicknessProperty);
			}
			set
			{
				SetValue(ResizeBorderThicknessProperty, value);
			}
		}

		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)GetValue(GlassFrameThicknessProperty);
			}
			set
			{
				SetValue(GlassFrameThicknessProperty, value);
			}
		}

		public bool UseAeroCaptionButtons
		{
			get
			{
				return (bool)GetValue(UseAeroCaptionButtonsProperty);
			}
			set
			{
				SetValue(UseAeroCaptionButtonsProperty, value);
			}
		}

		public bool IgnoreTaskbarOnMaximize
		{
			get
			{
				return (bool)GetValue(IgnoreTaskbarOnMaximizeProperty);
			}
			set
			{
				SetValue(IgnoreTaskbarOnMaximizeProperty, value);
			}
		}

		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)GetValue(CornerRadiusProperty);
			}
			set
			{
				SetValue(CornerRadiusProperty, value);
			}
		}

		public SacrificialEdge SacrificialEdge
		{
			get
			{
				return (SacrificialEdge)GetValue(SacrificialEdgeProperty);
			}
			set
			{
				SetValue(SacrificialEdgeProperty, value);
			}
		}

		internal event EventHandler PropertyChangedThatRequiresRepaint;

		private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!DesignerProperties.GetIsInDesignMode(d))
			{
				Window window = (Window)d;
				WindowChrome windowChrome = (WindowChrome)e.NewValue;
				WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
				if (windowChromeWorker == null)
				{
					windowChromeWorker = new WindowChromeWorker();
					WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
				}
				windowChromeWorker.SetWindowChrome(windowChrome);
			}
		}

		[Category("ControlzEx")]
		public static WindowChrome GetWindowChrome(Window window)
		{
			Verify.IsNotNull(window, "window");
			return (WindowChrome)window.GetValue(WindowChromeProperty);
		}

		public static void SetWindowChrome(Window window, WindowChrome chrome)
		{
			Verify.IsNotNull(window, "window");
			window.SetValue(WindowChromeProperty, chrome);
		}

		[Category("ControlzEx")]
		public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
		{
			Verify.IsNotNull(inputElement, "inputElement");
			DependencyObject obj = inputElement as DependencyObject;
			if (obj == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (bool)obj.GetValue(IsHitTestVisibleInChromeProperty);
		}

		public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
		{
			Verify.IsNotNull(inputElement, "inputElement");
			DependencyObject obj = inputElement as DependencyObject;
			if (obj == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			obj.SetValue(IsHitTestVisibleInChromeProperty, hitTestVisible);
		}

		[Category("ControlzEx")]
		public static ResizeGripDirection GetResizeGripDirection(IInputElement inputElement)
		{
			Verify.IsNotNull(inputElement, "inputElement");
			DependencyObject obj = inputElement as DependencyObject;
			if (obj == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (ResizeGripDirection)obj.GetValue(ResizeGripDirectionProperty);
		}

		public static void SetResizeGripDirection(IInputElement inputElement, ResizeGripDirection direction)
		{
			Verify.IsNotNull(inputElement, "inputElement");
			DependencyObject obj = inputElement as DependencyObject;
			if (obj == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			obj.SetValue(ResizeGripDirectionProperty, direction);
		}

		private static object _CoerceGlassFrameThickness(Thickness thickness)
		{
			if (!thickness.IsNonNegative())
			{
				return GlassFrameCompleteThickness;
			}
			return thickness;
		}

		private static bool _IsValidSacrificialEdge(object value)
		{
			SacrificialEdge sacrificialEdge = SacrificialEdge.None;
			try
			{
				sacrificialEdge = (SacrificialEdge)value;
			}
			catch (InvalidCastException)
			{
				return false;
			}
			if (sacrificialEdge == SacrificialEdge.None)
			{
				return true;
			}
			if ((sacrificialEdge | SacrificialEdge_All) != SacrificialEdge_All)
			{
				return false;
			}
			if (sacrificialEdge == SacrificialEdge_All)
			{
				return false;
			}
			return true;
		}

		protected override Freezable CreateInstanceCore()
		{
			return new WindowChrome();
		}

		public WindowChrome()
		{
			foreach (_SystemParameterBoundProperty boundProperty in _BoundProperties)
			{
				BindingOperations.SetBinding(this, boundProperty.DependencyProperty, new Binding
				{
					Path = new PropertyPath("(SystemParameters." + boundProperty.SystemParameterPropertyName + ")"),
					Mode = BindingMode.OneWay,
					UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				});
			}
		}

		private void _OnPropertyChangedThatRequiresRepaint()
		{
			this.PropertyChangedThatRequiresRepaint?.Invoke(this, EventArgs.Empty);
		}
	}
}
