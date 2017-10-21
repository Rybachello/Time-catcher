using Assets.Scripts.Behaviour.managers;
using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class ArrowBehaviour : MonoBehaviour
    {
        public ArrowType ArrowType;

        private Game _game;
        [SerializeField] private float LerpTime = 5;
     

        private void Start ( ) {
            Init();
        }

        private void Init ( ) {
            _game = Game.Instance;
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            UpdateArrowMovement();
        }

        private void UpdateArrowMovement ( ) {
            var delta = GetAngle();
            var angle = Quaternion.Euler(0, 0, delta);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, angle, LerpTime);
        }

        private float GetAngle ( ) {
            switch (ArrowType) {
                case ArrowType.Hours:
                    return _game.ClockTime.Hours * Constans.HoursToDegrees;
                case ArrowType.Minutes:
                    return _game.ClockTime.Minutes * Constans.MinutesToDegrees;
            }
            Debug.LogWarning("Cannot calculate angle");
            return float.NaN;
        }
    }

    public enum ArrowType
    {
        Hours,
        Minutes,
    }
}