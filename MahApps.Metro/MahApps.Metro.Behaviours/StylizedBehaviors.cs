using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Behaviours
{
	public class StylizedBehaviors
	{
		public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof(StylizedBehaviorCollection), typeof(StylizedBehaviors), new FrameworkPropertyMetadata(null, OnPropertyChanged));

		private static readonly DependencyProperty OriginalBehaviorProperty = DependencyProperty.RegisterAttached("OriginalBehavior", typeof(Behavior), typeof(StylizedBehaviors), new UIPropertyMetadata(null));

		[Category("MahApps.Metro")]
		public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
		{
			return (StylizedBehaviorCollection)uie.GetValue(BehaviorsProperty);
		}

		public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
		{
			uie.SetValue(BehaviorsProperty, value);
		}

		private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			FrameworkElement frameworkElement = dpo as FrameworkElement;
			if (frameworkElement != null)
			{
				StylizedBehaviorCollection stylizedBehaviorCollection = e.NewValue as StylizedBehaviorCollection;
				StylizedBehaviorCollection stylizedBehaviorCollection2 = e.OldValue as StylizedBehaviorCollection;
				if (stylizedBehaviorCollection != stylizedBehaviorCollection2)
				{
					BehaviorCollection behaviors = Interaction.GetBehaviors((DependencyObject)frameworkElement);
					frameworkElement.Unloaded -= FrameworkElementUnloaded;
					if (stylizedBehaviorCollection2 != null)
					{
						foreach (Behavior item in stylizedBehaviorCollection2)
						{
							int indexOf = GetIndexOf(behaviors, item);
							if (indexOf >= 0)
							{
								((FreezableCollection<Behavior>)behaviors).RemoveAt(indexOf);
							}
						}
					}
					if (stylizedBehaviorCollection != null)
					{
						foreach (Behavior item2 in stylizedBehaviorCollection)
						{
							if (GetIndexOf(behaviors, item2) < 0)
							{
								Behavior val = ((Animatable)item2).Clone();
								SetOriginalBehavior((DependencyObject)val, item2);
								((FreezableCollection<Behavior>)behaviors).Add(val);
							}
						}
					}
					if (((FreezableCollection<Behavior>)behaviors).Count > 0)
					{
						frameworkElement.Unloaded += FrameworkElementUnloaded;
					}
					frameworkElement.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
				}
			}
		}

		private static void Dispatcher_ShutdownStarted(object sender, EventArgs e)
		{
		}

		private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				foreach (Behavior item in (FreezableCollection<Behavior>)Interaction.GetBehaviors((DependencyObject)frameworkElement))
				{
					item.Detach();
				}
				frameworkElement.Loaded += FrameworkElementLoaded;
			}
		}

		private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Loaded -= FrameworkElementLoaded;
				foreach (Behavior item in (FreezableCollection<Behavior>)Interaction.GetBehaviors((DependencyObject)frameworkElement))
				{
					item.Attach((DependencyObject)frameworkElement);
				}
			}
		}

		private static int GetIndexOf(BehaviorCollection itemBehaviors, Behavior behavior)
		{
			int result = -1;
			Behavior originalBehavior = GetOriginalBehavior((DependencyObject)behavior);
			for (int i = 0; i < ((FreezableCollection<Behavior>)itemBehaviors).Count; i++)
			{
				Behavior val = ((FreezableCollection<Behavior>)itemBehaviors)[i];
				if (val == behavior || val == originalBehavior)
				{
					result = i;
					break;
				}
				Behavior originalBehavior2 = GetOriginalBehavior((DependencyObject)val);
				if (originalBehavior2 == behavior || originalBehavior2 == originalBehavior)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private static Behavior GetOriginalBehavior(DependencyObject obj)
		{
			return obj.GetValue(OriginalBehaviorProperty) as Behavior;
		}

		private static void SetOriginalBehavior(DependencyObject obj, Behavior value)
		{
			obj.SetValue(OriginalBehaviorProperty, value);
		}
	}
}
