using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    private void Start() {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Music")
        {
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void RestartGame()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(RestartGameIE());
        }
        else
        {
            UnityEditor.SceneManagement.EditorSceneManager.LoadScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().buildIndex);
        }
        IEnumerator RestartGameIE()
        {
            yield return new WaitForSeconds(1.0f);
            UnityEditor.SceneManagement.EditorSceneManager.LoadScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().buildIndex);
        }
        // UnityEditor.SceneManagement.EditorSceneManager.LoadScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().buildIndex);
    }

    public void Musicwork() {
        if (PlayerPrefs.GetString("music") == "No")
        {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        } else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
