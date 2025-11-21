using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton sencillo para acceder desde cualquier script
    public static GameManager Instance { get; private set; }

    [Header("Estado de juego")]
    public int score = 0;

    [Header("Vidas")]
    public int startingLives = 3;                 // Vidas iniciales
    [HideInInspector] public int currentLives;    // Vidas actuales

    [Header("Disparos (flechas)")]
    public int maxShots = 9;                      // Máximo de disparos (las flechas)
    [HideInInspector] public int currentShots = 0;

    [Header("Dianas")]
    public int totalTargets = 3;                  // Número total de dianas en la galería
    private int destroyedTargets = 0;

    [Header("Flags")]
    public bool isGalleryFinished = false;        // Se pone en true cuando termina la galería

    void Awake()
    {
        // Asegurar que haya solo un GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Inicializar contadores
        currentLives = startingLives;
        currentShots = 0;
        destroyedTargets = 0;
        isGalleryFinished = false;
    }

    // ===== PUNTUACIÓN =====
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntuación: " + score);
    }

    // ===== REGISTRO DE DISPARO =====
    public void RegisterShot()
    {
        if (isGalleryFinished) return;

        currentShots++;
        Debug.Log("Disparo #" + currentShots + " / " + maxShots);

        if (currentShots >= maxShots)
        {
            EndGallery("Se usaron todas las balas");
        }
    }

    // ===== VIDAS =====
    public void TakeDamage(int amount)
    {
        if (isGalleryFinished) return;

        currentLives -= amount;
        if (currentLives < 0) currentLives = 0;

        Debug.Log("Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            EndGallery("Sin vidas");
        }
    }

    // ===== DIANAS =====
    public void RegisterTargetDestroyed()
    {
        destroyedTargets++;
        Debug.Log("Dianas destruidas: " + destroyedTargets + " / " + totalTargets);

        if (destroyedTargets >= totalTargets)
        {
            EndGallery("Todas las dianas destruidas");
        }
    }

    // ===== FIN DE GALERÍA =====
    // ===== FIN DE GALERÍA =====
    void EndGallery(string reason)
    {
        if (isGalleryFinished) return;   // Evitar que se llame dos veces

        isGalleryFinished = true;

        Debug.Log("=== FIN DE LA GALERÍA DE TIRO ===");
        Debug.Log("Motivo: " + reason);
        Debug.Log("Puntuación final: " + score +
                  " | Vidas restantes: " + currentLives +
                  " | Disparos usados: " + currentShots + " / " + maxShots);

        // Buscar el controlador de UI de la galería
      #if UNITY_2023_1_OR_NEWER
          GalleryEndUI ui = FindAnyObjectByType<GalleryEndUI>();
      #else
          GalleryEndUI ui = FindObjectOfType<GalleryEndUI>();
      #endif

        if (ui != null)
        {
            ui.ShowEndPanel(reason);
        }
        else
        {
            Debug.LogWarning("No se encontró GalleryEndUI en la escena.");
        }
    }

}
