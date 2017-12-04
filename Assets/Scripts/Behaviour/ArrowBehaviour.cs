using Assets.Scripts.Behaviour.managers;
using UnityEngine;


namespace Assets.Scripts.Behaviour
{
    public abstract class ArrowBehaviour : MonoBehaviour
    {
        public enum ArrowType
        {
            Hours,
            Minutes
        }

        //constants
        public float MaxGameTimeSpeed = 3f;

        public float MinGameTimeSpeed = 0.05f;
        public float TimeMultiplier = 20f;
        public float LerpTime = 0.25f;
        public int RotatationLerpTime = 15;
        public Color SelectedColor = new Color(57, 160, 53, 255);
        public Color NormalColor = Color.white;

        protected float TimeValue { get; set; }
        protected bool Stop { get; set; }

        public float GetCaughtTime {
            get { return CaughtTime; }
        }

        protected bool _selected = false;
        protected bool _slowTime = false;

        protected float CaughtTime;
        protected float CurrentLerpTime;

        protected SpriteRenderer SpriteRenderer;
        protected float CurrentSpeed;

        public abstract ArrowType GetArrowType ( );
        protected abstract Quaternion GetAngle ( );
        protected abstract void UpdateTimeValue (float delta);

        public void CatchTime ( )
        {
            CaughtTime = TimeValue;
            Stop = true;
        }

        public void SlowTime (bool slow)
        {
            _slowTime = slow;
            CurrentLerpTime = 0;
        }

        public bool Selected {
            get { return _selected; }
            set {
                _selected = value;
                if (!SpriteRenderer)
                    Init();
                SpriteRenderer.color = _selected ? SelectedColor : NormalColor;
            }
        }

        public void ResetCaughtTime ( )
        {
            CaughtTime = -1;
            Stop = false;
        }
        
        private void Awake ( )
        {
            Init();
        }

        protected virtual void Init ( )
        {
            TimeValue = 0;
            CaughtTime = -1f;
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            CurrentSpeed = MaxGameTimeSpeed;
            Stop = false;
        }

        private void Update ( )
        {
            if (Game.Pause || Game.GameOver || Stop)
                return;
            CurrentSpeed = UpdateTime();
            UpdateTimeValue(CurrentSpeed * Time.deltaTime);
            UpdateArrowMovement();
        }

        protected virtual float UpdateTime ( )
        {
            var speed = UpdateSpeedTime();
            CurrentLerpTime += speed;
            if (CurrentLerpTime > LerpTime) {
                CurrentLerpTime = LerpTime;
            }
            return speed;
        }

        protected virtual float UpdateSpeedTime ( )
        {
            var perc = CurrentLerpTime * Time.deltaTime / LerpTime;
            float speed;
            if (_slowTime) {
                speed = Mathf.Lerp(CurrentSpeed, MinGameTimeSpeed, perc);
                speed = speed < MinGameTimeSpeed ? MinGameTimeSpeed : speed;
            } else {
                speed = Mathf.Lerp(MaxGameTimeSpeed, CurrentSpeed, perc);
                speed = speed > MaxGameTimeSpeed ? MaxGameTimeSpeed : speed;
            }
            return speed;
        }

        protected void UpdateArrowMovement ( )
        {
            var angle = GetAngle();
            transform.localRotation = Quaternion.Lerp(transform.localRotation, angle, RotatationLerpTime);
        }
    }
}