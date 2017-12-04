using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        public static int PressedHash = Animator.StringToHash("Pressed");
        public Animator RightHandAnimator;

        public List<ArrowBehaviour> ArrowBehavioursList;
        private int _arrowCurrentIndex;

        private int _targetHour;
        private int _targetMinute;

        private Camera _camera;
        private float _currentCameraSize;

        protected const float MaxCamaraOrthographicSize = 8f;
        protected const float MinCamaraOrthographicSize = 5f;
        protected const float CameraSizingSpeed = 5f;

        private void Awake ( )
        {
            _camera = Camera.main;
            _currentCameraSize = _camera.orthographicSize;
        }

        private void Start ( )
        {
            GenerateTargetTime();
            ChangeArrow();
            UpdateTargetText();
            ResetCurrentTime();
        }

        private ArrowBehaviour ChangeArrow ( )
        {
            ArrowBehavioursList[_arrowCurrentIndex].Selected = false;
            _arrowCurrentIndex = (_arrowCurrentIndex + 1) % ArrowBehavioursList.Count;
            ArrowBehavioursList[_arrowCurrentIndex].Selected = true;
            return ArrowBehavioursList[_arrowCurrentIndex];
        }

        public void OnSlowTimeClickDown ( )
        {
            Debug.Log("[InputManager]: Slow Time Down");
            SlowArrow(true);
            _camera.orthographicSize = Mathf.Lerp(_currentCameraSize, MinCamaraOrthographicSize,
                CameraSizingSpeed * Time.deltaTime);
        }

        public void OnSlowTimeClickUp ( )
        {
            Debug.Log("[InputManager]: Slow Time UP");
            SlowArrow(false);
            Check();
            ChangeArrow();
            UpdateTargetText();
            _camera.orthographicSize = Mathf.Lerp(_currentCameraSize, MaxCamaraOrthographicSize,
                CameraSizingSpeed * Time.deltaTime);
        }

        private void SlowArrow (bool pressed)
        {
            var current = ArrowBehavioursList[_arrowCurrentIndex];
            current.SlowTime(pressed);
            RightHandAnimator.SetBool(PressedHash, pressed);
        }

        private void Check ( )
        {
            var current = ArrowBehavioursList[_arrowCurrentIndex];
            current.CatchTime();

            var hour = GetArrowCaughtTime(ArrowBehaviour.ArrowType.Hours);
            var minute = GetArrowCaughtTime(ArrowBehaviour.ArrowType.Minutes);

            if (Mathf.Approximately(hour, -1f) || Mathf.Approximately(minute, -1f))
                return;

            Debug.Log("[input manager] Check time::" + hour + "h " + minute + "m");
            Debug.Log("Hour diff:" + Mathf.Abs(hour - _targetHour));

            CalculateUserScore(hour, minute);
            GenerateTargetTime();
            ResetCurrentTime();
        }

        private void CalculateUserScore (float hour, float minute)
        {
            var score = 0;
            var bonusTime = 0;
            var hourDiff = Mathf.Abs(hour - _targetHour) < 0.5 || Mathf.Abs(hour - _targetHour) > 11.5;
            var minuteDiff = Mathf.Abs(minute - _targetMinute) < 5 || Mathf.Abs(minute - _targetMinute) > 55;

            if (hourDiff && minuteDiff) {
                score = 20;
                bonusTime = 20;
                InGame.Instance.ShowCongartsText(bonusTime);
            } else if (hourDiff) {
                score = 5;
                bonusTime = 0;
            } else if (minuteDiff) {
                score = 10;
                bonusTime = 0;
            }
            Game.TimeLeft += bonusTime;
            User.Score += score;
        }

        private void ResetCurrentTime ( )
        {
            ArrowBehavioursList.ForEach(x => { x.ResetCaughtTime(); });
        }

        private float GetArrowCaughtTime (ArrowBehaviour.ArrowType type)
        {
            var arrow = ArrowBehavioursList.First(x => x.GetArrowType() == type);
            return arrow.GetCaughtTime;
        }

        private void GenerateTargetTime ( )
        {
            _targetHour = Random.Range(0, 12);
            _targetMinute = Random.Range(0, 60);
        }

        private void UpdateTargetText ( )
        {
            var inGame = InGame.Instance;
            var selectedType = ArrowBehavioursList[_arrowCurrentIndex].GetArrowType();
            inGame.UpdateTargetText(_targetHour, _targetMinute, selectedType);
        }
    }
}