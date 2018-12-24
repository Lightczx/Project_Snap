using System.Windows;

namespace MahApps.Metro.Controls
{
	public static class Spelling
	{
		public static ResourceKey SuggestionMenuItemStyleKey
		{
			get;
		} = new ComponentResourceKey(typeof(Spelling), SpellingResourceKeyId.SuggestionMenuItemStyle);


		public static ResourceKey IgnoreAllMenuItemStyleKey
		{
			get;
		} = new ComponentResourceKey(typeof(Spelling), SpellingResourceKeyId.IgnoreAllMenuItemStyle);


		public static ResourceKey NoSuggestionsMenuItemStyleKey
		{
			get;
		} = new ComponentResourceKey(typeof(Spelling), SpellingResourceKeyId.NoSuggestionsMenuItemStyle);


		public static ResourceKey SeparatorStyleKey
		{
			get;
		} = new ComponentResourceKey(typeof(Spelling), SpellingResourceKeyId.SeparatorStyle);

	}
}
