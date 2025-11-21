using UnityEngine;

public class EnergyProjectile : MonoBehaviour
{
    [Header("Puntuación (Galería de tiro)")]
    public int scoreOnTarget = 10;          // Puntos por cada diana
    [Tooltip("Tag que usan las dianas en la galería")]
    public string targetTag = "Diana";

    [Header("Daño a enemigos (Nivel 1)")]
    public int damageToEnemy = 1;           // Daño por impacto a enemigos

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Proyectil chocó con: " + other.name + " (tag: " + other.tag + ")");

        // 1) ¿Golpeó a una diana de la galería?
        if (other.CompareTag(targetTag) && GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreOnTarget);
            GameManager.Instance.RegisterTargetDestroyed();

            Destroy(other.gameObject);   // destruye la diana
            Destroy(gameObject);         // destruye el proyectil
            return;
        }

        // 2) ¿Golpeó a un enemigo del Nivel 1?
        //    Buscamos un componente EnemyHealth en este objeto o sus padres.
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageToEnemy);
            Destroy(gameObject);
            return;
        }

        // 3) Cualquier otra cosa (pared, suelo, etc.)
        Destroy(gameObject);
    }
}
