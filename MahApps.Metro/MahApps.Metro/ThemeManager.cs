using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows;

namespace MahApps.Metro
{
	public static class ThemeManager
	{
		private static IList<Accent> _accents;

		private static IList<AppTheme> _appThemes;

		public static IEnumerable<Accent> Accents
		{
			get
			{
				if (_accents != null)
				{
					return _accents;
				}
				string[] array = new string[23]
				{
					"Red",
					"Green",
					"Blue",
					"Purple",
					"Orange",
					"Lime",
					"Emerald",
					"Teal",
					"Cyan",
					"Cobalt",
					"Indigo",
					"Violet",
					"Pink",
					"Magenta",
					"Crimson",
					"Amber",
					"Yellow",
					"Brown",
					"Olive",
					"Steel",
					"Mauve",
					"Taupe",
					"Sienna"
				};
				_accents = new List<Accent>(array.Length);
				try
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						Uri resourceAddress = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Accents/{text}.xaml");
						_accents.Add(new Accent(text, resourceAddress));
					}
				}
				catch (Exception innerException)
				{
					throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", innerException);
				}
				return _accents;
			}
		}

		public static IEnumerable<AppTheme> AppThemes
		{
			get
			{
				if (_appThemes != null)
				{
					return _appThemes;
				}
				string[] array = new string[2]
				{
					"BaseLight",
					"BaseDark"
				};
				_appThemes = new List<AppTheme>(array.Length);
				try
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						Uri resourceAddress = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Accents/{text}.xaml");
						_appThemes.Add(new AppTheme(text, resourceAddress));
					}
				}
				catch (Exception innerException)
				{
					throw new MahAppsException("This exception happens because you are maybe running that code out of the scope of a WPF application. Most likely because you are testing your configuration inside a unit test.", innerException);
				}
				return _appThemes;
			}
		}

		public static event EventHandler<OnThemeChangedEventArgs> IsThemeChanged;

		public static bool AddAccent(string name, Uri resourceAddress)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			if (GetAccent(name) != null)
			{
				return false;
			}
			_accents.Add(new Accent(name, resourceAddress));
			return true;
		}

		public static bool AddAccent(string name, ResourceDictionary resourceDictionary)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceDictionary == null)
			{
				throw new ArgumentNullException("resourceDictionary");
			}
			if (GetAccent(name) != null)
			{
				return false;
			}
			_accents.Add(new Accent(name, resourceDictionary));
			return true;
		}

		public static bool AddAppTheme(string name, Uri resourceAddress)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceAddress == null)
			{
				throw new ArgumentNullException("resourceAddress");
			}
			if (GetAppTheme(name) != null)
			{
				return false;
			}
			_appThemes.Add(new AppTheme(name, resourceAddress));
			return true;
		}

		public static bool AddAppTheme(string name, ResourceDictionary resourceDictionary)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (resourceDictionary == null)
			{
				throw new ArgumentNullException("resourceDictionary");
			}
			if (GetAppTheme(name) != null)
			{
				return false;
			}
			_appThemes.Add(new AppTheme(name, resourceDictionary));
			return true;
		}

		public static AppTheme GetAppTheme(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			return AppThemes.FirstOrDefault((AppTheme x) => AreResourceDictionarySourcesEqual(x.Resources, resources));
		}

		public static AppTheme GetAppTheme(string appThemeName)
		{
			if (appThemeName == null)
			{
				throw new ArgumentNullException("appThemeName");
			}
			return AppThemes.FirstOrDefault((AppTheme x) => x.Name.Equals(appThemeName, StringComparison.OrdinalIgnoreCase));
		}

		public static AppTheme GetInverseAppTheme(AppTheme appTheme)
		{
			if (appTheme == null)
			{
				throw new ArgumentNullException("appTheme");
			}
			if (appTheme.Name.EndsWith("dark", StringComparison.OrdinalIgnoreCase))
			{
				return GetAppTheme(appTheme.Name.ToLower().Replace("dark", "light"));
			}
			if (appTheme.Name.EndsWith("light", StringComparison.OrdinalIgnoreCase))
			{
				return GetAppTheme(appTheme.Name.ToLower().Replace("light", "dark"));
			}
			return null;
		}

		public static Accent GetAccent(string accentName)
		{
			if (accentName == null)
			{
				throw new ArgumentNullException("accentName");
			}
			return Accents.FirstOrDefault((Accent x) => x.Name.Equals(accentName, StringComparison.OrdinalIgnoreCase));
		}

		public static Accent GetAccent(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			Accent accent = Accents.FirstOrDefault((Accent x) => AreResourceDictionarySourcesEqual(x.Resources, resources));
			if (accent != null)
			{
				return accent;
			}
			if (resources.Source == null && IsAccentDictionary(resources))
			{
				return new Accent
				{
					Name = "Runtime accent",
					Resources = resources
				};
			}
			return null;
		}

		public static bool IsAccentDictionary(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			foreach (string item in new List<string>(new string[12]
			{
				"HighlightColor",
				"AccentBaseColor",
				"AccentColor",
				"AccentColor2",
				"AccentColor3",
				"AccentColor4",
				"HighlightBrush",
				"AccentBaseColorBrush",
				"AccentColorBrush",
				"AccentColorBrush2",
				"AccentColorBrush3",
				"AccentColorBrush4"
			}))
			{
				if (!(from object resourceKey in resources.Keys
				select resourceKey as string).Any((string keyAsString) => string.Equals(keyAsString, item)))
				{
					return false;
				}
			}
			return true;
		}

		public static object GetResourceFromAppStyle(Window window, string key)
		{
			Tuple<AppTheme, Accent> tuple = (window != null) ? DetectAppStyle(window) : DetectAppStyle(Application.Current);
			if (tuple == null && window != null)
			{
				tuple = DetectAppStyle(Application.Current);
			}
			if (tuple == null)
			{
				return null;
			}
			object result = tuple.Item1.Resources[key];
			object obj = tuple.Item2.Resources[key];
			if (obj != null)
			{
				return obj;
			}
			return result;
		}

		[SecurityCritical]
		public static void ChangeAppTheme(Application app, string themeName)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (themeName == null)
			{
				throw new ArgumentNullException("themeName");
			}
			Tuple<AppTheme, Accent> tuple = DetectAppStyle(app);
			AppTheme appTheme;
			if ((appTheme = GetAppTheme(themeName)) != null)
			{
				ChangeAppStyle(app.Resources, tuple, tuple.Item2, appTheme);
			}
		}

		[SecurityCritical]
		public static void ChangeAppTheme(Window window, string themeName)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (themeName == null)
			{
				throw new ArgumentNullException("themeName");
			}
			Tuple<AppTheme, Accent> tuple = DetectAppStyle(window);
			AppTheme appTheme;
			if ((appTheme = GetAppTheme(themeName)) != null)
			{
				ChangeAppStyle(window.Resources, tuple, tuple.Item2, appTheme);
			}
		}

		[SecurityCritical]
		public static void ChangeAppStyle(Application app, Accent newAccent, AppTheme newTheme)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (newAccent == null)
			{
				throw new ArgumentNullException("newAccent");
			}
			if (newTheme == null)
			{
				throw new ArgumentNullException("newTheme");
			}
			Tuple<AppTheme, Accent> oldThemeInfo = DetectAppStyle(app);
			ChangeAppStyle(app.Resources, oldThemeInfo, newAccent, newTheme);
		}

		[SecurityCritical]
		public static void ChangeAppStyle(Window window, Accent newAccent, AppTheme newTheme)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (newAccent == null)
			{
				throw new ArgumentNullException("newAccent");
			}
			if (newTheme == null)
			{
				throw new ArgumentNullException("newTheme");
			}
			Tuple<AppTheme, Accent> oldThemeInfo = DetectAppStyle(window);
			ChangeAppStyle(window.Resources, oldThemeInfo, newAccent, newTheme);
		}

		[SecurityCritical]
		private static void ChangeAppStyle(ResourceDictionary resources, Tuple<AppTheme, Accent> oldThemeInfo, Accent newAccent, AppTheme newTheme)
		{
			bool flag = false;
			if (oldThemeInfo != null)
			{
				Accent oldAccent = oldThemeInfo.Item2;
				if (oldAccent != null && oldAccent.Name != newAccent.Name)
				{
					ResourceDictionary resourceDictionary = resources.MergedDictionaries.FirstOrDefault((ResourceDictionary d) => AreResourceDictionarySourcesEqual(d, oldAccent.Resources));
					if (resourceDictionary != null)
					{
						resources.MergedDictionaries.Remove(resourceDictionary);
					}
					resources.MergedDictionaries.Add(newAccent.Resources);
					flag = true;
				}
				AppTheme oldTheme = oldThemeInfo.Item1;
				if (oldTheme != null && oldTheme != newTheme)
				{
					ResourceDictionary resourceDictionary2 = resources.MergedDictionaries.FirstOrDefault((ResourceDictionary d) => AreResourceDictionarySourcesEqual(d, oldTheme.Resources));
					if (resourceDictionary2 != null)
					{
						resources.MergedDictionaries.Remove(resourceDictionary2);
					}
					resources.MergedDictionaries.Add(newTheme.Resources);
					flag = true;
				}
			}
			else
			{
				ChangeAppStyle(resources, newAccent, newTheme);
				flag = true;
			}
			if (flag)
			{
				OnThemeChanged(newAccent, newTheme);
			}
		}

		[SecurityCritical]
		public static void ChangeAppStyle(ResourceDictionary resources, Accent newAccent, AppTheme newTheme)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			if (newAccent == null)
			{
				throw new ArgumentNullException("newAccent");
			}
			if (newTheme == null)
			{
				throw new ArgumentNullException("newTheme");
			}
			ApplyResourceDictionary(newAccent.Resources, resources);
			ApplyResourceDictionary(newTheme.Resources, resources);
		}

		[SecurityCritical]
		private static void ApplyResourceDictionary(ResourceDictionary newRd, ResourceDictionary oldRd)
		{
			oldRd.BeginInit();
			IDictionaryEnumerator enumerator = newRd.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
					if (oldRd.Contains(dictionaryEntry.Key))
					{
						oldRd.Remove(dictionaryEntry.Key);
					}
					oldRd.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}
			oldRd.EndInit();
		}

		internal static void CopyResource(ResourceDictionary fromRD, ResourceDictionary toRD)
		{
			if (fromRD == null)
			{
				throw new ArgumentNullException("fromRD");
			}
			if (toRD == null)
			{
				throw new ArgumentNullException("toRD");
			}
			ApplyResourceDictionary(fromRD, toRD);
			foreach (ResourceDictionary mergedDictionary in fromRD.MergedDictionaries)
			{
				CopyResource(mergedDictionary, toRD);
			}
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle()
		{
			try
			{
				Tuple<AppTheme, Accent> tuple = DetectAppStyle(Application.Current.MainWindow);
				if (tuple != null)
				{
					return tuple;
				}
			}
			catch (Exception)
			{
			}
			return DetectAppStyle(Application.Current);
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			Tuple<AppTheme, Accent> tuple = DetectAppStyle(window.Resources);
			if (tuple == null)
			{
				tuple = DetectAppStyle(Application.Current.Resources);
			}
			return tuple;
		}

		public static Tuple<AppTheme, Accent> DetectAppStyle(Application app)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			return DetectAppStyle(app.Resources);
		}

		private static Tuple<AppTheme, Accent> DetectAppStyle(ResourceDictionary resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			AppTheme detectedTheme = null;
			Tuple<AppTheme, Accent> detectedAccentTheme = null;
			if (DetectThemeFromResources(ref detectedTheme, resources) && GetThemeFromResources(detectedTheme, resources, ref detectedAccentTheme))
			{
				return new Tuple<AppTheme, Accent>(detectedAccentTheme.Item1, detectedAccentTheme.Item2);
			}
			return null;
		}

		internal static bool DetectThemeFromAppResources(out AppTheme detectedTheme)
		{
			detectedTheme = null;
			return DetectThemeFromResources(ref detectedTheme, Application.Current.Resources);
		}

		private static bool DetectThemeFromResources(ref AppTheme detectedTheme, ResourceDictionary dict)
		{
			IEnumerator<ResourceDictionary> enumerator = dict.MergedDictionaries.Reverse().GetEnumerator();
			while (enumerator.MoveNext())
			{
				ResourceDictionary current = enumerator.Current;
				AppTheme appTheme;
				if ((appTheme = GetAppTheme(current)) != null)
				{
					detectedTheme = appTheme;
					enumerator.Dispose();
					return true;
				}
				if (DetectThemeFromResources(ref detectedTheme, current))
				{
					return true;
				}
			}
			enumerator.Dispose();
			return false;
		}

		internal static bool GetThemeFromResources(AppTheme presetTheme, ResourceDictionary dict, ref Tuple<AppTheme, Accent> detectedAccentTheme)
		{
			Accent accent;
			if ((accent = GetAccent(dict)) != null)
			{
				detectedAccentTheme = Tuple.Create(presetTheme, accent);
				return true;
			}
			foreach (ResourceDictionary item in dict.MergedDictionaries.Reverse())
			{
				if (GetThemeFromResources(presetTheme, item, ref detectedAccentTheme))
				{
					return true;
				}
			}
			return false;
		}

		[SecurityCritical]
		private static void OnThemeChanged(Accent newAccent, AppTheme newTheme)
		{
			ThemeManager.IsThemeChanged?.Invoke(Application.Current, new OnThemeChangedEventArgs(newTheme, newAccent));
		}

		private static bool AreResourceDictionarySourcesEqual(ResourceDictionary first, ResourceDictionary second)
		{
			if (first == null || second == null)
			{
				return false;
			}
			if (first.Source == null || second.Source == null)
			{
				try
				{
					foreach (object key in first.Keys)
					{
						if (!second.Contains(key) || !object.Equals(first[key], second[key]))
						{
							return false;
						}
					}
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return Uri.Compare(first.Source, second.Source, UriComponents.Host | UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}
