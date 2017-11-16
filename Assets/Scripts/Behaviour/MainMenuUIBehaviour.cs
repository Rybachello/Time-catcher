using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour
{
    public class MainMenuUIBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _playButton;

        private void Awake ( ) {
            Init();
        }

        private void Init ( ) {
            //buttons
            _playButton = _playButton ?? gameObject.transform.Find("Btn_Play").GetComponent<Button>();
            _exitButton = _exitButton ?? gameObject.transform.Find("Btn_Exit").GetComponent<Button>();
        }

        private void Start ( ) {
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnPlayButtonClick ( ) {
            SceneManager.LoadScene("game");
        }

        private void OnExitButtonClick ( ) {
            Application.Quit();
        }
    }
}