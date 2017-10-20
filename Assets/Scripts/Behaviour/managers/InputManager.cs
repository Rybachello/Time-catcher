using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        private const float LerpTime = 0.5f;
        private const string AnimRelease = "release";
        private const string AnimPress = "press";

        private Camera _camera;
        private Animator _animator;

        private float _currentLerpTime;
        private float _currentSpeed;
        private float _currentSize;
        private bool _slowTime;
        
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
            if (_currentLerpTime > LerpTime) {
                _currentLerpTime = LerpTime;
            }
            Game.Speed = UpdateSpeedTime();
        }

        private float UpdateSpeedTime ( ) {
            var perc = _currentLerpTime / LerpTime;
            float speed;
            if (_slowTime) {
                speed = Mathf.Lerp(_currentSpeed, Constans.MinGameTimeSpeed, perc);
                speed = speed < Constans.MinGameTimeSpeed ? Constans.MinGameTimeSpeed : speed;
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
            _animator.Play(AnimPress);
        }


        void OnMouseUp ( ) {
            SlowTime = false;
            EventManager.TriggerEvent(EventManagerType.CatchTime);
            _currentSize = _camera.orthographicSize;
            _animator.Play(AnimRelease);
        }

        public bool SlowTime {
            get { return _slowTime; }
            private set {
                _slowTime = value;
                _currentLerpTime = 0;
            }
        }
    }
}