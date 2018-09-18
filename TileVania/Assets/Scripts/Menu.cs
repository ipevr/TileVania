using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] GameObject lifesPanel;
    [SerializeField] GameObject heartLifePrefab;

    private List<GameObject> heartLife = new List<GameObject>();

    public void StartFirstLevel() {
        SceneManager.LoadScene(1);
    }

    public void StartMainMenu() {
        SceneManager.LoadScene(0);
        foreach (GameSession gameSession in FindObjectsOfType<GameSession>()) {
            Destroy(gameSession.gameObject);
        }
    }

    public void CreateLifeHearts(int lifes) {
        for (int i = 0; i < lifes; i++) {
            heartLife.Add(Instantiate(heartLifePrefab, lifesPanel.transform));
            Debug.Log(heartLife[i].name);
        }
    }

    public void DestroyLifeHeart() {
        heartLife[0].SetActive(false);
    }
}
