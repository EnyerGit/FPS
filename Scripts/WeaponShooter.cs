using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;   // Prefab de la bala (Proj_Energia)
    public Transform shootPoint;          // Empty en la punta del arma

    [Header("Parámetros de disparo")]
    public float projectileSpeed = 40f;   // Velocidad de la bala
    public float fireRate = 0.25f;        // Tiempo entre disparos (segundos)

    [Header("Apuntar (clic derecho)")]
    [Tooltip("Normalmente este mismo objeto del arma")]
    public Transform weaponTransform;     // Malla del arma
    public Camera playerCamera;           // PlayerCamera

    [Tooltip("Cuánto se mueve el arma hacia el centro al apuntar")]
    public Vector3 aimOffset = new Vector3(-0.15f, -0.1f, 0.1f);

    [Tooltip("Qué tan rápido interpola el arma y la cámara al apuntar")]
    public float aimLerpSpeed = 10f;

    [Tooltip("Campo de visión (FOV) al apuntar")]
    public float aimFOV = 40f;

    private float nextFireTime = 0f;

    // Guardamos las posiciones / FOV originales
    private Vector3 originalWeaponLocalPos;
    private float originalFOV;

    void Start()
    {
        if (weaponTransform == null)
            weaponTransform = transform;

        originalWeaponLocalPos = weaponTransform.localPosition;

        if (playerCamera != null)
            originalFOV = playerCamera.fieldOfView;
    }

    void Update()
    {
        // Si estamos en la galería y ya terminó, no disparamos más
        if (GameManager.Instance != null && GameManager.Instance.isGalleryFinished)
            return;

        // 1) Apuntar con clic derecho (MouseButton 1)
        bool isAiming = Input.GetMouseButton(1);
        UpdateAim(isAiming);

        // 2) Disparar con clic izquierdo (MouseButton 0)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void UpdateAim(bool isAiming)
    {
        // Mover el arma
        if (weaponTransform != null)
        {
            Vector3 targetPos = originalWeaponLocalPos;

            if (isAiming)
            {
                targetPos = originalWeaponLocalPos + aimOffset;
            }

            weaponTransform.localPosition =
                Vector3.Lerp(weaponTransform.localPosition, targetPos,
                             Time.unscaledDeltaTime * aimLerpSpeed);
        }

        // Cambiar FOV de la cámara
        if (playerCamera != null)
        {
            float targetFOV = isAiming ? aimFOV : originalFOV;
            playerCamera.fieldOfView =
                Mathf.Lerp(playerCamera.fieldOfView, targetFOV,
                           Time.unscaledDeltaTime * aimLerpSpeed);
        }
    }

    void Shoot()
    {
        // Registrar el tiempo del próximo disparo
        nextFireTime = Time.time + fireRate;

        // 1) Crear el proyectil
        if (projectilePrefab == null || shootPoint == null)
        {
            Debug.LogWarning("WeaponShooter: falta asignar projectilePrefab o shootPoint.");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab,
                                      shootPoint.position,
                                      shootPoint.rotation);

        // 2) Darle velocidad hacia adelante
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // En Unity 6 usamos linearVelocity para evitar warnings
            rb.linearVelocity = shootPoint.forward * projectileSpeed;
        }

        // 3) Avisar al GameManager (solo existe en la galería)
        if (GameManager.Instance != null && !GameManager.Instance.isGalleryFinished)
        {
            GameManager.Instance.RegisterShot();
        }

        // 4) Destruir el proyectil después de unos segundos por si no choca
        Destroy(proj, 3f);
    }
}
