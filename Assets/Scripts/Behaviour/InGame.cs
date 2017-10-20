using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Behaviour.managers;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public Text TargetText;
    public Text UserScoreText;

    void Update ( ) {
        UserScoreText.text = User.Score.ToString();
        //todo: finish here
        //var time = Game.Instance.TargetTime;
        //TargetText.text = "Catch time:" + (time / 60).ToString("##") + ":" + (time % 60).ToString("##");
    }
}