using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using SharpMUD.Structures;
using SharpMUD.Enumerations;

namespace SharpMUD
{
    public partial class SharpCore
    {
        public event EventHandler Started;
		public event EventHandler Stopped;

        private bool _running = false;

		public SharpCore ()
		{
			
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