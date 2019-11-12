using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float slowMoSpeed = 0.3f;
    [SerializeField] float timeToNextLevel = 0.2f;

    private void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine(loadNextScene());
    }

    IEnumerator loadNextScene()
    {
        Time.timeScale = slowMoSpeed;
        yield return new WaitForSeconds(timeToNextLevel);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
