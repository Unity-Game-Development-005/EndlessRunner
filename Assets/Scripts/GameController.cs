
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    // set a reference to the player controller
    private PlayerController playerController;

    // set a reference to the pickup controller script
    private PickupController pickupController;

    // set a reference to the obstacle controller script
    private ObstacleController obstacleController;

    // set a reference to the scenery controller script
    private SceneryController sceneryController;


    // game over panel
    public Image gameOverPanel;


    // get a reference to the audio source component
    private AudioSource audioPlayer;

    // game over text
    public TMP_Text gameOverText;

    // start game text
    public TMP_Text startGameText;

    // score UI
    public TMP_Text scoreValue;

    public TMP_Text highScoreValue;

    public TMP_Text elapsedTimeValue;

    public TMP_Text bestTimeValue;

    public TMP_Text countdownTimerValue;


    // in-game sounds
    public AudioClip countSound;

    public AudioClip goSound;



    // score
    [HideInInspector] public int score;

    // high score
    private int highScore = 0;

    // elapsed time
    private float elapsedTime;

    // best time
    private float bestTime = 0;

    // current time
    private float gameTime;

    // for formatting the elapsed time display
    private TimeSpan timeSpan;

    private float countdownTime;

    private float waitSeconds;


    // game over flag
    [HideInInspector] public bool gameOver;

    // pressed a key for start/restart flag
    [HideInInspector] public bool inPlay;

    [HideInInspector] public bool startCountdown;



    private void Awake()
    {
        // get the reference to the player controller script
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // get the reference to the pickup controller script
        pickupController = GameObject.Find("Pickup Controller").GetComponent<PickupController>();

        // get the reference to the obstacle controller script
        obstacleController = GameObject.Find("Obstacle Controller").GetComponent<ObstacleController>();

        // get the reference to the scenery controller script
        sceneryController = GameObject.Find("Background").GetComponent<SceneryController>();

        // set reference to the audio source component
        audioPlayer = GetComponent<AudioSource>();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialise();

        GameOver();
    }


    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();

        RunTimers();
    }



    private void GetPlayerInput()
    {
        WaitForKeyPress();
    }


    private void RunTimers()
    {
        if (startCountdown)
        {
            StartCoroutine(RunCountdownTimer());
        }

        else if (!gameOver)
        {
            RunGameTimeTimer();
        }
    }


    private void Initialise()
    {
        gameOver = true;

        inPlay = false;

        startCountdown = false;

        countdownTime = 3f;
    }


    private void WaitForKeyPress()
    {
        // if the game is over
        if (gameOver)
        {
            // check to see if we press the 's' key to start/restart the game
            if (Input.GetKeyDown(KeyCode.S))
            {
                // if we do
                // clear the game over elements
                gameOverPanel.gameObject.SetActive(false);

                // wait for the countdown
                StartCountdown();

                // set game in play flag
                inPlay = true;

                // and restart the game
                GameInPlay();
            }
        }
    }


    private void StartCountdown()
    {
        // enable the countdown time UI
        countdownTimerValue.gameObject.SetActive(true);

        // update the countdown timer display
        UpdateCountdownTimer();

        // start the countdown
        startCountdown = true;
    }


    private void GameInPlay()
    {
        // clear any spawned pickups
        pickupController.ClearSpawnedPickups();

        // clear any spawned obstacles
        obstacleController.ClearSpawnedObstacles();

        // reset player
        playerController.InitialisePlayer();

        // initialise the score and time
        score = 0;
        
        elapsedTime = 0f;

        gameTime = 0f;

        // update the UI display
        UpdateScore();
        
        UpdateElapsedTime();
    }


    public void GameOver()
    {
        // set game in play flag to false
        inPlay = false;

        // if the current elapsed time is greater then the best time
        if (elapsedTime > bestTime)
        {
            // update the best time
            bestTime = elapsedTime;

            UpdateBestTime();
        }

        // if current score is greater than the high score
        if (score > highScore)
        {
            // update the high score
            highScore = score;

            UpdateHighScore();
        }

        // enable the game over UI elements
        gameOverPanel.gameObject.SetActive(true);
    }


    public void UpdateScore()
    {
        scoreValue.text = score.ToString();
    }


    private void UpdateHighScore()
    {
        highScoreValue.text = highScore.ToString();
    }


    private void UpdateBestTime()
    {
        bestTimeValue.text = bestTime.ToString() + "s";
    }


    private void RunGameTimeTimer()
    {
        // update the game play time
        gameTime += Time.deltaTime;

        // get the elapsed game time in seconds
        timeSpan = TimeSpan.FromSeconds(gameTime);
        
        elapsedTime = timeSpan.Seconds;

        // update the display
        UpdateElapsedTime();
    }


    private IEnumerator RunCountdownTimer()
    {
        // set countdown to false to prevent multiple calls from update
        startCountdown = false;

        // initialise time delay between seconds
        waitSeconds = 2.8f;

        // if countdown time is greater than zero
        while (countdownTime > 0f)
        {
            // update the UI
            UpdateCountdownTimer();

            // play the countdown sound
            audioPlayer.PlayOneShot(countSound, 1f);

            // wait for time delay between seconds
            yield return new WaitForSeconds(waitSeconds);

            // update the countdown timw
            countdownTime--;
        }

        // when the timer has finished
        // play the go sound
        audioPlayer.PlayOneShot(goSound, 1f);

        // disable the countdown timer UI
        countdownTimerValue.gameObject.SetActive(false);

        // end reset the countdown time
        countdownTime = 3f;
    }


    private void UpdateCountdownTimer()
    {
        countdownTimerValue.text = countdownTime.ToString();
    }


    private void UpdateElapsedTime()
    {
        elapsedTimeValue.text = elapsedTime.ToString("0") + "s";
    }


} // end of class
