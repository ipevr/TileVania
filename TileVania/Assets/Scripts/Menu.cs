using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [Header("Panels")]
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject congratulationPanel;

    [Header("Audio Files")]
    [SerializeField] AudioClip gameWonSound;

    private void Start() {
        startButton.SetActive(SceneManager.GetActiveScene().buildIndex == 0);
        restartButton.SetActive(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1);
        congratulationPanel.SetActive(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1);
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            AudioSource.PlayClipAtPoint(gameWonSound, Camera.main.transform.position);
        }
    }

    public void StartFirstLevel() {
        SceneManager.LoadScene(1);
    }

    public void StartMainMenu() {
        SceneManager.LoadScene(0);
        foreach (GameSession gameSession in FindObjectsOfType<GameSession>()) {
            Destroy(gameSession.gameObject);
        }
    }

}
