using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Structures
{
    class TimeInfoData
    {
        private long _hours;
        private long _day;
        private long _month;
        private long _year;

        public long Hours
        {
            get { return _hours; }
            set { _hours = value; }
        }

        public long Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public long Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public long Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public TimeInfoData(long hours, long day, long month, long year)
        {
            _hours = hours;
            _day = day;
            _month = month;
            _year = year;
        }
    }
}
