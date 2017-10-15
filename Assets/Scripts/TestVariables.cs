using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestVariables : MonoBehaviour {
        
        public float GameSpeed = 1;
        public float ClockMultiplierSpeed = 1;

        private void Start()
        {
#if UNITY_EDITOR
            Game.Speed = GameSpeed;
            Game.Instance.ClockTime.TimeMultiplier = ClockMultiplierSpeed;
#endif
        }
    }
}
