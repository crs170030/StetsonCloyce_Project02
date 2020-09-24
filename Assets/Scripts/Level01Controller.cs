using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text _currentScoreTextView;

    int _currentScore;
    string highScoreSt = "HighScore";

    // Update is called once per frame
    private void Update()
    {
        //Increase Score
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitLevel();
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        //increase score
        _currentScore += scoreIncrease;
        //update score display so we can see new score
        _currentScoreTextView.text = "Score: " + _currentScore.ToString();
    }

    public void ExitLevel()
    {
        //check if score is greater than highscore
        int highScore = PlayerPrefs.GetInt(highScoreSt);
        if(_currentScore > highScore)
        {
            //save current score as new high score
            PlayerPrefs.SetInt(highScoreSt, _currentScore);
            Debug.Log("New high score: " + _currentScore);
        }
        //load level
        SceneManager.LoadScene("MainMenu");
    }
}
