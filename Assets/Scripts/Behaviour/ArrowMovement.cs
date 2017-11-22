using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class ArrowMovement : MonoBehaviour {
        [SerializeField] private float _deltaRotation = 15f;
        [SerializeField] private float _timeRotation = 10f;

        void Update () {
            UpdateArrowMovement();
        }

        private void UpdateArrowMovement()
        {
            var deltaAngle = Quaternion.Euler(0f, 0f, -_deltaRotation);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, transform.localRotation * deltaAngle,
                _timeRotation * UnityEngine.Time.deltaTime);
        }
    }
}
