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
    [SerializeField] Text lifesText;
    [SerializeField] Text coinsText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameOverPanel;

    [Header("Audio Files")]
    [SerializeField] AudioClip gameOverSound;

    private int playerLifes = 0;
    private int coins = 0;
    private int score = 0;
    private bool playerIsDead = false;

    public bool PlayerIsDead => playerIsDead;

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
        ActualizeCoinsCounter();
        ActualizeLifesCounter();
        ActualizeScoreCounter();
        gameOverPanel.SetActive(false);
    }

    private void Update() {
        Debug.Log(playerIsDead);
    }

    private void ActualizeCoinsCounter() {
        coinsText.text = coins.ToString();
    }

    private void ActualizeLifesCounter() {
        lifesText.text = (playerLifes - 1).ToString();
    }

    private void ActualizeScoreCounter() {
        scoreText.text = score.ToString();
    }

    private void TakeLife() {
        playerLifes--;
        ActualizeLifesCounter();
        playerIsDead = false;
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

    public void AddToCoins(int points) {
        coins += points;
        ActualizeCoinsCounter();
    }

    public void AddLife() {
        if (playerLifes < playerMaxLifes) {
            playerLifes++;
            ActualizeLifesCounter();
        }
    }

    public void AddToScore(int points) {
        score += points;
        ActualizeScoreCounter();
    }

    public void PlayerGotDeadlyHit() {
        playerIsDead = true;
    }

}
