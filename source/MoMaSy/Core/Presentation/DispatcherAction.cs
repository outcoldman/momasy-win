using System;
using System.Windows.Threading;

namespace outcold.MoMaSy.Core.Presentation
{
	internal static class DispatcherAction
	{
		public static void Do(this Dispatcher dispatcher, Action action)
		{
			dispatcher.Invoke(action);
		}

		public static void DoAsync(this Dispatcher dispatcher, Action action)
		{
			dispatcher.BeginInvoke(action);
		}
	}
}
