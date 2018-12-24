using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Shapes;

namespace MahApps.Metro.Behaviours
{
	public class BindableResourceBehavior : Behavior<Shape>
	{
		public static readonly DependencyProperty ResourceNameProperty = DependencyProperty.Register("ResourceName", typeof(string), typeof(BindableResourceBehavior), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(BindableResourceBehavior), new PropertyMetadata((object)null));

		public string ResourceName
		{
			get
			{
				return (string)base.GetValue(ResourceNameProperty);
			}
			set
			{
				base.SetValue(ResourceNameProperty, (object)value);
			}
		}

		public DependencyProperty Property
		{
			get
			{
				return (DependencyProperty)base.GetValue(PropertyProperty);
			}
			set
			{
				base.SetValue(PropertyProperty, (object)value);
			}
		}

		protected override void OnAttached()
		{
			base.get_AssociatedObject().SetResourceReference(Property, ResourceName);
			this.OnAttached();
		}
	}
}
