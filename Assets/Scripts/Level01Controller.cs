using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * TODO:
 *  ~ Fix enemy not turning back all the way when it is bounced by a shot
 *  V add sound to when targets are destroyed
 *  X move death sound and explosion code to level controller so targets and enemys can share code and die in peace
 *  V add death sound
 *  V add death screen effect (red filter?)
 *  
 *  make sound work- on audio manager: import scene tools, check scene number, play based on current scene
 *  add audio track to slot on audio manager
 *  
 *  V make player fov increase while running
 *  ~ make enemy gun tilt downward when not hostile (gravity tilts the enemy face downward)
 *  V make jump sound
 *  V made plateu and death trigger at bottom of world
 *  make win condition
 *  
 *  Stretch goals:
 *  V Level Design -Wild West Town
 *  Make Enemy Move towards player
 *  Power Ups? -(Gun glows and is 3x powerfull in force and damage)(Speed increase)
 *  Health Pack Pick up
 *  Make gun have 6 bullets
 *  Make reload mechanic and animation
 *  X Make Rocket Enemy
*/

public class Level01Controller : MonoBehaviour
{
    [SerializeField] GameObject _menuQuit = null;
    public GameObject _player  = null;
    //public GameObject _camera = null;
    public GunController _gun;
    public PlayerMovement _pm;
    private MouseLook _mouse;//get reference to camera mouse control
    public GameObject _menuDeath = null; //reference for menu
    public GameObject _bloody = null;    //reference for death screen filter

    [SerializeField] Text _currentScoreTextView = null;

    int _currentScore;
    string highScoreSt = "HighScore";
    bool menuOpen = false;
    bool dead = false;

    public int maxHealth = 100;
    public int currentHealth;
    public int healthWarning = 25;

    public HealthBar healthBar;

    public float c_FOV = 60f;
    public float c_FOVRunning = 70f;

    public AudioClip lowHealth;
    public AudioClip damagedSound;
    public AudioClip deathSound;
    public AudioSource _playerSounds;

    private void Start()
    {
        //lock the cursor when the game starts
        lockCursor(true);
        //make sure game is not paused
        Time.timeScale = 1;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //_mouse = FindObjectOfType<MouseLook>();
        //_camera = FindObjectOfType

        _playerSounds = GetComponent<AudioSource>();
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

            if (!dead)//if player is dead, then don't resume time
            {
                //Time has resumed
                //Soshite toki wa ugaki dasu
                Time.timeScale = 1;
            }
        }
        else
        {
            //menu is closed. toggle open
            menuOpen = true;
            _menuQuit.SetActive(true);
            //add cursor lock
            lockCursor(false);

            //ZA WARUDO TOKI WO TOMARE!
            Time.timeScale = 0;
        }

        //may not be fully necessary but I will keep it to stop sprint and jump effects even if they don't do anything
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
        _playerSounds.PlayOneShot(damagedSound, 1f);

        if(currentHealth <= 0)
        {
            Kill();
        }
        else if(currentHealth <= healthWarning)
        {
            //play low health sound
            _playerSounds.PlayOneShot(lowHealth, 1f);
        }
    }

    public void Kill()
    {
        Debug.Log("You killed me!");
        //_player.SetActive(false);
        //_camera.SetActive(true);
        _menuDeath.SetActive(true);
        _bloody.SetActive(true);
        if(!dead)
            _playerSounds.PlayOneShot(deathSound, 1f); //only play dead sound once

        dead = true;
        OpenMenu();
    }

    public void SprintFOV(bool isSprinting)
    {
        if (isSprinting)
        {
            Camera.main.fieldOfView = c_FOVRunning;
            //_camera.fieldOfView = c_FOVRunning;
        }
        else
        {
            Camera.main.fieldOfView = c_FOV;
            //_camera.fieldOfView = c_FOV;
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
