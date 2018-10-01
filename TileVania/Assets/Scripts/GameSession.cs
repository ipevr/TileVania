using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLifesAtStart = 3;
    [SerializeField] int playerMaxLifes = 5;
    [SerializeField] float gameOverWaitTime = 3f;

    [Header("Panels")]
    [SerializeField] GameObject lifesPanel;

    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject gameOverPanel;

    [Header("Prefabs")]
    [SerializeField] GameObject lifesPrefab;

    [Header("Audio Files")]
    [SerializeField] AudioClip gameOverSound;

    private List<GameObject> heartLife = new List<GameObject>();
    private int playerLifes = 0;
    private int score = 0;
    private Text scoreText;

    private void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        playerLifes = playerLifesAtStart;
        scoreText = scorePanel.GetComponentInChildren<Text>();
        CreateLifeHearts(playerLifes);
        ActualizeScoreCounter(score);
        gameOverPanel.SetActive(false);
    }

    private void ActualizeScoreCounter(int points) {
        scoreText.text = points.ToString();
    }

    private void CreateLifeHearts(int lifes) {
        for (int i = 0; i < lifes - 1; i++) {
            heartLife.Add(Instantiate(lifesPrefab, lifesPanel.transform));
        }
    }

    private void TakeLife() {
        playerLifes--;
        Destroy(heartLife[playerLifes - 1]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ResetGameSession() {
        AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(gameOverWaitTime);
        SceneManager.LoadScene(0);
        Destroy(FindObjectOfType<ScenePersist>().gameObject);
        Destroy(gameObject);
    }

    public void ProcessPlayerDeath() {
        if (playerLifes > 1) {
            TakeLife();
        } else {
            StartCoroutine(ResetGameSession());
        }
    }

    public void AddToScore(int points) {
        score += points;
        ActualizeScoreCounter(score);
    }

    public void AddHeart() {
        if (playerLifes < playerMaxLifes) {
            heartLife.Add(Instantiate(lifesPrefab, lifesPanel.transform));
            playerLifes++;
        }
    }

}
