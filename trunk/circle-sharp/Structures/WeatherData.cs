using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    internal class WeatherData
    {
        int _pressure;
        int _change;
        SkyState _sky;
        SunState _sun;

        public int Pressure
        {
            get { return _pressure; }
            set { _pressure = value; }
        }

        public int Change
        {
            get { return _change; }
            set { _change = value; }
        }

        public SkyState Sky
        {
            get { return _sky; }
            set { _sky = value; }
        }

        public SunState Sun
        {
            get { return _sun; }
            set { _sun = value; }
        }
    }
}
