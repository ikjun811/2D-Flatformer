using UnityEngine;
using UnityEngine.SceneManagement; // 씬을 다시 불러오거나 클리어 처리를 위해 필요

public class GoalPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 포탈에 닿았을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("🎉 축하합니다! 게임 클리어! 🎉");

            // 과제 제출용 간단한 처리: 콘솔에 클리어 띄우고 1초 뒤 게임 재시작 (또는 에디터 일시정지)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 중지
#else
            Application.Quit(); // 빌드된 게임일 경우 종료
#endif
        }
    }
}