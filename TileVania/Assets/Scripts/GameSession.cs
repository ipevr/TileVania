using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLifes = 3;

    private void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            FindObjectOfType<Menu>().CreateLifeHearts(playerLifes);
        }
    }

    private void OnLevelWasLoaded(int level) {
        if (gameObject.scene.name == "DontDestroyOnLoad" && level != 0) {
            FindObjectOfType<Menu>().CreateLifeHearts(playerLifes);
        }
    }

    public void ProcessPlayerDeath() {
        if (playerLifes > 1) {
            TakeLife();
        } else {
            ResetGameSession();
        }
    }

    private void TakeLife() {
        FindObjectOfType<Menu>().DestroyLifeHeart();
        playerLifes--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGameSession() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
