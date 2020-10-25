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
 *  V make money effect just stay there for a few seconds
 *  
 *  Stretch goals:
 *  V Level Design -Wild West Town
 *  V Make Enemy Move towards player
 *  V Make Boss Enemy
 *  X Power Ups? -(Gun glows and is 3x powerfull in force and damage)(Speed increase)
 *  X Make time slow powerup
 *  V Health Pack Pick up
 *  V Money Pick Up
 *  X Make gun have 6 bullets
 *  X Make reload mechanic and animation
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
    public GameObject _menuTimeOut = null;

    [SerializeField] Text _currentScoreTextView = null;
    [SerializeField] Text _currentTimeTextView = null;
    [SerializeField] Text _scoreIncreaseTextView = null;

    public int timeLimit = 120;
    int _currentScore;
    int _timeRemaining = 0;
    float timeElasped = 0;

    int _scoreIncrease;
    string highScoreSt = "HighScore";
    public int spawnScore = 1000;
    public int winScore = 2000;
    bool menuOpen = false;
    bool dead = false;
    bool timerIsRunning = true;
    bool hasWon = false;
    int bossStatus = 0; // 0 == not spawned, 1 == active, 2 == ded

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
    public AudioClip healSound;
    public AudioClip moneySound;
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
    }

    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                timeElasped += Time.deltaTime;
                if (timeElasped > 1f)
                {
                    _timeRemaining -= (int)timeElasped;
                    timeElasped = 0;
                }
                //_timeRemaining -= (int)Time.fixedDeltaTime;
                //if(_timeRemaining % 1 == 0)
                _currentTimeTextView.text = "Time: " + _timeRemaining.ToString();
            }
            else
            {
                Debug.Log("Time has run out!");
                _timeRemaining = 0;
                _currentTimeTextView.text = "Time: " + _timeRemaining.ToString();
                timerIsRunning = false;
                if (_currentScore < winScore)
                {
                    Debug.Log("Time is Up! Game Over");
                    _menuTimeOut.gameObject.SetActive(true); //draw time dialog on top of other menus and call death
                    Kill();
                }
            }
        }

        if (!hasWon && bossStatus == 2) //_currentScore >= winScore && 
        {
            StartCoroutine(WinScreen());//call victory when the boss is killed
            bossStatus = 2;
            hasWon = true;
            timerIsRunning = false;
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        //increase score
        _currentScore += scoreIncrease;
        //update score display so we can see new score
        _currentScoreTextView.text = "Score: $" + _currentScore.ToString();
        StartCoroutine(moneyEffect(scoreIncrease));
        //money(scoreIncrease);

        //check if score surpasses 1000
        if ((_currentScore >= spawnScore) && bossStatus == 0) //if 10 members are dead, but dont duplicate
        {
            Debug.Log("The Gang is Dead! Here comes da boss!");
            BossController _boss = FindObjectOfType<BossController>();
            _boss.Spawn();
            bossStatus = 1;
        }
    }

    IEnumerator moneyEffect(int monScore)
    {
        yield return new WaitForSeconds(.05f);
        _playerSounds.PlayOneShot(moneySound, 1f);
        //_scoreIncreaseTextView.transform.position = new Vector3(463f, 350f, -133f);
        _scoreIncreaseTextView.gameObject.SetActive(true);
        _scoreIncreaseTextView.text = "$" + monScore.ToString();
        /*for(int i = 0; i<100; i++)
        {
            yield return new WaitForSeconds(.01f);
            //_scoreIncreaseTextView.transform.position += new Vector3(0f,1f,0f);
        }*/
        yield return new WaitForSeconds(1f);
        _scoreIncreaseTextView.gameObject.SetActive(false);
    }
    
    public void KillBoss()
    {
        bossStatus = 2;//1 == boss is now active
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

        if(damage > 0)
            _playerSounds.PlayOneShot(damagedSound, 1f);
        else
            _playerSounds.PlayOneShot(healSound, 1f);

        if (currentHealth <= 0)
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
        _sl._audio.PlaySong(9); //call sceneloader to stop music
        if (!dead)
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
        yield return new WaitForSeconds(1f);
        IncreaseScore(_timeRemaining * 10);
        yield return new WaitForSeconds(3f);
        ExitLevel();
    }
}
