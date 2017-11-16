using Assets.Scripts.Behaviour.managers;
using Assets.Scripts.Classes;
using UnityEngine;



namespace Assets.Scripts.Behaviour
{
    public class ArrowBehaviour : MonoBehaviour
    {
        public ArrowType ArrowType;

        public Color SelectedColor;
        public Color NormalColor;
        public float LerpTime = 0.25f;

        private bool _selected = false;

        public float RotatationLerpTime = 15;

        private bool _slowTime = false;

        private float _currentSpeed;
        private float _currentLerpTime;
        private bool _stop;

        private float _caughtTime = -1f;

        private Time _time;
        private SpriteRenderer _spriteRenderer;

        private void Start ( ) {
            Init();
        }

        private void Init ( ) {
            _time = new Time(ArrowType);
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _currentSpeed = Constans.MaxGameTimeSpeed;
            _stop = false;
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            if (_stop)
                return;
            UpdateTime();
            UpdateArrowMovement();
        }

        public void UpdateTime ( ) {
            _currentSpeed = UpdateSpeedTime();
            _currentLerpTime += _currentSpeed;
            if (_currentLerpTime > LerpTime) {
                _currentLerpTime = LerpTime;
            }
            _time.UpdateTimeValue(_currentSpeed * UnityEngine.Time.deltaTime);
        }

        private float UpdateSpeedTime ( ) {
            var perc = _currentLerpTime * UnityEngine.Time.deltaTime / LerpTime;
            float speed;
            if (_slowTime) {
                speed = Mathf.Lerp(_currentSpeed, Constans.MinGameTimeSpeed, perc);
                speed = speed < Constans.MinGameTimeSpeed ? Constans.MinGameTimeSpeed : speed;
            } else {
                speed = Mathf.Lerp(Constans.MaxGameTimeSpeed, _currentSpeed, perc);
                speed = speed > Constans.MaxGameTimeSpeed ? Constans.MaxGameTimeSpeed : speed;
            }
            return speed;
        }

        private void UpdateArrowMovement ( ) {
            var angle = GetAngle();
            transform.localRotation = Quaternion.Lerp(transform.localRotation, angle, RotatationLerpTime);
        }

        private Quaternion GetAngle ( ) {
            var c = float.NaN;
            switch (ArrowType) {
                case ArrowType.Hours:
                    c = Constans.HoursToDegrees;
                    break;
                case ArrowType.Minutes:
                    c = Constans.MinutesToDegrees;
                    break;
                default:
                    Debug.LogWarning("Cannot calculate angle");
                    break;
            }
            var delta = _time.TimeValue * c;
            return Quaternion.Euler(0, 0, delta);
        }

        public void SlowTime (bool slow) {
            _slowTime = slow;
            _currentLerpTime = 0;
        }

        public bool Stop {
            set { _stop = value; }
        }

        public float TimeValue {
            get { return _time.TimeValue; }
        }

        public float CaughtTime {
            get { return _caughtTime; }
            set { _caughtTime = value; }
        }

        public bool Selected {
            get { return _selected; }
            set {
                _selected = value;
                if (!_spriteRenderer)
                    Init();
                _spriteRenderer.color = _selected ? SelectedColor : NormalColor;
            }
        }

        public void CatchTime ( ) {
            _caughtTime = _time.TimeValue;
            _stop = true;
            
        }
    }

    public class Time
    {
        private float _timeValue;
        private float _timeMultiplier = 20f; //50
        private readonly ArrowType _arrowType;

        public Time (ArrowType arrowType) {
            _arrowType = arrowType;
            _timeValue = 0;
        }

        public void Reset ( ) {
            _timeValue = 0;
        }

        public void UpdateTimeValue (float delta) {
            TimeValue += delta * _timeMultiplier;
        }

        public float TimeValue {
            get { return _timeValue; }
            set {
                _timeValue = value;
                if (_arrowType == ArrowType.Hours && _timeValue > 12) {
                    _timeValue = 0;
                    return;
                }
                if (_arrowType == ArrowType.Minutes && _timeValue > 60) {
                    _timeValue = 0;
                    return;
                }
            }
        }
    }

    public enum ArrowType
    {
        Hours,
        Minutes,
    }
}