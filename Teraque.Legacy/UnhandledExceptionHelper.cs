using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teraque
{
	public static class UnhandledExceptionHelper
	{
		/// <summary>
		/// array of exception TickCounts so if get
		/// flooded with exceptions will shutdown the app instead of letting it 
		/// lockup
		/// </summary>
		private static int[] exceptionTicks = new int[exceptionTickArLength];
		private static int exceptionTickIndex = 0;

		private const int exceptionTickArLength = 100;
		private const int tickTimeMsecForAbort = 10000;

		/// <summary>
		/// Dispatcher ExceptionFilter handler
		/// will mark as handled and log to event log
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void Dispatcher_UnhandledExceptionFilter(object sender, System.Windows.Threading.DispatcherUnhandledExceptionFilterEventArgs e)
		{
			string errorString = "Dispatcher_UnhandledExceptionFilter ";
			try
			{
				errorString += e.Exception.ToString();
				Teraque.Log.Error("{0}", errorString);
			}
			catch
			{
				//log failed do not throw
			}
			System.Diagnostics.Debug.WriteLine("{0}", errorString);

			e.RequestCatch = ShouldMarkExceptionHandled();
		}


		/// <summary>
		/// Dispatcher ExceptionFilter handler
		/// will mark as handled and log to event log 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			string errorString = "Dispatcher_UnhandledException ";
			try
			{
				errorString += e.Exception.ToString();
				Teraque.Log.Error("{0}", errorString);
			}
			catch
			{
				//log failed do not throw
			}
			System.Diagnostics.Debug.WriteLine("{0}", errorString);
			e.Handled = ShouldMarkExceptionHandled();
		}

		/// <summary>
		/// appdomain event handler.. should not be called, but will log if we do get here
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			string errorString = "CurrentDomain_UnhandledException ";
			if (e.ExceptionObject != null)
			{
				errorString += e.ExceptionObject.ToString();
			}

			try
			{
				Teraque.Log.Error("{0}", errorString);
			}
			catch
			{
				//log failed do not throw
			}

			System.Diagnostics.Debug.WriteLine("{0}", errorString);
		}

		/// <summary>
		/// if getting flooded with exceptions let the applicaion fail
		/// </summary>
		/// <returns></returns>
		private static bool ShouldMarkExceptionHandled()
		{
			lock(exceptionTicks)
			{
				int index = exceptionTickIndex % exceptionTickArLength;
				exceptionTickIndex++;
				exceptionTicks[index] = Environment.TickCount;
				int minTick = 0;
				int maxTick = 0;
				foreach(int tick in exceptionTicks)
				{
					if (tick < minTick || minTick == 0)
					{
						minTick = tick;
					}
					else if (tick > maxTick)
					{
						maxTick = tick;
					}
				}

				if (maxTick != minTick &&
					maxTick - minTick < tickTimeMsecForAbort)
					return false;
			}

			return true;
		}

		/// <summary>
		/// get current stackTrace
		/// </summary>
		/// <returns></returns>
		public static string GetStackString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
			sb.Append("TickCount:");
			sb.AppendLine(Environment.TickCount.ToString());
			for(int i = 1; i < st.FrameCount; i++)
			{
				System.Diagnostics.StackFrame sf = st.GetFrame(i);
				var method = sf.GetMethod();
				sb.Append(method.DeclaringType);
				sb.Append(".");
				sb.Append(method.ToString());
				sb.Append(": FileName ");
				sb.Append(sf.GetFileName());
				sb.Append(" line: ");
				sb.Append(sf.GetFileLineNumber());
				sb.Append(" : ");
				sb.Append(sf.GetFileColumnNumber());
				sb.AppendLine();
			}

			return sb.ToString();
		}

		/// <summary>
		/// get all the info of an exception including the inner exceptions
		/// </summary>
		/// <param name="errorInfo">header string that is appended to give some custom info</param>
		/// <param name="ex">exception to get details for</param>
		/// <returns></returns>
		public static string GetExceptionTreeDetails(string errorInfo, Exception ex)
		{
			string exceptionTree = string.Concat(errorInfo, "\r\n", GetStackString(), "\r\n");
			try
			{
				Exception innerException = ex;
				while (innerException != null)
				{
					exceptionTree = string.Format("{0}\r\n----------------------\r\n{1}  Message: {2}\r\nToString:{3}\r\nStack{4}", exceptionTree, innerException.GetType().Name, innerException.Message, innerException.ToString(), innerException.StackTrace);
					innerException = innerException.InnerException;
				}
			}
			catch
			{
			}

			return exceptionTree;
		}
	}
}
