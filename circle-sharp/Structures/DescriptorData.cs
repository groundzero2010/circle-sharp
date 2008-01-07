using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    internal class DescriptorData
    {
        private TcpClient _connection;
        private string _hostname;
        private int _badPasswords;
        private int _idleTicks;
        private ConnectState _connectState;
        private DateTime _loginTime;
        private bool _hasPrompt = false;
        private char[] _rawInputBuffer = new char[0];
        private string _lastInput;
        private string _output;
        private Queue<string> _inputQueue = new Queue<string> ();
        private List<string> _history = new List<string> ();
        private DescriptorData _snooping;
        private DescriptorData _snoopBy;
        private CharacterData _character;
        private CharacterData _original;

//char *showstr_head;		/* for keeping track of an internal str	*/
//char **showstr_vector;	/* for paging through texts		*/
//int  showstr_count;		/* number of pages to page through	*/
//int  showstr_page;		/* which page are we currently showing?	*/
//char	**str;			/* for the modify-str system		*/
//char *backstr;		/* backup string for modify-str system	*/
//size_t max_str;	        /* maximum size of string in modify-str	*/
//long	mail_to;		/* name for mail system			*/
//char	inbuf[MAX_RAW_INPUT_LENGTH];  /* buffer for raw input		*/
//char	last_input[MAX_INPUT_LENGTH]; /* the last input			*/
//char small_outbuf[SMALL_BUFSIZE];  /* standard output buffer		*/
//char *output;		/* ptr to the current output buffer	*/
//int	history_pos;		/* Circular array position.		*/
//int  bufptr;			/* ptr to end of current output		*/
//int	bufspace;		/* space left in the output buffer	*/
//struct txt_block *large_outbuf; /* ptr to large buffer, if we need it */
//struct char_data *character;	/* linked to char			*/
//struct char_data *original;	/* original char if switched		*/
//void *olc;                    /* OLC data - see oasis.h              */

        public TcpClient Connection
        {
            get { return _connection; }
        }

        public string Hostname
        {
            get { return _hostname; }
            set { _hostname = value; }
        }

        public ConnectState ConnectState
        {
            get { return _connectState; }
            set { _connectState = value; }
        }

        public char[] RawInputBuffer
        {
            get { return _rawInputBuffer; }
            set { _rawInputBuffer = value; }
        }

        public int IdleTicks
        {
            get { return _idleTicks; }
            set { _idleTicks = value; }
        }

        public DateTime LoginTime
        {
            get { return _loginTime; }
            set { _loginTime = value; }
        }

        public bool HasPrompt
        {
            get { return _hasPrompt; }
            set { _hasPrompt = value; }
        }

        public int BadPasswords
        {
            get { return _badPasswords; }
            set { _badPasswords = value; }
        }

        public string LastInput
        {
            get { return _lastInput; }
            set { _lastInput = value; }
        }

        public string Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public Queue<string> InputQueue
        {
            get { return _inputQueue; }
        }

        public CharacterData Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public CharacterData Original
        {
            get { return _original; }
            set { _original = value; }
        }

        public DescriptorData(TcpClient connection)
        {
            _connection = connection;
        }
    }
}
