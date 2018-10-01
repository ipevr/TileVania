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
    [SerializeField] GameObject gameOverPanel;

    [Header("Audio Files")]
    [SerializeField] AudioClip gameOverSound;

    private int playerLifes = 0;
    private int coins = 0;
    
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
        ActualizeCoinsCounter(coins);
        ActualizeLifesCounter(playerLifes);
        gameOverPanel.SetActive(false);
    }

    private void ActualizeCoinsCounter(int points) {
        coinsText.text = points.ToString();
    }

    private void ActualizeLifesCounter(int lifes) {
        lifesText.text = (lifes - 1).ToString();
    }

    private void TakeLife() {
        playerLifes--;
        ActualizeLifesCounter(playerLifes);
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
        ActualizeCoinsCounter(coins);
    }

    public void AddLife() {
        if (playerLifes < playerMaxLifes) {
            playerLifes++;
            ActualizeLifesCounter(playerLifes);
        }
    }

}
