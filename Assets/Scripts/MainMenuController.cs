﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //[SerializeField] AudioClip _startingSong;
    [SerializeField] Text _highScoreTextView = null;

    // Start is called before the first frame update
    void Start()
    {
        //load highscore to display
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();

        /*
        //play song on start
        if(_startingSong != null){
            AudioManager.Instance.PlaySong(_startingSong);
        }
        */
    }

    public void ResetData()
    {
        //set highscore to zero.
        //is there anything else we need to reset?
        //Debug.Log("Reseting Data to 0...");
        PlayerPrefs.SetInt("HighScore", 0);
        _highScoreTextView.text = "0";
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
