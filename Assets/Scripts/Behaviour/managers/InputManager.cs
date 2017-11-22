using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        public static int PressedHash = Animator.StringToHash("Pressed");
        public Animator RightHandAnimator;

        //todo:refactor
        public ArrowBehaviour MinuteArrow;
        public ArrowBehaviour HourArrow;
        private ArrowBehaviour _current;

        private int _targetHour;
        private int _targetMinute;

        private Camera _camera;
        private float _currentCameraSize;

        protected const float MaxCamaraOrthographicSize = 8f;
        protected const float MinCamaraOrthographicSize = 5f;
        protected const float CameraSizingSpeed = 5f;


        private void Awake ( ) {
            _camera = Camera.main;
            _currentCameraSize = _camera.orthographicSize;
            Current = HourArrow;
        }

        private void Start ( ) {
            GenerateTargetTime();
            ResetCurrentTime();
        }

        private void ChangeArrow ( ) {
            //todo: refactor 
            _current.Selected = false;
            Current = _current.ArrowType == ArrowType.Minutes
                ? HourArrow
                : MinuteArrow;
        }

        public void OnSlowTimeClickDown ( ) {
            Debug.Log("[InputManager]: Slow Time Down");
            SlowArrow(_current, true);
            _camera.orthographicSize = Mathf.Lerp(_currentCameraSize, MinCamaraOrthographicSize, CameraSizingSpeed * UnityEngine.Time.deltaTime);
        }

        public void OnSlowTimeClickUp ( ) {
            Debug.Log("[InputManager]: Slow Time UP");
            SlowArrow(_current, false);
            _current.CatchTime();
            Check();
            ChangeArrow();
            _camera.orthographicSize = Mathf.Lerp(_currentCameraSize, MaxCamaraOrthographicSize, CameraSizingSpeed * UnityEngine.Time.deltaTime);
        }

        private void SlowArrow (ArrowBehaviour current, bool pressed) {
            current.SlowTime(pressed);
            RightHandAnimator.SetBool(PressedHash, pressed);
        }

        private void Check ( ) {
            var hour = HourArrow.CaughtTime;
            var minute = MinuteArrow.CaughtTime;

            if(Mathf.Approximately(hour,-1f) || Mathf.Approximately(minute, -1f))
                return;

            Debug.Log("[input manager] Check time::" + HourArrow.CaughtTime + "h " + MinuteArrow.CaughtTime + "m");
            Debug.Log("Hour diff:" + Mathf.Abs(hour - _targetHour));


            var score = 0;
            var bonusTime = 0;
            var hourDiff = Mathf.Abs(hour - _targetHour);
            var minuteDiff = Mathf.Abs(minute - _targetMinute);

            if (hourDiff < 0.5 && minuteDiff < 5) {
                score = 20;
                bonusTime = 20;
                InGame.Instance.ShowCongartsText(bonusTime);
            } else if (hourDiff < 0.8) {
                score = 5;
                bonusTime = 0;
            } else if (minuteDiff < 8) {
                score = 10;
                bonusTime = 0;
            }
            Game.TimeLeft += bonusTime;
            User.Score += score;
            GenerateTargetTime();
            ResetCurrentTime();
            MinuteArrow.Stop = HourArrow.Stop = false;
        }

        private void ResetCurrentTime ( ) {
            HourArrow.CaughtTime = -1;
            MinuteArrow.CaughtTime = -1;
        }

        private void GenerateTargetTime ( ) {
            _targetHour = Random.Range(0, 12);
            _targetMinute = Random.Range(0, 60);
            InGame.Instance.UpdateTargetText(_targetHour, _targetMinute);
        }

        public ArrowBehaviour Current {
            get { return _current.ArrowType == ArrowType.Hours ? HourArrow : MinuteArrow; }
            set {
                _current = value;
                _current.Selected = true;
            }
        }
    }
}