using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class RevealImage : UserControl, IComponentConnector
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RevealImage), new UIPropertyMetadata(""));

		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(RevealImage), new UIPropertyMetadata(null));

		internal RevealImage revealImage;

		internal BeginStoryboard OnMouseLeave1_BeginStoryboard;

		internal Grid grid;

		internal Border border;

		internal TextBlock textBlock;

		private bool _contentLoaded;

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		public ImageSource Image
		{
			get
			{
				return (ImageSource)GetValue(ImageProperty);
			}
			set
			{
				SetValue(ImageProperty, value);
			}
		}

		public RevealImage()
		{
			InitializeComponent();
		}

		private static void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
		{
			Storyboard storyboard = new Storyboard
			{
				FillBehavior = FillBehavior.HoldEnd
			};
			StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames
			{
				Duration = new Duration(timeSpan)
			};
			string str = string.Empty;
			for (int i = 0; i < textToAnimate.Length; i++)
			{
				char c = textToAnimate[i];
				DiscreteStringKeyFrame discreteStringKeyFrame = new DiscreteStringKeyFrame
				{
					KeyTime = KeyTime.Paced
				};
				str = (discreteStringKeyFrame.Value = str + c.ToString());
				stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
			}
			Storyboard.SetTargetName(stringAnimationUsingKeyFrames, txt.Name);
			Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));
			storyboard.Children.Add(stringAnimationUsingKeyFrames);
			storyboard.Begin(txt);
		}

		private void GridMouseEnter(object sender, MouseEventArgs e)
		{
			TypewriteTextblock(Text.ToUpper(), textBlock, TimeSpan.FromSeconds(0.25));
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/controls/revealimage.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				revealImage = (RevealImage)target;
				break;
			case 2:
				OnMouseLeave1_BeginStoryboard = (BeginStoryboard)target;
				break;
			case 3:
				grid = (Grid)target;
				grid.MouseEnter += GridMouseEnter;
				break;
			case 4:
				border = (Border)target;
				break;
			case 5:
				textBlock = (TextBlock)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
