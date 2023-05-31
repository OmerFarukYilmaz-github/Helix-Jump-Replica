using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public int best;
    public int score;
    public int currentStage = 0;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this) 
        {
            Destroy(gameObject);
        }

        // Load the saved highscore
        best = PlayerPrefs.GetInt("Highscore");
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level");
        // Show Adds Advertisement.Show();
        Instance.score = 0;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            PlayerPrefs.SetInt("Highscore", score);
            best = score;
        }
    }


}
