using System;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	public static class Extensions
	{
		public static T Invoke<T>(this DispatcherObject dispatcherObject, Func<T> func)
		{
			if (dispatcherObject == null)
			{
				throw new ArgumentNullException("dispatcherObject");
			}
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
			if (dispatcherObject.Dispatcher.CheckAccess())
			{
				return func();
			}
			return dispatcherObject.Dispatcher.Invoke((Func<T>)func.Invoke);
		}

		public static void Invoke(this DispatcherObject dispatcherObject, Action invokeAction)
		{
			if (dispatcherObject == null)
			{
				throw new ArgumentNullException("dispatcherObject");
			}
			if (invokeAction == null)
			{
				throw new ArgumentNullException("invokeAction");
			}
			if (dispatcherObject.Dispatcher.CheckAccess())
			{
				invokeAction();
			}
			else
			{
				dispatcherObject.Dispatcher.Invoke(invokeAction);
			}
		}

		public static void BeginInvoke(this DispatcherObject dispatcherObject, Action invokeAction, DispatcherPriority priority = DispatcherPriority.Background)
		{
			if (dispatcherObject == null)
			{
				throw new ArgumentNullException("dispatcherObject");
			}
			if (invokeAction == null)
			{
				throw new ArgumentNullException("invokeAction");
			}
			dispatcherObject.Dispatcher.BeginInvoke(priority, invokeAction);
		}
	}
}
