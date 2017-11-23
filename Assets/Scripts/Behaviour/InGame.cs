using System;
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
        [SerializeField] private Text _timeLeft;
        [SerializeField] private Text _targetText;
        [SerializeField] private Text _congratsText;

        private static InGame _instance;

        protected readonly string[] CongratStrings = new[] { "\"Awesome!\"", "Nice!", "Impressive!", "Wonderful!" };
        protected const string MainMenuSceneName = "menu";
        protected const string GameSceneName = "game";

        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
            //get ref
            _instance = this;
            //panel
            GamePausePanel = GamePausePanel ?? gameObject.transform.Find("GamePausePanel").gameObject;
            InGamePanel = InGamePanel ?? gameObject.transform.Find("InGamePanel").gameObject;
            GameOverPanel = GameOverPanel ?? gameObject.transform.Find("GameOverPanel").gameObject;

            //text
            _timeLeft = _timeLeft ?? InGamePanel.transform.Find("Img_TimeLeft").GetComponentInChildren<Text>();
            _userScore = _userScore ?? InGamePanel.transform.Find("Img_Score").GetComponentInChildren<Text>();
            _targetText = _targetText ?? InGamePanel.transform.Find("Img_TargetTime").GetComponentInChildren<Text>();

            GamePausePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            InGamePanel.SetActive(true);
        }

        private void OnDestroy ( ) {
            _instance = null;
        }

        private void OnEnable ( ) {
            EventManager.StartListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
        }

        private void OnDisable ( ) {
            EventManager.StopListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
        }


        void Update ( ) {
            _userScore.text = User.Score.ToString();
            _timeLeft.text = Game.TimeLeft.ToString("##.##") + "s";
            //var time = NumberManager.Instance.TargetTime;

            //MinutesText.text = time?  "Hours :: " + time.Hour :"";
            //HoursText.text = time ? "Minutes :: " + time.Minutes:"";
        }

        public void ShowCongartsText (int bonusTime) {
            if (!_congratsText.IsActive()) {
                _congratsText.gameObject.SetActive(true);
                _congratsText.rectTransform.anchoredPosition = Vector2.zero;
                _congratsText.rectTransform.localScale = Vector3.one * 0.8f;
            }
            _congratsText.text = CongratStrings[UnityEngine.Random.Range(0, CongratStrings.Length)];
            _congratsText.text += "\n" + "+" + bonusTime + "s";
            _congratsText.rectTransform.DOScale(Vector3.one * 1.3f, 1f);
            _congratsText.rectTransform
                         .DOAnchorPos(_congratsText.rectTransform.anchoredPosition + new Vector2(0, 50), 1f)
                         .OnComplete(( ) => { _congratsText.gameObject.SetActive(false); });
        }

        public void UpdateTargetText (int targetHour, int targetMinute) {
            if (targetHour == -1 || targetMinute == -1)
                _targetText.text = "";
            else
                _targetText.text = targetHour + "h " + targetMinute + "m";
        }

        public void ShowGameOverPanel ( ) {
            GameOverPanel.gameObject.SetActive(true);
            GameOverPanel.transform.SetAsLastSibling();
        }

        #region buttons events

        public void OnPauseClick (bool pause) {
            Debug.Log("[InGame] On Pause Click");
            Game.Pause = pause;
            GamePausePanel.SetActive(pause);
            if (pause) {
                GamePausePanel.transform.SetAsLastSibling();
            } else {
                InGamePanel.transform.SetAsLastSibling();
            }
        }

        public void OnRestartButtonClick ( ) {
            Debug.Log("[InGame] Restart Button");
            SceneManager.LoadScene(GameSceneName);
        }


        public void OnExitButtonClick ( ) {
            Application.Quit();
        }

        public void OnMainMenuClick ( ) {
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