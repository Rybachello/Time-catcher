using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class ExplosionBehaviour : MonoBehaviour
    {
        private float _speed = 0.5f;

        private Transform _clockCenter;

        private void Awake ( ) {
            _clockCenter = GameObject.Find("clock").transform;
        }

        //private void Update ( ) {
        //    transform.position =
        //        Vector3.Lerp(transform.position, _clockCenter.transform.position, _speed * Time.deltaTime);
        //}

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag != "clock")
                return;
            Destroy(this.gameObject);
        }
    }
}