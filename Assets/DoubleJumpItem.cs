using UnityEngine;

public class DoubleJumpItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 오브젝트의 태그가 "Player"인지 확인
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.hasDoubleJump = true; // 플레이어의 2단 점프 해금!
                Debug.Log("획득: 2단 점프 능력이 해금되었습니다!");

                // 아이템 오브젝트 파괴 (사라짐 연출)
                Destroy(gameObject);
            }
        }
    }
}