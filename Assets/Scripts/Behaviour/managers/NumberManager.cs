using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.managers;
using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class NumberManager : MonoBehaviour
    {
        public GameObject NumberPrefab; //reference for number gameobject
        public GameObject ExplosionPrefab; //reference for explosion gameobject 
        public GameObject LimitPrefab; //reference for limitPrefab
        public Sprite[] NumberSprites;

        private readonly List<NumberBehaviour> _numbers = new List<NumberBehaviour>();

        private static NumberManager _instance;

        private float Timer;
        [SerializeField] private GameObject _leftLimit;
        [SerializeField] private GameObject _rightLimit;
        private float _deltaTime = 1f;

        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
            Timer = Time.time + Constans.SpawnNumberTime + Random.Range(0, Constans.SpawnNumberDeltaTime);
        }

        private void OnEnable ( ) {
            EventManager.StartListening(EventManagerType.CatchTime, CatchTime);
        }

        private void OnDisable ( ) {
            EventManager.StopListening(EventManagerType.CatchTime, CatchTime);
        }

        private void Update ( ) {
            if (Game.Pause || Game.GameOver)
                return;
            if (Timer < Time.time) {
                Spawn();
                Timer = Time.time + Constans.SpawnNumberTime + Random.Range(0, Constans.SpawnNumberDeltaTime);
            }
            UpdateLimitsPostion();
        }

        private void OnDestroy ( ) {
            _instance = null;
        }
        private void UpdateLimitsPostion ( ) {
            var target = TargetTime;
            if (!target)
                return;
            var hour = target.Hour;
            
            _leftLimit.gameObject.SetActive(true);
            _rightLimit.gameObject.SetActive(true);
            _leftLimit.transform.localRotation = Quaternion.Euler(0, 0, (hour + 1) *- Constans.HoursToDegrees);
            _rightLimit.transform.localRotation = Quaternion.Euler(0, 0, (hour - 1) *- Constans.HoursToDegrees);
        }

        void Spawn ( ) {
            var hour = Random.Range(0, 11);
            var minutes = Random.Range(0, 59);

            var angle = hour * 360f / 12f;
            var sprite = NumberSprites[hour];
            var number = AddNumber(sprite);
            number.Hour = hour;
            number.Minutes = minutes;

            number.name = "number_" + hour;
            number.transform.parent = this.transform;
            var r = Random.Range(Constans.MinNumberRange, Constans.MaxNumberRange);
            number.transform.position =
                new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * r;
        }

        public NumberBehaviour AddNumber (Sprite sprite) {
            var numberGameObject = Instantiate(NumberPrefab);
            var number = numberGameObject.GetComponent<NumberBehaviour>();
            number.Init(sprite);
            _numbers.Add(number);
            return number;
        }

        public void DestroyNumber (NumberBehaviour numberBehaviour) {
            _numbers.Remove(numberBehaviour);
            Destroy(numberBehaviour.gameObject);
        }

        //convert to minutes
        private static float GetTime (float hours, float minutes) {
            return hours * 60 + minutes;
        }

        public void CatchTime ( ) {
            Debug.Log("[game] Catch Time");
            var clockTime = Game.Instance.ClockTime;
            CheckTime(clockTime.Hours);
        }

        private void CheckTime (float currentTime) {
            //todo: need to finish here
            var target = TargetTime;
            if(!target) return;
            var diff = Mathf.Abs(target.Hour - (int)currentTime);
            
            if (diff > _deltaTime)
            {
               Game.OnGameEnd();
               return;
            }

            User.Score += 5;
            if (diff < _deltaTime / 2)
            {
                User.Score += 10;
            }

            CreateFXExplosion(target.transform.position);
            DestroyNumber(target);
        }

        private void CreateFXExplosion (Vector3 numberPosition) {
            var explosionGameObject = Instantiate(ExplosionPrefab);
            explosionGameObject.transform.position = numberPosition;
            explosionGameObject.AddComponent<ExplosionBehaviour>();
        }

        public NumberBehaviour TargetTime {
            get {
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