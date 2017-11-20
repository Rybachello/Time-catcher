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

        //private Button _resumeButton;
        //private Button _restartButton;
        //private Button _pauseButton;
        //private Button _exitButton;

        private readonly string[] _congratStrings = new[] {"\"Awesome!\"", "Nice!", "Impressive!", "Wonderful!"};
        private static InGame _instance;

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
            ////buttons
            //_resumeButton = _resumeButton ?? GamePausePanel.transform.Find("Btn_resume").GetComponent<Button>();
            //_restartButton = _restartButton ?? GamePausePanel.transform.Find("Btn_restart").GetComponent<Button>();
            //_exitButton = _exitButton ?? GamePausePanel.transform.Find("Btn_exit").GetComponent<Button>();
            //_pauseButton = _pauseButton ?? InGamePanel.transform.Find("Btn_pause").GetComponent<Button>();
            //text
            _timeLeft = _timeLeft ?? InGamePanel.transform.Find("Img_TimeLeft").GetComponentInChildren<Text>();
            _userScore = _userScore ?? InGamePanel.transform.Find("Img_Score").GetComponentInChildren<Text>();
            _targetText = _targetText ?? InGamePanel.transform.Find("Img_TargetTime").GetComponentInChildren<Text>();

            GamePausePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            InGamePanel.SetActive(true);
        }

        //private void Start ( ) {
        //    _pauseButton.onClick.AddListener(( ) => OnPauseClick(true));
        //    _resumeButton.onClick.AddListener(( ) => OnPauseClick(false));
        //    _restartButton.onClick.AddListener(OnRestartButtonClick);
        //    _restartButton.onClick.AddListener(OnExitButtonClick);
        //}

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
            _congratsText.text = _congratStrings[UnityEngine.Random.Range(0, _congratStrings.Length)];
            _congratsText.text += "\n" + "+" + bonusTime + "s";
            _congratsText.rectTransform.DOScale(Vector3.one * 1.3f, 1f);
            _congratsText.rectTransform
                         .DOAnchorPos(_congratsText.rectTransform.anchoredPosition + new Vector2(0, 50), 1f)
                         .OnComplete(( ) => { _congratsText.gameObject.SetActive(false); });
        }

        public void UpdateTargetText (int targetHour, int targetMinute) {
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
            SceneManager.LoadScene("game");
        }


        public void OnExitButtonClick ( ) {
            Application.Quit();
        }

        public void OnMainMenuClick ( ) {
            SceneManager.LoadScene("menu");
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