﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        private static Game _instance;
        private static bool _pause = false;

        [SerializeField] private static float _gameSpeed = 1f;

        private ClockTime _clockTime;

        private void Awake ( ) {
            _instance = this;
            _clockTime = new ClockTime();
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        private void Update ( ) {
            if (Game.Pause)
                return;

            Time.timeScale = Speed;
            _clockTime.UpdateSeconds(Time.deltaTime * _gameSpeed);
            Debug.Log(_clockTime.Hours + ":" + _clockTime.Minutes + ":" + _clockTime.Seconds.ToString("#.##"));
        }


        private static void OnGameStart ( ) {
            //todo: implement here
        }

        private static void OnGameFinished ( ) {
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