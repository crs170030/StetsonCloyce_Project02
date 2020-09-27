using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*TODO:
 * V lock cursor when in level 01
 * V Find quit panel object
 * V when esc is pressed, call panel to active
 * V unlock cursor when panel is active
 * ? pause the game when panel is active (Lock update?)
 * V if esc is pressed and panel is active, make activen't
 * V lock cursor when panel is not active
 * V Find good copyright free music replacement :c
 * 
*/

public class Level01Controller : MonoBehaviour
{
    [SerializeField] GameObject _menuQuit;
    //GameObject _menuQuit;

    [SerializeField] Text _currentScoreTextView;

    int _currentScore;
    string highScoreSt = "HighScore";
    bool menuOpen = false;

    private void Awake()
    {
        //lock the cursor when the game starts
        lockCursor(true);

        //_menuQuit = GameObject.Find("/Canvas/QuitMenu_pnl");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!menuOpen) //don't take player input while paused
        {
            //Increase Score
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IncreaseScore(5);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        //increase score
        _currentScore += scoreIncrease;
        //update score display so we can see new score
        _currentScoreTextView.text = "Score: " + _currentScore.ToString();
    }

    public void OpenMenu()
    {
        Debug.Log("Menu is Active? :: "+ _menuQuit.activeSelf);
        //_menuQuit.activeSelf
        if (_menuQuit.activeSelf)
        {
            //menu is open. toggle to close
            lockCursor(true);
            menuOpen = false;
            _menuQuit.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //menu is closed. toggle open
            menuOpen = true;
            _menuQuit.SetActive(true);
            //add cursor lock
            lockCursor(false);
        }
    }

    private void lockCursor(bool activateLock)
    {
        if (activateLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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
        lockCursor(false); //unlocks cursor in case it was locked when scene changes
        SceneManager.LoadScene("MainMenu");
    }
}
