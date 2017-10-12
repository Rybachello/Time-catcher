using UnityEngine;

namespace Assets.Scripts
{
    public class ArrowBehaviour : MonoBehaviour
    {
        private Game _game;

        [SerializeField] private float _timeRotation = 10f;
        [SerializeField] private float _deltaRotation = 15f;
        [SerializeField] private float Speed = 1;

        private const float hoursToDegrees = 360f / 12f;
        private const float minutesToDegrees = 360f / 60f;
        private const float secondsToDegrees = 360f / 60f;

        private void Start ( ) {
            Init();
        }

        private void Init ( ) {
            //todo: implement refences
            _game = Game.Instance;
        }

        private void Update ( ) {
            if(Game.Pause) return;
            UpdateArrowMovement();
        }

        private void UpdateArrowMovement ( ) {
            var deltaAngle = Quaternion.Euler(0f, 0f, -_deltaRotation);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, transform.localRotation * deltaAngle,
                _timeRotation * Time.deltaTime);
        }

        //todo: create time behaviour that increments by delta time multiply by some amount
    }
}