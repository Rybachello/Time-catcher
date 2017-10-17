using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        private const float MinTimeSpeed = 0.75f;
        private const float MaxTimeSpeed = 5f;

        [SerializeField] private float _lerpTime = 1.25f;
        private float _currentLerpTime;

        private Game _game;

        private float _currentSpeed;
        private bool _slowTime;

        private void Start ( ) {
            _game = Game.Instance;
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
                speed = Mathf.Lerp(_currentSpeed, MinTimeSpeed, perc);
                speed = speed < MinTimeSpeed ? MinTimeSpeed : speed;
                Camera.main.orthographicSize = Mathf.Lerp(5.5f, 5f, perc);

            } else {
                speed = Mathf.Lerp(MinTimeSpeed, _currentSpeed, perc);
                speed = speed > MaxTimeSpeed ? MaxTimeSpeed : speed;
                if(Camera.main.orthographicSize != 5.5f)
                Camera.main.orthographicSize = Mathf.Lerp(5f, 5.5f, perc);
            }
            return speed;
        }

        void OnMouseDown ( ) {
            SlowTime = true;
            Debug.Log("[input] mouseDown");
        }


        void OnMouseUp ( ) {
            SlowTime = false;
            _game.CatchTime();
            EventManager.TriggerEvent(EventManagerType.CatchTime); //set an ivent
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
}