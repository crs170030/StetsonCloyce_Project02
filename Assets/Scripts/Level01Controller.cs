using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * TODO:
 *  ~ Fix enemy not turning back all the way when it is bounced by a shot
 *  V add sound to when targets are destroyed
 *  X move death sound and explosion code to level controller so targets and enemys can share code and die in peace
 *  V add death sound
 *  V add death screen effect (red filter?)
 *  
 *  V make sound work- on audio manager: import scene tools, check scene number, play based on current scene
 *  V add audio track to slot on audio manager
 *  
 *  V make player fov increase while running
 *  ~ make enemy gun tilt downward when not hostile (gravity tilts the enemy face downward)
 *  V make jump sound
 *  V made plateu and death trigger at bottom of world
 *  V make win condition (Kill everyone, collect currency)
 *  
 *  change timer to float type
 *  make money effect just stay there for a few seconds
 *  
 *  Stretch goals:
 *  V Level Design -Wild West Town
 *  V Make Enemy Move towards player
 *  V Make Boss Enemy
 *  X Power Ups? -(Gun glows and is 3x powerfull in force and damage)(Speed increase)
 *  Make time slow powerup
 *  Health Pack Pick up
 *  Money Pick Up
 *  Make gun have 6 bullets
 *  Make reload mechanic and animation
 *  X Make Rocket Enemy
*/

public class Level01Controller : MonoBehaviour
{
    [SerializeField] GameObject _menuQuit = null;
    public GameObject _player  = null;
    //public GameObject _camera = null;
    public GunController _gun;  //ref to stop shooting when paused
    public PlayerMovement _pm;  //ref to stop movement when paused
    public SceneLoader _sl;     //ref to use scene loader script for music functionality
    private MouseLook _mouse;//get reference to camera mouse control
    public GameObject _menuDeath = null; //reference for menu
    public GameObject _bloody = null;    //reference for death screen filter
    public GameObject _menuWin = null;

    [SerializeField] Text _currentScoreTextView = null;
    [SerializeField] Text _currentTimeTextView = null;
    [SerializeField] Text _scoreIncreaseTextView = null;

    public int timeLimit = 120;
    int _currentScore;
    int _timeRemaining = 0;
    int _scoreIncrease;
    string highScoreSt = "HighScore";
    public int spawnScore = 1000;
    public int winScore = 2000;
    bool menuOpen = false;
    bool dead = false;
    bool timerIsRunning = true;

    public int maxHealth = 100;
    public int currentHealth;
    public int healthWarning = 25;

    public HealthBar healthBar;

    public float c_FOV = 60f;
    public float c_FOVRunning = 70f;

    public AudioClip lowHealth;
    public AudioClip damagedSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioSource _playerSounds;

    private void Start()
    {
        //lock the cursor when the game starts
        lockCursor(true);
        //make sure game is not paused
        Time.timeScale = 1;
        _timeRemaining = timeLimit;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //_mouse = FindObjectOfType<MouseLook>();
        //_camera = FindObjectOfType

        _playerSounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        //debug code for damage and score
        /*if (!menuOpen && !dead) //don't take player input while paused
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
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }

        if (timerIsRunning)
        {
            if(_timeRemaining > 0)
            {
                _timeRemaining -= (int)Time.deltaTime;
                _currentTimeTextView.text = "Time: " + _timeRemaining.ToString();
            }
            else
            {
                Debug.Log("Time has run out!");
                _timeRemaining = 0;
                _currentTimeTextView.text = "Time: " + _timeRemaining.ToString();
                timerIsRunning = false;
                if(_currentScore < winScore)
                {

                }
            }
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        //increase score
        _currentScore += scoreIncrease;
        //update score display so we can see new score
        _currentScoreTextView.text = "Score: $" + _currentScore.ToString();
        //StartCoroutine(moneyEffect(_currentScore));
        money(_currentScore);

        //check if all enemies are dead
        if ((_currentScore >= spawnScore) && (_currentScore < (int)(spawnScore * 1.9))) //if 10 members are dead, but dont duplicate
        {
            Debug.Log("The Gang is Dead! Here comes da boss!");
            BossController _boss = FindObjectOfType<BossController>();
            _boss.Spawn();
        }

        if(_currentScore >= winScore)
        {
            StartCoroutine(WinScreen());//call victory when score is over limit
        }
    }

    void money(int monScore)
    {
        //StartCoroutine(moneyEffect(_currentScore));
        _scoreIncreaseTextView.transform.position = new Vector3(463f, 350f, -133f);
        _scoreIncreaseTextView.gameObject.SetActive(true);
        _scoreIncreaseTextView.text = "$" + monScore.ToString();
        for (int i = 0; i < 100; i++)
        {
            _scoreIncreaseTextView.transform.position += new Vector3(0f, 0.001f, 0f);
        }
        //_scoreIncreaseTextView.transform

        _scoreIncreaseTextView.gameObject.SetActive(false);
    }

    /*IEnumerator moneyEffect(int monScore)
    {
        _scoreIncreaseTextView.transform.position = new Vector3(463f, 350f, -133f);
        _scoreIncreaseTextView.gameObject.SetActive(true);
        _scoreIncreaseTextView.text = "$" + monScore.ToString();
        for(int i = 0; i<100; i++)
        {
            _scoreIncreaseTextView.transform.position += new Vector3(0f,1f,0f);
        }
        //_scoreIncreaseTextView.transform

        _scoreIncreaseTextView.gameObject.SetActive(false);
        //return 1;
    }*/

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
        //SceneManager.LoadScene("MainMenu");
        _sl.LoadScene("MainMenu");
    }

    IEnumerator WinScreen()
    {
        Debug.Log("The Orange Organization is defeated. Player Wins.");
        _playerSounds.PlayOneShot(winSound, 1f);
        _menuWin.SetActive(true);
        yield return new WaitForSeconds(3f);
        ExitLevel();
    }
}
