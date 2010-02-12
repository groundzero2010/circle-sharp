using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public delegate void LogEventHandler (object sender, string text);

    public partial class CircleCore
    {
		public static GlobalSettings GlobalSettings = new GlobalSettings ();

        public event EventHandler Started;
		public event EventHandler Stopped;
		public event LogEventHandler Logged;

        private bool _running = false;

		private string _baseDirectory = String.Empty;

		public CircleCore ()
		{
			_baseDirectory = Path.Combine (Environment.CurrentDirectory, GlobalConstants.LIB_DIR);

			GlobalSettings.Load (_baseDirectory);
		}

		private void Log(string text)
		{
			if (Logged != null)
				Logged (this, text);
		}

		public void Start ()
		{
			if (Started != null)
				Started (this, new EventArgs ());
			
			InitializeGame ();
		}
		
		public void Stop ()
		{
			_listener.Stop ();
			_running = false;
		}
    }
}
