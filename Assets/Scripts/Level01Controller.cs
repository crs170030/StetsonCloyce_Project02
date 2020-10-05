using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * TODO:
 * V  Call Mouse Look function to stop camera movement while the menu is open.
 * V  Make gun move backwards and return to postion when shooting
 * V  Make impact effect work
 * V  Create Health system for player
 * V  create damage volume to hurt player
 * V  make player hud with health bar and score
*/

public class Level01Controller : MonoBehaviour
{
    [SerializeField] GameObject _menuQuit = null;
    public GameObject _player  = null;
    public GameObject _camera = null;
    public GunController _gun;
    public PlayerMovement _pm;
    private MouseLook _mouse;//get reference to camera mouse control
    public GameObject _menuDeath = null;

    [SerializeField] Text _currentScoreTextView = null;

    int _currentScore;
    string highScoreSt = "HighScore";
    bool menuOpen = false;
    bool dead = false;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        //lock the cursor when the game starts
        lockCursor(true);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //_mouse = FindObjectOfType<MouseLook>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!menuOpen && !dead) //don't take player input while paused
        {
            //Increase Score
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IncreaseScore(5);
            }
            //Increase Score
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeDamage(20);
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
        _gun = FindObjectOfType<GunController>();
        _pm = FindObjectOfType<PlayerMovement>();
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

        if (!dead)//stop gun shooting and player movement if dead
        {
            _gun.menuIsOpen = menuOpen;
            _pm.pmMenuIsOpen = menuOpen;
        }
        else
        {
            _gun.menuIsOpen = true;
            _pm.pmMenuIsOpen = true;
        }
    }

    private void lockCursor(bool activateLock)
    {
        _mouse = FindObjectOfType<MouseLook>();

        if (activateLock)
        {
            _mouse.timeToStop = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            _mouse.timeToStop = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("You killed me!");
        //_player.SetActive(false);
        //_camera.SetActive(true);
        _menuDeath.SetActive(true);
        dead = true;
        OpenMenu();
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
