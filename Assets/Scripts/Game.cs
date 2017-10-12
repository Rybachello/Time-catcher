using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        private static Game _instance;
        private static bool _pause = false;

        private static float _gameSpeed = 1f;

        private void Awake ( ) {
            _instance = this;
        }

        private void Update ( ) {
            Time.timeScale = Speed;
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        private static void OnGameStart ( ) {
            //todo: implement here
        }

        private static void OnGameFinished ( ) {
            //todo: implement here
        }


        public static float Speed
        {
            get { return _gameSpeed; }
            set
            {
                _gameSpeed = value;

                if (!Game.Pause) Time.timeScale = value;
            }
        }



        public static bool Pause
        {
            get { return _pause; } 
            set
            {
                _pause = value;
            }
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