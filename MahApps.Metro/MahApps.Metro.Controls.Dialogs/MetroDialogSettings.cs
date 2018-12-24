using System;
using System.Threading;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public class MetroDialogSettings
	{
		public bool OwnerCanCloseWithDialog
		{
			get;
			set;
		}

		public string AffirmativeButtonText
		{
			get;
			set;
		}

		public bool AnimateHide
		{
			get;
			set;
		}

		public bool AnimateShow
		{
			get;
			set;
		}

		public CancellationToken CancellationToken
		{
			get;
			set;
		}

		public MetroDialogColorScheme ColorScheme
		{
			get;
			set;
		}

		public ResourceDictionary CustomResourceDictionary
		{
			get;
			set;
		}

		public MessageDialogResult DefaultButtonFocus
		{
			get;
			set;
		}

		public string DefaultText
		{
			get;
			set;
		}

		public double DialogMessageFontSize
		{
			get;
			set;
		}

		public MessageDialogResult? DialogResultOnCancel
		{
			get;
			set;
		}

		public double DialogTitleFontSize
		{
			get;
			set;
		}

		public string FirstAuxiliaryButtonText
		{
			get;
			set;
		}

		public double MaximumBodyHeight
		{
			get;
			set;
		}

		public string NegativeButtonText
		{
			get;
			set;
		}

		public string SecondAuxiliaryButtonText
		{
			get;
			set;
		}

		[Obsolete("This property will be deleted in the next release.")]
		public bool SuppressDefaultResources
		{
			get;
			set;
		}

		public MetroDialogSettings()
		{
			OwnerCanCloseWithDialog = false;
			AffirmativeButtonText = "OK";
			NegativeButtonText = "Cancel";
			ColorScheme = MetroDialogColorScheme.Theme;
			bool animateShow = AnimateHide = true;
			AnimateShow = animateShow;
			MaximumBodyHeight = double.NaN;
			DefaultText = "";
			DefaultButtonFocus = MessageDialogResult.Negative;
			CancellationToken = CancellationToken.None;
			DialogTitleFontSize = double.NaN;
			DialogMessageFontSize = double.NaN;
			DialogResultOnCancel = null;
		}
	}
}
