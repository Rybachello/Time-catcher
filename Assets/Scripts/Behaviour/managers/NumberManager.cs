using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.managers;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class NumberManager : MonoBehaviour
    {
        public GameObject NumberPrefab;
        public GameObject ExplosionPrefab;

        public Sprite[] NumberSprites;
        private List<NumberBehaviour> _numbers = new List<NumberBehaviour>();

        private float SpawnNumberTime = 5;
        private float deltaTime = 4;
        private float MinNumberRange = 9;
        private float MaxNumberRange = 16;


        private static NumberManager _instance;


        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
        }
        
        private void OnEnable ( ) {
            EventManager.StartListening(EventManagerType.OnGameStart, StartSpawning);

            EventManager.StartListening(EventManagerType.CatchTime, CatchTime);
        }

        private void OnDisable ( ) {
            // EventManager.StopListening(EventManagerType.CatchTime, SpawnNumber);
            EventManager.StopListening(EventManagerType.CatchTime, CatchTime);
        }

        private void Update ( ) {
        }

        private void StartSpawning ( ) {
            float spawnTime = SpawnNumberTime + Random.Range(0, deltaTime);
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }

        void Spawn ( ) {
            //todo: remake spawn
            var hour = Random.Range(0, 11);
            var angle = hour * 360f / 12f;
            var sprite = NumberSprites[hour];
            var number = AddNumber(sprite);

            number.name = "number_" + hour;
            number.transform.parent = this.transform;
            number.transform.position =
                new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) *
                Random.Range(MinNumberRange, MaxNumberRange);
        }

        public NumberBehaviour AddNumber (Sprite sprite) {
            var numberGameObject = Instantiate(NumberPrefab);
            var number = numberGameObject.AddComponent<NumberBehaviour>();
            number.Init(sprite);
            _numbers.Add(number);
            return number;
        }

        public void DestroyNumber (NumberBehaviour numberBehaviour) {
            _numbers.Remove(numberBehaviour);
            Destroy(numberBehaviour.gameObject);
        }

        private static float GenerateNewTime ( ) {
            var hours = Random.Range(0, 11);
            var minutes = Random.Range(0, 59);
            var time = GetTime(hours, minutes);
            return time;
        }
     
        //convert to minutes
        private static float GetTime (float hours, float minutes) {
            //hours* Mathf.Pow(10, 2) + minutes * Mathf.Pow(10, 1);
            return hours * 60 + minutes;
        }

        public void CatchTime()
        {
            Debug.Log("[game] Catch Time");
            var currentHour = Game.Instance.ClockTime.Hours;
            Debug.Log("[game] Time: " + (currentHour / 60).ToString("##"));
            CheckTime(currentHour);
        }

        private void CheckTime(float currentTime)
        {
            var target = TargetTime;
            //var diff = Mathf.Abs(target.hour - currentTime);
            //if (diff > _deltaTime)
            //{
            //    todo: end game
            //     Game.OnGameEnd();
            //    return;
            //}

            //User.Score += 5;
            //if (diff < _deltaTime / 2)
            //{
            //    successful
            //    User.Score += 10;
            //}

            CreateFXExplosion(target.transform.position);
            DestroyNumber(target);

            //todo: mb restrict deltaTime
            //_targetTime = GenerateNewTime();
        }

        private void CreateFXExplosion (Vector3 numberPosition) {
            var explosionGameObject = Instantiate(ExplosionPrefab);
            explosionGameObject.transform.position = numberPosition;
            explosionGameObject.AddComponent<ExplosionBehaviour>();
        }

        public NumberBehaviour TargetTime {
            get {
                //todo: possible error
                return _numbers.FirstOrDefault();
            }
        }

        public static NumberManager Instance {
            get {
                if (_instance)
                    return _instance;
                else {
                    var dataGameOb = GameObject.Find("numbers");
                    if (dataGameOb) {
                        _instance = dataGameOb.GetComponent<NumberManager>();
                        return _instance;
                    }
                    var gameob = new GameObject();
                    gameob.name = "numbers";
                    _instance = gameob.AddComponent<NumberManager>();
                    return _instance;
                }
            }
        }
    }
}