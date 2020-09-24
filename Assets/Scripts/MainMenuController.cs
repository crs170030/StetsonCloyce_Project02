using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text _highScoreTextView;

    // Start is called before the first frame update
    void Start()
    {
        //load highscore to display
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();

        //play song on start
        if(_startingSong != null){
            AudioManager.Instance.PlaySong(_startingSong);
        }
    }
}
