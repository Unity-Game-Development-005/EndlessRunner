
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    // game over panel
    public Image gameOverPanel;


    // game over text
    public TMP_Text gameOverText;

    // start game text
    public TMP_Text startGameText;

    // score UI
    public TMP_Text scoreValue;

    public TMP_Text highScoreValue;

    public TMP_Text elapsedTimeValue;

    public TMP_Text bestTimeValue;


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


    // game over flag
    public bool gameOver = false;

    // pressed 's' key flag
    public bool hasPressedKey = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameOver();
    }


    // Update is called once per frame
    void Update()
    {
        WaitForSpaceBar();

        if (!gameOver)
        {
            RunTimer();
        }
    }


    private void WaitForSpaceBar()
    {
        // if the game id over
        if (gameOver)
        {
            // check to see if we press the 's' key to start a new game
            if (Input.GetKeyDown(KeyCode.S))
            {
                // if we do
                // clear the game over elements
                gameOverPanel.gameObject.SetActive(false);

                // and restart the game
                RestartGame();
            }
        }
    }


    private void RestartGame()
    {
        // initialise the score and time
        score = 0;
        
        elapsedTime = 0f;

        gameTime = 0f;

        // update the display
        UpdateScore();
        
        UpdateElapsedTime();

        // clear the game over flag
        gameOver = false;
    }


    public void GameOver()
    {
        // set game over flag
        gameOver = true;

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


    private void RunTimer()
    {
        // update the time playing the game
        gameTime += Time.deltaTime;

        timeSpan = TimeSpan.FromSeconds(gameTime);

        // get the elapsed game time in seconds
        elapsedTime = timeSpan.Seconds;

        // update the display
        UpdateElapsedTime();
    }


    private void UpdateElapsedTime()
    {
        elapsedTimeValue.text = elapsedTime.ToString("0") + "s";
    }


} // end of class
