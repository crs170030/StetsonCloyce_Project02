using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * TODO:
 * Call Mouse Look function to stop camera movement while the menu is open.
*/

public class Level01Controller : MonoBehaviour
{
    [SerializeField] GameObject _menuQuit = null;
    //public GameObject mainCamera;
    //private MouseLook _mouse;//get reference to camera mouse control
    //GameObject _menuQuit;

    [SerializeField] Text _currentScoreTextView = null;

    int _currentScore;
    string highScoreSt = "HighScore";
    bool menuOpen = false;

    private void Awake()
    {
        //lock the cursor when the game starts
        lockCursor(true);

        //_mouse = _MainCamera.GetComponent<MouseLook>();//FindObjectOfType<MouseLook>(); 
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
        //Debug.Log("Menu is Active? :: "+ _menuQuit.activeSelf);
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
            //_mouse.stopCamera(true);
            //_mouse.timeToStop = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            //_mouse.stopCamera(false);
            //_mouse.timeToStop = false;
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
