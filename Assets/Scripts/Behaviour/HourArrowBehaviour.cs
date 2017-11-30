using Assets.Scripts.Behaviour.managers;
using UnityEngine;

namespace Assets.Scripts.Behaviour {
    public class HourArrowBehaviour : ArrowBehaviour {
        protected const float HoursToDegrees = 360f / 12f;

        public override ArrowType GetArrowType ( )
        {
            return ArrowType.Hours;
        }
      
        protected override Quaternion GetAngle ( )
        {
            var delta = TimeValue * HoursToDegrees;
            return Quaternion.Euler(0, 0, delta);
        }

        protected override void UpdateTimeValue (float delta)
        {
            TimeValue += delta * TimeMultiplier;
            if (TimeValue > 12) {
                TimeValue = 0;
            }
        }

        private void Awake ( )
        {
            Init();
        }

        private void Update ( )
        {
            if (Game.Pause || Game.GameOver || Stop)
                return;
            UpdateTime();
            UpdateArrowMovement();
        }
    }
}
