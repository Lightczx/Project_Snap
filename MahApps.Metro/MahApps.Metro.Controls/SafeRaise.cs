using System;

namespace MahApps.Metro.Controls
{
	internal static class SafeRaise
	{
		public delegate T GetEventArgs<T>() where T : EventArgs;

		public static void Raise(EventHandler eventToRaise, object sender)
		{
			eventToRaise?.Invoke(sender, EventArgs.Empty);
		}

		public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
		{
			Raise(eventToRaise, sender, EventArgs.Empty);
		}

		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args) where T : EventArgs
		{
			eventToRaise?.Invoke(sender, args);
		}

		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, GetEventArgs<T> getEventArgs) where T : EventArgs
		{
			eventToRaise?.Invoke(sender, getEventArgs());
		}
	}
}
