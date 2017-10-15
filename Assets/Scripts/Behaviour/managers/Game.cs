using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Game : MonoBehaviour
    {
        private ClockTime _clockTime;

        private static bool _pause = false;
        private static bool _gameOver = false;
        private static float _gameSpeed = 1f;
        
        private static Game _instance;

        private void Awake ( ) {
            _instance = this;
            _clockTime = new ClockTime();
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        private void OnEnable ( ) {
            EventManager.StartListening(EventManagerType.OnGameStart, OnGameStart);
            EventManager.StartListening(EventManagerType.OnGameEnd, OnGameEnd);
        }

        private void OnDisable ( ) {
            EventManager.StopListening(EventManagerType.OnGameStart, OnGameStart);
            EventManager.StopListening(EventManagerType.OnGameEnd, OnGameEnd);
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            _clockTime.UpdateMinutes(Time.deltaTime * Speed);
        }

        private static void OnGameStart ( ) {
            //todo: generate target 
            //EventManager.TriggerEvent(
        }

        private static void OnGameEnd ( ) {
            //todo: implement here
        }

        public ClockTime ClockTime {
            get { return _clockTime; }
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
    }
}