using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed = .6f;
    public float runSpeed = 1.2f; // nueva velocidad al correr

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A/D o ←/→
        float v = Input.GetAxis("Vertical");   // W/S o ↑/↓

        Vector3 move = new Vector3(h, 0f, v);

        // Determinar si está corriendo
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : speed;

        // Activar animaciones
        if (animator != null)
        {
            // Activar animación de caminar solo si hay input y no está corriendo
            animator.SetBool("IsWalking", move.magnitude > 0.1f && !isRunning);

            // Activar animación de correr si está presionando shift y hay input
            animator.SetBool("IsRunning", isRunning && move.magnitude > 0.1f);
        }

        // Opcional: rotar hacia la dirección en la que camina
        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
        }

        MoveCharacter(move, currentSpeed);
    }

    void MoveCharacter(Vector3 move, float currentSpeed)
    {
        if (rb != null)
        {
            // Mover el personaje usando física
            Vector3 movePos = rb.position + move.normalized * currentSpeed * Time.deltaTime;
            rb.MovePosition(movePos);
        }
    }
}
