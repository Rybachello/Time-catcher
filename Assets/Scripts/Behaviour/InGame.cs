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
        public GameObject GameOverPanel;
        public GameObject InGamePanel;

        [SerializeField] private Text _userScore;
        [SerializeField] private Text _timeLeft;
        [SerializeField] private Text _targetText;
        [SerializeField] private Text _testText;


        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _exitButton;

        private readonly string[] _congratStrings = new[] { "\"Awesome!\"", "Nice!", "Impressive!", "Wonderful!" };
        private static InGame _instance;

        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
            //get ref
            _instance = this;
            //panel
            GameOverPanel = GameOverPanel ?? gameObject.transform.Find("GameOverPanel").gameObject;
            InGamePanel = InGamePanel ?? gameObject.transform.Find("InGamePanel").gameObject;
            //buttons
            _resumeButton = _resumeButton ?? GameOverPanel.transform.Find("Btn_resume").GetComponent<Button>();
            _restartButton = _restartButton ?? GameOverPanel.transform.Find("Btn_restart").GetComponent<Button>();
            _exitButton = _exitButton ?? GameOverPanel.transform.Find("Btn_exit").GetComponent<Button>();
            _pauseButton = _pauseButton ?? InGamePanel.transform.Find("Btn_pause").GetComponent<Button>();
            //text
            _timeLeft = _timeLeft ?? InGamePanel.transform.Find("Img_TimeLeft").GetComponentInChildren<Text>();
            _userScore = _userScore ?? InGamePanel.transform.Find("Img_Score").GetComponentInChildren<Text>();
            _targetText = _targetText ?? InGamePanel.transform.Find("Img_TargetTime").GetComponentInChildren<Text>();

            GameOverPanel.SetActive(false);
        }

        private void Start ( ) {
            _pauseButton.onClick.AddListener(( ) => OnPauseClick(true));
            _resumeButton.onClick.AddListener(( ) => OnPauseClick(false));
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _restartButton.onClick.AddListener(OnExitButtonClick);
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

        public void UpdateTargetText (int targetHour, int targetMinute) {
            _targetText.text = targetHour + "h " + targetMinute + "m";
        }

        private void OnPauseClick (bool pause) {
            Debug.Log("Click");
            Game.Pause = pause;
            GameOverPanel.SetActive(pause);
            if (pause) {
                GameOverPanel.transform.SetAsLastSibling();
            } else {
                InGamePanel.transform.SetAsLastSibling();
            }
        }

        public void ShowGameOverPanel ( ) {
            //todo:uncomment here
            //if (GameOverPanel.activeInHierarchy == false) {
            //    GameOverPanel.SetActive(true);
            //}
        }

        private void OnRestartButtonClick ( ) {
            Debug.Log("[InGame] Restart Button");
            SceneManager.LoadScene("game");
        }


        private void OnExitButtonClick ( ) {
            Application.Quit();
        }

        public static InGame Instance {
            get {
                if (_instance)
                    return _instance;
                Debug.LogError("Cannot get InGame Instance");
                return null;
            }
            private set { _instance = value; }
        }

     

        public void ShowCongartsText (int bonusTime) {
            
            if (!_testText.IsActive()) {
                _testText.gameObject.SetActive(true);
                _testText.rectTransform.anchoredPosition = Vector2.zero;
                _testText.rectTransform.localScale = Vector3.one * 0.8f;
            }
            _testText.text = _congratStrings[UnityEngine.Random.Range(0, _congratStrings.Length)];
            _testText.text += "\n" + "+" + bonusTime + "s";
            _testText.rectTransform.DOScale(Vector3.one * 1.3f,1f);
            _testText.rectTransform
                     .DOAnchorPos(_testText.rectTransform.anchoredPosition + new Vector2(0,50), 1f)
                     .OnComplete(() => {
                         _testText.gameObject.SetActive(false);
                     });
        }

     
    }
}