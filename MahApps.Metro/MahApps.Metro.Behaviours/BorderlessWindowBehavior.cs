using ControlzEx.Behaviors;
using ControlzEx.Windows.Shell;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Behaviours
{
	public class BorderlessWindowBehavior : WindowChromeBehavior
	{
		protected override void OnAttached()
		{
			BindingOperations.SetBinding((DependencyObject)this, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty, new Binding
			{
				Path = new PropertyPath(MetroWindow.IgnoreTaskbarOnMaximizeProperty),
				Source = base.get_AssociatedObject()
			});
			BindingOperations.SetBinding((DependencyObject)this, WindowChromeBehavior.ResizeBorderThicknessProperty, new Binding
			{
				Path = new PropertyPath(MetroWindow.ResizeBorderThicknessProperty),
				Source = base.get_AssociatedObject()
			});
			BindingOperations.SetBinding((DependencyObject)this, WindowChromeBehavior.GlowBrushProperty, new Binding
			{
				Path = new PropertyPath(MetroWindow.GlowBrushProperty),
				Source = base.get_AssociatedObject()
			});
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			BindingOperations.ClearBinding((DependencyObject)this, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty);
			BindingOperations.ClearBinding((DependencyObject)this, WindowChromeBehavior.ResizeBorderThicknessProperty);
			BindingOperations.ClearBinding((DependencyObject)this, WindowChromeBehavior.GlowBrushProperty);
			base.OnDetaching();
		}

		protected override void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
		{
			MetroWindow metroWindow = sender as MetroWindow;
			if (metroWindow != null && metroWindow.ResizeMode != 0)
			{
				metroWindow.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_Icon");
				metroWindow.SetWindowChromeResizeGripDirection("WindowResizeGrip", ResizeGripDirection.BottomRight);
			}
		}
	}
}
