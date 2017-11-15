using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour.managers
{
    public class InputManager : MonoBehaviour
    {
        //todo:move to anim script
        private const string AnimRelease = "release";

        private const string AnimPress = "press";
        public Animator _animator;

        public ArrowBehaviour _minuteArrow;
        public ArrowBehaviour _hourArrow;
        private ArrowBehaviour _current;

        private int _targetHour;
        private int _targetMinute;

        private Camera _camera;
        private float _currentSize;

        private void Start ( ) {
            Current = _hourArrow;
            _camera = Camera.main;
            _currentSize = _camera.orthographicSize;

            GenerateTargetTime();
            ResetCurrentTime();
        }

        private void ChangeArrow ( ) {
            //todo: refactor 
            _current.Selected = false;
            Current = _current.ArrowType == ArrowType.Minutes
                ? _hourArrow
                : _minuteArrow;
        }

        public void OnSlowTimeClickDown ( ) {
            SlowArrow(true);
            Debug.Log("Slow Time Down");
            _camera.orthographicSize = Mathf.Lerp(_currentSize, Constans.MinCamaraOrthographicSize, 0.15f);
        }

        public void OnSlowTimeClickUp ( ) {
            SlowArrow(false);
            _current.CatchTime();
            Check();
            ChangeArrow();
            _camera.orthographicSize = Mathf.Lerp(_currentSize, Constans.MaxCamaraOrthographicSize, 0.15f);
            //check time here
            Debug.Log("Slow Time UP");
        }

        private void SlowArrow (bool b) {
            _current.SlowTime(b);
            _animator.Play(b ? AnimPress : AnimRelease);
        }

        private void Check ( ) {
            var hour = _hourArrow.CaughtTime;
            var minute = _minuteArrow.CaughtTime;

            if (hour == -1f || minute == -1f)
                return;

            Debug.Log("[input manager] Check time::" + _hourArrow.CaughtTime + "h " + _minuteArrow.CaughtTime + "m");

            if (Mathf.Abs(hour - _targetHour) < 0.5) {
                Game.TimeLeft += 2;
            } else {
                Game.TimeLeft -= 15;
            }

            if (Mathf.Abs(minute - _targetMinute) < 5) {
                Game.TimeLeft += 3;
            } else {
                Game.TimeLeft -= 5;
            }

            User.Score += 15;

            GenerateTargetTime();
            ResetCurrentTime();
            _minuteArrow.Stop = _hourArrow.Stop = false;
        }

        private void ResetCurrentTime ( ) {
            _hourArrow.CaughtTime = -1;
            _minuteArrow.CaughtTime = -1;
        }

        private void GenerateTargetTime ( ) {
            _targetHour = Random.Range(0, 12);
            _targetMinute = Random.Range(0, 60);
            InGame.Instance.UpdateTargetText(_targetHour, _targetMinute);
        }

        public ArrowBehaviour Current {
            get { return _current.ArrowType == ArrowType.Hours ? _hourArrow : _minuteArrow; }
            set {
                _current = value;
                _current.Selected = true;
            }
        }
    }
}