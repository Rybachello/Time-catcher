using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class Game : MonoBehaviour
    {
        private ClockTime _clockTime;

        //todo mb move to another class
        private float _targetTime; // time once you pressed in minutes

        private float _deltaTime = 60; // upper/lower limit 

        private static bool _pause = false;
        private static bool _gameOver = false;
        private static float _gameSpeed = 2f;

        private static Game _instance;

        private void Awake ( ) {
            Init();
            OnGameStart();
        }

        private void Init ( ) {
            _instance = this;
            _clockTime = new ClockTime();

            _pause = false;
            _gameOver = false;
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        //private void OnEnable ( ) {
        //    EventManager.StartListening(EventManagerType.OnGameStart, OnGameStart);
        //    EventManager.StartListening(EventManagerType.OnGameEnd, OnGameEnd);
        //}

        //private void OnDisable ( ) {
        //    EventManager.StopListening(EventManagerType.OnGameStart, OnGameStart);
        //    EventManager.StopListening(EventManagerType.OnGameEnd, OnGameEnd);
        //}

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            _clockTime.UpdateMinutes(Time.deltaTime * Speed);
        }

        public void OnGameStart ( ) {
            Debug.Log("[game] Game Started");
            EventManager.TriggerEvent(EventManagerType.OnGameStart);
            _targetTime = GenerateNewTime();
            Debug.Log("[game] Time: " + (_targetTime / 60).ToString("##") + ":" + (_targetTime % 60).ToString("##"));
        }

        public static void OnGameEnd ( ) {
            Debug.Log("[game] Game Over");
            EventManager.TriggerEvent(EventManagerType.OnGameEnd);
            GameOver = true;
        }


        public void CatchTime ( ) {
            Debug.Log("[game] Catch Time");
            EventManager.TriggerEvent(EventManagerType.CatchTime);
            var currentTime = GetTime(_clockTime.Hours, _clockTime.Hours);
            Debug.Log("[game] Time: " + (currentTime / 60).ToString("##") + ":" + (currentTime % 60).ToString("##"));
            CheckTime(currentTime);
        }

        private static float GenerateNewTime ( ) {
            var hours = Random.Range(0, 12);
            var minutes = Random.Range(0, 60);
            var time = GetTime(hours, minutes);

            return time;
        }

        //convert to minutes
        private static float GetTime (float hours, float minutes) {
            //hours* Mathf.Pow(10, 2) + minutes * Mathf.Pow(10, 1);
            return hours * 60 + minutes;
        }

        private void CheckTime (float currentTime) {
            if (Mathf.Abs(_targetTime - currentTime) < _deltaTime) {
                //successful
                User.Score += 5;
                _targetTime = GenerateNewTime();
                //todo: mb restrict deltaTime
            } else {
                Debug.Log("[game] you loose");
                Game.OnGameEnd();
            }
        }

        #region  properties

        public ClockTime ClockTime {
            get { return _clockTime; }
        }

        public float TargetTime {
            get { return _targetTime; }
        }

        public static float Speed {
            get { return _gameSpeed; }
            set {
                _gameSpeed = value;

                if (!Game.Pause)
                    Time.timeScale = value;
            }
        }

        public static bool Pause {
            get { return _pause; }
            set { _pause = value; }
        }

        public static bool GameOver {
            get { return _gameOver; }
            set { _gameOver = value; }
        }

        public static Game Instance {
            get {
                if (_instance)
                    return _instance;
                else {
                    var dataGameOb = GameObject.Find("game manager");
                    if (dataGameOb) {
                        _instance = dataGameOb.GetComponent<Game>();
                        return _instance;
                    }
                    var gameob = new GameObject();
                    gameob.name = "game manager";
                    _instance = gameob.AddComponent<Game>();
                    return _instance;
                }
            }
        }

        #endregion
    }
}