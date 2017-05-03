using System;

namespace CatelDemo.Services.Other
{
    public class TimePickerHelper
    {
        int _currentHour;
        bool _isEnabled = true;



        public string GetFirstTime()
        {
            RefreshTime();
            if (_currentHour < 8)
            {
                _currentHour = 8;
            }

            return $"{_currentHour}:00";
        }

        public string GetLastTime()
        {
            if (_currentHour == 23)
            {
                _isEnabled = false;
                return $"{0}:00";
            }
            else if (_currentHour < 8)
            {
                return $"{9}:00";
            }
            return $"{_currentHour + 1}:00";
        }

        public bool IsEnabled()
        {
            return _isEnabled;
        }

        private void RefreshTime()
        {
            _currentHour = DateTime.Now.Hour;
        }
    }
}