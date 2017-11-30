using System;
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

        public float MaxGameTimeSpeed = 3f;
        public float MinGameTimeSpeed = 0.05f;
        public float TimeMultiplier = 20f;
        public float LerpTime = 0.25f;
        public int RotatationLerpTime = 15;
        public Color SelectedColor = new Color(57, 160, 53, 255);
        public Color NormalColor = Color.white;

        public bool Stop { get; set; }
        public float TimeValue { get; set; }
        public float CaughtTime { get; set; }

        protected bool _selected = false;
        protected bool _slowTime = false;

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

        protected virtual void Init ( )
        {
            TimeValue = 0;
            CaughtTime = -1f; //todo: set proper float? 
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            CurrentSpeed = MaxGameTimeSpeed;
            Stop = false;
        }

        protected virtual void UpdateTime ( )
        {
            CurrentSpeed = UpdateSpeedTime();
            CurrentLerpTime += CurrentSpeed;
            if (CurrentLerpTime > LerpTime) {
                CurrentLerpTime = LerpTime;
            }
            UpdateTimeValue(CurrentSpeed * Time.deltaTime);
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