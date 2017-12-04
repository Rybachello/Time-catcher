using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class MinuteArrowBehaviour : ArrowBehaviour
    {
        protected const float MinutesToDegrees = 360f / 60f;

        public override ArrowType GetArrowType ( )
        {
            return ArrowType.Minutes;
        }

        protected override void UpdateTimeValue (float delta)
        {
            TimeValue += delta * TimeMultiplier;
            if (TimeValue > 60) {
                TimeValue = 0;
            }
        }

        protected override Quaternion GetAngle ( )
        {
            var delta = TimeValue * MinutesToDegrees;
            return Quaternion.Euler(0, 0, delta);
        }
    }
}