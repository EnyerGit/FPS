using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    // Carga la escena de entrenamiento (galería de tiro)
    public void PlayTraining()
    {
        Time.timeScale = 1f; // por si vienes de un juego pausado
        SceneManager.LoadScene("01_GaleriaDeTiro");
    }

    // Carga el Nivel 1 de enemigos
    public void PlayLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("02_Nivel_Enemigos");
    }

    // Sale del juego
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Si estás en el editor, detiene el Play
        EditorApplication.isPlaying = false;
        #else
        // Si es un build, cierra la app
        Application.Quit();
        #endif
    }
}
