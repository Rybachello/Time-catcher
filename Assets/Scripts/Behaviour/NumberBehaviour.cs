using Assets.Scripts.Behaviour.managers;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class NumberBehaviour : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Transform _clockCenter;
        private readonly float _speed = 0.15f;

        private void Awake ( ) {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _clockCenter = GameObject.Find("clock").transform;
        }

        public void Init (Sprite sprite) {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.sortingOrder = 10;
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            transform.position =
                Vector3.Lerp(transform.position, _clockCenter.transform.position, _speed * Time.deltaTime);
        }

        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag != "clock")
                return;
            NumberManager.Instance.DestroyNumber(this);
            Game.OnGameEnd();
        }

        public int Hour { get; set; }
        public int Minutes { get; set; }
    }
}