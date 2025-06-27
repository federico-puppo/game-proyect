using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    public Transform player;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float shootingInterval = 2f;
    public float bulletSpeed = 10f;
    public float rotationSpeed = 5f;

    private float timer;

    void Update()
    {
        if (player == null) return;

        // Rotar hacia el jugador
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // mantener rotación solo en Y
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Disparar a intervalos
        timer += Time.deltaTime;
        if (timer >= shootingInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
        }
    }
}
