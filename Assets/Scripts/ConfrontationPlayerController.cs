// GunCombatController.cs
using UnityEngine;

public class GunCombatController : MonoBehaviour
{
    public Animator animator;
    public float speed = 5f;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    private Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        AimTowardsMouse();
        HandleShooting();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);

        if (animator != null)
            animator.SetBool("IsWalking", move.magnitude > 0.1f);

        Vector3 movePos = transform.position + move.normalized * speed * Time.deltaTime;
        rb.MovePosition(movePos);
    }

    void AimTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            if (rbBullet != null)
            {
                rbBullet.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
        }
    }
}
