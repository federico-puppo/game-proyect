using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed = 5f;
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

        // Activar animación solo si hay input
        if (animator != null)
            animator.SetBool("IsWalking", move.magnitude > 0.1f);

        // Mover el personaje
        transform.Translate(move.normalized * speed * Time.deltaTime, Space.World);

        // Opcional: rotar hacia la dirección en la que camina
        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
        }

        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
        }

        MoveCharacter(move);
    }

    void MoveCharacter(Vector3 move)
    {
        if (rb != null)
        {
            Vector3 movePos = rb.position + move.normalized * speed * Time.deltaTime;
            rb.MovePosition(movePos);
        }
    }
}

