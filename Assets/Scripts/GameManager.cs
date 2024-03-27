using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject[] powerUpPrefabs;
    public GameObject[] spawnPoints;

    int score = 0;

    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject reloadButton;
    public GameObject pauseButton;
    public GameObject player;
    public GameObject powerup;
    public TextMeshProUGUI pauseText;
    public GameObject titleCard;

    public NewBehaviourScript playerScript;
    public TextMeshProUGUI damagePanel;

    private int highScore = 0;
    private string highScoreKey = "Meilleur score";
    public TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        damagePanel.enabled = false;
        pauseText.enabled = false;
        scoreText.enabled = false;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        //obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);
        }
        UpdateHighScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.hasTriggeredScore)
        {
            BonusScore();
            playerScript.hasTriggeredScore = false; 
        }

        if (playerScript.hasTriggeredNuke)
        {
            
            Nuke();
            playerScript.hasTriggeredNuke = false; 
        }
        if (playerScript.hasLost)
        {
            GameEnd();
            playerScript.hasLost = false; 
        }
        UpdateHighScore(score);
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = "Meilleur score: " + highScore.ToString();
    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > highScore)
        {
            highScore = newScore;
            PlayerPrefs.SetInt(highScoreKey, highScore); // Sauvegarder le nouveau score le plus élevé
            UpdateHighScoreText();
        }
    }

    IEnumerator SpawnObstacles()
    {
        while(true)
        {
            float waitTime = Random.Range(0.5f, 1.25f);

            yield return new WaitForSeconds(waitTime);

            int randomSpa = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPosition = spawnPoints[randomSpa].transform.position;

            int randomObs = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstacle = obstaclePrefabs[randomObs];

            Instantiate(obstacle, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnPowerUps()
    {
        while(true)
        {
            float waitTime = 2f;

            yield return new WaitForSeconds(waitTime);

            float probaPowerUp = Random.Range(0f, 1f);
            if (probaPowerUp > 0.5f)
            {
                int randomPow = Random.Range(0, powerUpPrefabs.Length);
                GameObject powerup = powerUpPrefabs[randomPow];

                int randomSpa = Random.Range(0, spawnPoints.Length);
                Vector3 spawnPosition = spawnPoints[randomSpa].transform.position;

                Instantiate(powerup, spawnPosition, Quaternion.identity);
            }
        }
    }
    void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void BonusScore() 
    {
        score += 10;
        scoreText.text = score.ToString();
    }

    void Nuke()
    {
        GameObject[] field = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in field)
        {
            Destroy(obstacle);
        }
    }

    public void Pause()
    {
        if(Time.timeScale != 0)
        {
            Time.timeScale = 0f;
            pauseText.enabled = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseText.enabled = false;
        }
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
        scoreText.enabled = true;
        titleCard.SetActive(false);
        StartCoroutine(playerScript.ResetFlagAfterDelay());
        player.SetActive(true);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        StartCoroutine("SpawnObstacles");
        StartCoroutine("SpawnPowerUps");
        InvokeRepeating("ScoreUp", 2f, 1f);
    }

    public void GameEnd()
    {
        
        Time.timeScale = 0f;
        damagePanel.enabled = true; 
        player.SetActive(false); 
        reloadButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }
}
