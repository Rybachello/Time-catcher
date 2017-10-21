using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public GameObject GameOverPanel;

    public Text TargetText;
    public Text UserScoreText;


    private void OnEnable ( ) {
        EventManager.StartListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
    }

    private void OnDisable ( ) {
        EventManager.StopListening(EventManagerType.OnGameEnd, ShowGameOverPanel);
    }

    private void ShowGameOverPanel ( ) {
        if (GameOverPanel.activeInHierarchy == false) {
            GameOverPanel.SetActive(true);
        }
    }

    public void OnRestartButtonClick ( ) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void Update ( ) {
        UserScoreText.text ="Score: "+ User.Score;
        var time = NumberManager.Instance.TargetTime;
        TargetText.text = time ? "Catch time:" + time.Hour : "";
    }
}