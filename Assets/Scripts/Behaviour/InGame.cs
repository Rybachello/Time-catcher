﻿using System;
using Assets.Scripts.Behaviour.managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts.Behaviour
{
    public class InGame : MonoBehaviour
    {
        [Header("Panels")] public GameObject GamePausePanel;
        public GameObject InGamePanel;
        public GameObject GameOverPanel;

        [Header("Texts")] [SerializeField] private Text _userScore;
        [SerializeField] private Text _timeLeftText;
        [SerializeField] private Text _targetHourText;
        [SerializeField] private Text _targetMinuteText;
        [SerializeField] private Text _congratsText;
        [SerializeField] private Text _countDownText;
        private RectTransform _countdownRectTransform;

        private static InGame _instance;

        protected readonly string[] CongratStrings = {"Awesome!", "Nice!", "Impressive!", "Wonderful!"};
        protected const string MainMenuSceneName = "menu";
        protected const string GameSceneName = "game";
        public Color SelectedColor = new Color(57, 160, 53, 255);
        public Color NormalColor = new Color(87, 59, 84, 255);

        private void Awake ( )
        {
            Init();
        }

        private void Init ( )
        {
            //get ref
            _instance = this;
            //panel
            GamePausePanel = GamePausePanel ?? gameObject.transform.Find("GamePausePanel").gameObject;
            InGamePanel = InGamePanel ?? gameObject.transform.Find("InGamePanel").gameObject;
            GameOverPanel = GameOverPanel ?? gameObject.transform.Find("GameOverPanel").gameObject;

            //text
            _timeLeftText = _timeLeftText ?? InGamePanel.transform.Find("Img_TimeLeft").GetComponentInChildren<Text>();
            _userScore = _userScore ?? InGamePanel.transform.Find("Img_Score").GetComponentInChildren<Text>();
            _congratsText = _congratsText ?? InGamePanel.transform.Find("Txt_Congrats").GetComponent<Text>();
            _countDownText = _countDownText ?? InGamePanel.transform.Find("Txt_CountDown").GetComponent<Text>();
            _countdownRectTransform = _countDownText.GetComponent<RectTransform>();

            GamePausePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            InGamePanel.SetActive(true);
        }

        private void OnDestroy ( )
        {
            _instance = null;
        }

        private void OnEnable ( )
        {
            EventManager.StartListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
        }

        private void OnDisable ( )
        {
            EventManager.StopListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
        }


        void Update ( )
        {
            _userScore.text = User.Score.ToString();
            _timeLeftText.text = Game.TimeLeft.ToString("##.##") + "s";

            var timeToStart = Game.Instance.TimeToStart;
            if (timeToStart >= 0) {
                _countDownText.gameObject.SetActive(true);
                _countDownText.text = Mathf.Ceil(timeToStart).ToString();
                _countdownRectTransform.localScale =
                    Vector3.one * (1.0f - (timeToStart - Mathf.Floor(timeToStart)));
            } else {
                _countdownRectTransform.localScale = Vector3.zero;
            }
        }

        public void ShowCongartsText (int bonusTime)
        {
            if (!_congratsText.IsActive()) {
                _congratsText.gameObject.SetActive(true);
                _congratsText.rectTransform.anchoredPosition = Vector2.zero;
                _congratsText.rectTransform.localScale = Vector3.one * 0.8f;
            }
            _congratsText.text = CongratStrings[UnityEngine.Random.Range(0, CongratStrings.Length)];
            _congratsText.text += "\n" + "+" + bonusTime + "s";
            _congratsText.rectTransform.DOScale(Vector3.one * 1.3f, 1f);
            _congratsText.rectTransform
                         .DOAnchorPos(_congratsText.rectTransform.anchoredPosition + new Vector2(0, 75), 1f)
                         .OnComplete(( ) => { _congratsText.gameObject.SetActive(false); });
        }

        public void UpdateTargetText (int targetHour, int targetMinute, ArrowBehaviour.ArrowType selectedType)
        {
            if (targetHour == -1 || targetMinute == -1) {
                _targetHourText.text = _targetMinuteText.text = "";
                _targetHourText.color = _targetMinuteText.color = NormalColor;
                return;
            }
            _targetHourText.text = targetHour + "h ";
            _targetHourText.color = selectedType == ArrowBehaviour.ArrowType.Hours ? SelectedColor : NormalColor;
            _targetMinuteText.text = targetMinute + "m";
            _targetMinuteText.color = selectedType == ArrowBehaviour.ArrowType.Minutes ? SelectedColor : NormalColor;
        }

        public void ShowGameOverPanel ( )
        {
            GameOverPanel.gameObject.SetActive(true);
            GameOverPanel.transform.SetAsLastSibling();
        }

        #region buttons events

        public void OnPauseClick (bool pause)
        {
            Debug.Log("[InGame] On Pause Click");
            Game.Pause = pause;
            GamePausePanel.SetActive(pause);
            if (pause) {
                GamePausePanel.transform.SetAsLastSibling();
            } else {
                InGamePanel.transform.SetAsLastSibling();
            }
        }

        public void OnRestartButtonClick ( )
        {
            Debug.Log("[InGame] Restart Button");
            SceneManager.LoadScene(GameSceneName);
        }


        public void OnExitButtonClick ( )
        {
            Application.Quit();
        }

        public void OnMainMenuClick ( )
        {
            SceneManager.LoadScene(MainMenuSceneName);
        }

        #endregion


        public static InGame Instance {
            get {
                if (_instance)
                    return _instance;
                Debug.LogError("Cannot get InGame Instance");
                return null;
            }
            private set { _instance = value; }
        }
    }
}