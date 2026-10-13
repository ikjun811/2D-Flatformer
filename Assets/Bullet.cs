using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    [HideInInspector] public int direction = 1; // 1이면 우측, -1이면 좌측

    void Update()
    {
        // 총알을 바라보는 방향으로 계속 이동
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적(Enemy)에 닿으면 적을 파괴하고 총알도 사라짐
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // 적 파괴
            Destroy(gameObject); // 총알 파괴
        }
    }
}