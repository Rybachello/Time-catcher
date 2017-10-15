using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        private const float MinTimeSpeed = 0.75f;
        private const float MaxTimeSpeed = 5f;

        [SerializeField] private float _lerpTime;

        private Game _game;

        private float _currentSpeed;
        private float currentLerpTime;
        private bool _slowTime;
        private bool _speedUpTime;

        private void Start ( ) {
            _game = Game.Instance;
            _currentSpeed = Game.Speed;
        }

        private void Update ( ) {

            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > _lerpTime) {
                currentLerpTime = _lerpTime;
            }

          
            if (_slowTime) {
                float perc = currentLerpTime / _lerpTime;
                var speed  = Mathf.Lerp(_currentSpeed, MinTimeSpeed, perc);
                speed = speed < MinTimeSpeed ? MinTimeSpeed : speed;
                Game.Speed = speed;
                return;

            } 
            if( _speedUpTime)
            {
                float perc = currentLerpTime / _lerpTime;
                var speed = Mathf.Lerp(MinTimeSpeed, _currentSpeed, perc);
                speed = speed > MaxTimeSpeed ? MaxTimeSpeed : speed;
                Game.Speed = speed;
                return;
            }
        }
        //todo: refactor
        void OnMouseDown ( ) {
            _slowTime = true;
            _speedUpTime = false;
            currentLerpTime = 0f;
            Debug.Log("Down");
        }

        void OnMouseUp ( ) {
            _speedUpTime = true;
            _slowTime = false;
            currentLerpTime = 0f;
            Debug.Log("UP");
        }

        //private float SlowDownTime ( ) {
           

         
        //    speed = speed < MinTimeSpeed ? MinTimeSpeed : speed;
        //    return speed;
        //}

        private float SpeedUpTime ( ) {
            var speed = Mathf.Lerp(MinTimeSpeed, _currentSpeed, _lerpTime);
            return speed;
        }
    }
}