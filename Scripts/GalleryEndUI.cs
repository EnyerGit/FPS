using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GalleryEndUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject endPanel;      // Panel_EndGallery
    public TMP_Text resultLabel;     // Txt_Result (TextMeshPro)

    void Start()
    {
        // Ocultamos el panel al comenzar
        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        // Asegurarnos de que el tiempo esté normal cuando se entra a la escena
        Time.timeScale = 1f;
    }

    // Llamado desde el GameManager cuando termina la galería
    public void ShowEndPanel(string reason)
    {
        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        // Pausar el juego (solo si quieres que se congele todo)
        Time.timeScale = 0f;

        // Mostrar el cursor para poder usar los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Mostrar resultados en el texto
        if (resultLabel != null && GameManager.Instance != null)
        {
            var gm = GameManager.Instance;

            resultLabel.text =
                reason + "\n\n" +
                $"Puntuación final: {gm.score}\n" +
                $"Vidas restantes: {gm.currentLives}\n" +
                $"Disparos usados: {gm.currentShots} / {gm.maxShots}";
        }
    }

    // ===== Botones =====

    public void OnClick_MainMenu()
    {
        Debug.Log("BTN: Menú principal");

        Time.timeScale = 1f;
        SceneManager.LoadScene("00_MainMenu");
    }

    public void OnClick_Retry()
    {
        Debug.Log("BTN: Reintentar galería");

        Time.timeScale = 1f;
        SceneManager.LoadScene("01_GaleriaDeTiro");
    }

    public void OnClick_Level1()
    {
        Debug.Log("BTN: Ir a Nivel 1");

        Time.timeScale = 1f;
        SceneManager.LoadScene("02_Nivel_Enemigos");
    }
}
