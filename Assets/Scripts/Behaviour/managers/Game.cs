using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour.managers
{
    public class Game : MonoBehaviour
    {
        private ClockTime _clockTime;
        
        private static bool _pause = false;
        private static bool _gameOver = false;
        private static float _gameSpeed = 2f;


        private static Game _instance;

        private void Awake ( ) {
            Init();
        }

        private void Start ( ) {
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

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            _clockTime.UpdateHours(Time.deltaTime * Speed);
        }

        public void OnGameStart ( ) {
            Debug.Log("[game] Game Started");
            EventManager.TriggerEvent(EventManagerType.OnGameStart);
        }

        public static void OnGameEnd ( ) {
            Debug.Log("[game] Game Over");
            EventManager.TriggerEvent(EventManagerType.OnGameEnd);
            //GameOver = true; //todo: rename
        }

        #region  properties

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

        #endregion
    }
}