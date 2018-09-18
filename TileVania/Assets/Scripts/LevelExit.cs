using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float delayTime = 3f;

    private void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        // Do some things before entering next level

        yield return new WaitForSecondsRealtime(delayTime);
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

}
