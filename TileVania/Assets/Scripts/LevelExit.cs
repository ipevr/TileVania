using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float delayTime = 3f;

    [Header("Audio Files")]
    [SerializeField] AudioClip levelExitSound;

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(FindObjectOfType<ScenePersist>());
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        AudioSource.PlayClipAtPoint(levelExitSound, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(delayTime);
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

}
