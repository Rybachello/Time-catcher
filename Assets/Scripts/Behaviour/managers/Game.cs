using Assets.Scripts.Classes;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Assets.Scripts.Behaviour.managers
{
    public class Game : MonoBehaviour
    {
        private static bool _pause = false;
        private static bool _gameOver = false;
        private static float _gameSpeed = 1f;

        private static Game _instance;
        private static float _timeleft = 90f;


        private void Awake ( ) {
            Init();
        }

        private void Start ( ) {
#if UNITY_ANDROID
            Screen.orientation = ScreenOrientation.Portrait;
#endif
            OnGameStart();
            UnityEngine.Time.timeScale = _gameSpeed;
            _timeleft = 90f;
        }

        private void Init ( ) {
            Instance = this;

            _pause = false;
            _gameOver = false;
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        private void Update ( ) {
            if (Pause || GameOver)
                return;
            _timeleft -= UnityEngine.Time.deltaTime;
            if (_timeleft < 0) {
                _timeleft = 0f;
                OnGameEnd();
            }
        }

        public void OnGameStart ( ) {
            Debug.Log("[game] Game Started");
            User.Reset();
            EventManager.TriggerEvent(EventManagerType.OnGameStart);
        }

        public static void OnGameEnd ( ) {
            Debug.Log("[game] Game Over");
            EventManager.TriggerEvent(EventManagerType.OnGameEnd);
            GameOver = true; 
        }

#region  properties

        public static float Speed {
            get { return _gameSpeed; }
            set {
                _gameSpeed = value;

                if (!Game.Pause)
                    UnityEngine.Time.timeScale = value;
            }
        }

        public static float TimeLeft {
            get { return _timeleft; }
            set { _timeleft = value; }
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
            private set { _instance = value; }
        }
    }

#endregion
}