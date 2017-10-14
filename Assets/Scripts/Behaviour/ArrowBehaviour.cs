using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArrowBehaviour : MonoBehaviour
    {
        public ArrowType ArrowType;

        private Game _game;
        [SerializeField] private float LerpTime = 5;
        private const float hoursToDegrees = 360f / 12f;
        private const float minutesToDegrees = 360f / 60f;
        private const float secondsToDegrees = 360f / 60f;

        private void Start ( ) {
            Init();
        }

        private void Init ( ) {
            _game = Game.Instance;
        }

        private void Update ( ) {
            if (Game.Pause)
                return;
            UpdateArrowMovement();
        }

        private void UpdateArrowMovement ( ) {
            var delta = GetAngle();
            var angle = Quaternion.Euler(0, 0, delta);
            transform.localRotation = Quaternion.Lerp(transform.localRotation,angle,LerpTime);
        }

        private float GetAngle ( ) {
            switch (ArrowType) {
                case ArrowType.Hours:
                    return _game.ClockTime.Hours * -hoursToDegrees;
                case ArrowType.Minutes:
                    return _game.ClockTime.Minutes * -minutesToDegrees;
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