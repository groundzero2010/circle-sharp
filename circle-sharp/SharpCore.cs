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
    public partial class SharpCore
    {
		public static GlobalSettings GlobalSettings = new GlobalSettings ();

        public event EventHandler Started;
		public event EventHandler Stopped;

        private bool _running = false;

		private string _baseDirectory = String.Empty;

		public SharpCore ()
		{
			_baseDirectory = Path.Combine (Environment.CurrentDirectory, GlobalConstants.LIB_DIR);

			GlobalSettings.Load (_baseDirectory);
		}
		
		public void Start ()
		{
			if (Started != null)
				Started (this, new EventArgs ());
			
			InitializeGame ();
		}
		
		public void Stop ()
		{
			_running = false;
		}
    }
}
