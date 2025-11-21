using UnityEngine;

public class PlayerFPSController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;              // Qué tan rápido camina
    [Header("Cámara / Ratón")]
    public float mouseSensitivity = 2f;       // Sensibilidad del mouse
    public Transform cameraTransform;         // Aquí pondremos la PlayerCamera

    private CharacterController controller;
    private float verticalRotation = 0f;      // Rotación hacia arriba/abajo de la cámara

    void Start()
    {
        // Buscamos el CharacterController que está en el mismo objeto (Player)
        controller = GetComponent<CharacterController>();

        // Bloquear el cursor en el centro de la pantalla y ocultarlo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1) MOVIMIENTO CON TECLADO (WASD / Flechas)

        // Eje horizontal (A/D o Flechas izquierda/derecha)
        float inputX = Input.GetAxisRaw("Horizontal");
        // Eje vertical (W/S o Flechas arriba/abajo)
        float inputZ = Input.GetAxisRaw("Vertical");

        // Dirección en espacio local (adelante/atrás + derecha/izquierda)
        Vector3 moveDirection = new Vector3(inputX, 0f, inputZ);

        // Si hay movimiento, lo normalizamos para que no corra más en diagonal
        if (moveDirection.magnitude > 1f)
        {
            moveDirection = moveDirection.normalized;
        }

        // Convertimos la dirección local a dirección global según hacia donde mira el jugador
        moveDirection = transform.TransformDirection(moveDirection);

        // SimpleMove aplica el movimiento + gravedad automáticamente
        controller.SimpleMove(moveDirection * moveSpeed);

        // 2) MIRAR CON EL RATÓN (Mouse X, Mouse Y)

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotación horizontal: giramos TODO el Player en Y
        transform.Rotate(0f, mouseX, 0f);

        // Rotación vertical: solo la cámara (mirar arriba/abajo)
        verticalRotation -= mouseY; // restamos para que mover el mouse hacia arriba mire hacia arriba
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f); // límite para no girar la cabeza 360°

        // Aplicamos la rotación vertical a la cámara
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
}
