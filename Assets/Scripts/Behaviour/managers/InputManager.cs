using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        private float _lerpTime = 0.5f;
        private float _currentLerpTime;

        private Camera _camera;
        private Animator _animator;

        private float _currentSpeed;
        private bool _slowTime;
        private float _currentSize;

        private const string anim_release = "relase";
        private const string anim_press = "press";

        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
            _animator = GetComponent<Animator>();
            _camera = Camera.main;
            _currentSize = _camera.orthographicSize;
            _currentSpeed = Game.Speed;
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;

            _currentLerpTime += Time.deltaTime;
            if (_currentLerpTime > _lerpTime) {
                _currentLerpTime = _lerpTime;
            }
            Game.Speed = UpdateSpeedTime();
        }

        private float UpdateSpeedTime ( ) {
            var perc = _currentLerpTime / _lerpTime;
            float speed;
            if (_slowTime) {
                speed = Mathf.Lerp(_currentSpeed, Constans.MinGameTimeSpeed, perc);
                speed = speed < Constans.MinGameTimeSpeed ? Constans.MinGameTimeSpeed : speed;
                //_camera.orthographicSize = Mathf.Lerp(_currentSize, Constans.MinCamaraOrthographicSize, perc);
            } else {
                speed = Mathf.Lerp(Constans.MaxMaxTimeSpeed, _currentSpeed, perc);
                speed = speed > Constans.MaxMaxTimeSpeed ? Constans.MaxMaxTimeSpeed : speed;
            }
            _camera.orthographicSize = Mathf.Lerp(_currentSize,
                _slowTime ? Constans.MinCamaraOrthographicSize : Constans.MaxCamaraOrthographicSize, perc);
            return speed;
        }

        void OnMouseDown ( ) {
            SlowTime = true;
            _currentSize = _camera.orthographicSize;
            _animator.Play("press");
            Debug.Log("[input] mouseDown");
        }


        void OnMouseUp ( ) {
            SlowTime = false;
            EventManager.TriggerEvent(EventManagerType.CatchTime); //set an ivent //todo: delete
            _currentSize = _camera.orthographicSize;
            _animator.Play("release");
            Debug.Log("[input] mouseUp");
        }

        public bool SlowTime {
            get { return _slowTime; }
            private set {
                _slowTime = value;
                _currentLerpTime = 0;
            }
        }
    }

    public class Constans
    {
        public static float MaxCamaraOrthographicSize = 6f;
        public static float MinCamaraOrthographicSize = 5f;

        public static float MinGameTimeSpeed = 0f;
        public static float MaxMaxTimeSpeed = 2f;
    }
}