using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace MahApps.Metro.Behaviours
{
	public class TiltBehavior : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty KeepDraggingProperty = DependencyProperty.Register("KeepDragging", typeof(bool), typeof(TiltBehavior), new PropertyMetadata(true));

		public static readonly DependencyProperty TiltFactorProperty = DependencyProperty.Register("TiltFactor", typeof(int), typeof(TiltBehavior), new PropertyMetadata(20));

		private bool isPressed;

		private Thickness originalMargin;

		private Panel originalPanel;

		private Size originalSize;

		private FrameworkElement attachedElement;

		private Point current = new Point(-99.0, -99.0);

		private int times = -1;

		public bool KeepDragging
		{
			get
			{
				return (bool)base.GetValue(KeepDraggingProperty);
			}
			set
			{
				base.SetValue(KeepDraggingProperty, (object)value);
			}
		}

		public int TiltFactor
		{
			get
			{
				return (int)base.GetValue(TiltFactorProperty);
			}
			set
			{
				base.SetValue(TiltFactorProperty, (object)value);
			}
		}

		public Planerator RotatorParent
		{
			get;
			set;
		}

		protected override void OnAttached()
		{
			attachedElement = base.get_AssociatedObject();
			if (!(attachedElement is ListBox))
			{
				Panel attachedElementPanel = attachedElement as Panel;
				if (attachedElementPanel != null)
				{
					attachedElementPanel.Loaded += delegate
					{
						attachedElementPanel.Children.Cast<UIElement>().ToList().ForEach(delegate(UIElement element)
						{
							((FreezableCollection<Behavior>)Interaction.GetBehaviors((DependencyObject)element)).Add(new TiltBehavior
							{
								KeepDragging = KeepDragging,
								TiltFactor = TiltFactor
							});
						});
					};
				}
				else
				{
					originalPanel = ((attachedElement.Parent as Panel) ?? GetParentPanel(attachedElement));
					originalMargin = attachedElement.Margin;
					originalSize = new Size(attachedElement.Width, attachedElement.Height);
					double left = Canvas.GetLeft(attachedElement);
					double right = Canvas.GetRight(attachedElement);
					double top = Canvas.GetTop(attachedElement);
					double bottom = Canvas.GetBottom(attachedElement);
					int zIndex = Panel.GetZIndex(attachedElement);
					VerticalAlignment verticalAlignment = attachedElement.VerticalAlignment;
					HorizontalAlignment horizontalAlignment = attachedElement.HorizontalAlignment;
					RotatorParent = new Planerator
					{
						Margin = originalMargin,
						Width = originalSize.Width,
						Height = originalSize.Height,
						VerticalAlignment = verticalAlignment,
						HorizontalAlignment = horizontalAlignment
					};
					RotatorParent.SetValue(Canvas.LeftProperty, left);
					RotatorParent.SetValue(Canvas.RightProperty, right);
					RotatorParent.SetValue(Canvas.TopProperty, top);
					RotatorParent.SetValue(Canvas.BottomProperty, bottom);
					RotatorParent.SetValue(Panel.ZIndexProperty, zIndex);
					originalPanel.Children.Remove(attachedElement);
					attachedElement.Margin = default(Thickness);
					attachedElement.Width = double.NaN;
					attachedElement.Height = double.NaN;
					originalPanel.Children.Add(RotatorParent);
					RotatorParent.Child = attachedElement;
					CompositionTarget.Rendering += CompositionTargetRendering;
				}
			}
		}

		protected override void OnDetaching()
		{
			this.OnDetaching();
			CompositionTarget.Rendering -= CompositionTargetRendering;
		}

		private void CompositionTargetRendering(object sender, EventArgs e)
		{
			if (KeepDragging)
			{
				current = Mouse.GetPosition(RotatorParent.Child);
				if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					if (current.X > 0.0 && current.X < attachedElement.ActualWidth && current.Y > 0.0 && current.Y < attachedElement.ActualHeight)
					{
						RotatorParent.RotationY = (double)(-1 * TiltFactor) + current.X * 2.0 * (double)TiltFactor / attachedElement.ActualWidth;
						RotatorParent.RotationX = (double)(-1 * TiltFactor) + current.Y * 2.0 * (double)TiltFactor / attachedElement.ActualHeight;
					}
				}
				else
				{
					RotatorParent.RotationY = ((RotatorParent.RotationY - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationY - 5.0));
					RotatorParent.RotationX = ((RotatorParent.RotationX - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationX - 5.0));
				}
			}
			else if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				if (!isPressed)
				{
					current = Mouse.GetPosition(RotatorParent.Child);
					if (current.X > 0.0 && current.X < attachedElement.ActualWidth && current.Y > 0.0 && current.Y < attachedElement.ActualHeight)
					{
						RotatorParent.RotationY = (double)(-1 * TiltFactor) + current.X * 2.0 * (double)TiltFactor / attachedElement.ActualWidth;
						RotatorParent.RotationX = (double)(-1 * TiltFactor) + current.Y * 2.0 * (double)TiltFactor / attachedElement.ActualHeight;
					}
					isPressed = true;
				}
				if (isPressed && times == 7)
				{
					RotatorParent.RotationY = ((RotatorParent.RotationY - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationY - 5.0));
					RotatorParent.RotationX = ((RotatorParent.RotationX - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationX - 5.0));
				}
				else if (isPressed && times < 7)
				{
					times++;
				}
			}
			else
			{
				isPressed = false;
				times = -1;
				RotatorParent.RotationY = ((RotatorParent.RotationY - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationY - 5.0));
				RotatorParent.RotationX = ((RotatorParent.RotationX - 5.0 < 0.0) ? 0.0 : (RotatorParent.RotationX - 5.0));
			}
		}

		private static Panel GetParentPanel(DependencyObject element)
		{
			return element.TryFindParent<Panel>();
		}
	}
}
