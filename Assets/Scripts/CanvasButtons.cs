using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class CanvasButtons : MonoBehaviour
{
    public void RestartGame()
    {
        UnityEditor.SceneManagement.EditorSceneManager.LoadScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().buildIndex);
    }
}
