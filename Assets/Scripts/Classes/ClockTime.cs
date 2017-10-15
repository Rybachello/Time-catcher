namespace Assets.Scripts.Classes
{
    public class ClockTime
    {
        private float _hours;
        private float _minutes;
        private float _seconds;

        private float _timeMultiplier = 10f;

        public ClockTime ( ) {
            _hours = 0;
            _minutes = 0;
            _seconds = 0;
        }

        public ClockTime (int hours, int minutes, int second) {
            _hours = hours;
            _minutes = minutes;
            _seconds = second;
        }

        public void Reset ( ) {
            _hours = 0;
            _minutes = 0;
            _seconds = 0;
        }


        public void UpdateSeconds (float deltaSecond) {
            var seconds = deltaSecond * _timeMultiplier;
            Seconds += seconds;
        }

        public void UpdateMinutes (float deltaMinutes) {
            var minutes = deltaMinutes * _timeMultiplier;
            Minutes += minutes;
        }

        public float TimeMultiplier {
            set { _timeMultiplier = value; }
        }

        public float Seconds {
            get { return _seconds; }
            private set {
                _seconds = value;
                if (_seconds <= 60)
                    return;
                _seconds = 0;
                Minutes += 1f;
            }
        }

        public float Minutes {
            get { return _minutes; }
            private set {
                _minutes = value;
                if (_minutes <= 60)
                    return;
                _minutes = 0;
                Hours += 1f;
            }
        }

        public float Hours {
            get { return _hours; }
            private set {
                _hours = value;
                if (_hours <= 24)
                    return;
                _hours = 0;
            }
        }
    }
}