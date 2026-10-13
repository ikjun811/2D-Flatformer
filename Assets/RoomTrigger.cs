using UnityEngine;
using Cinemachine;

public class RoomTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera roomCamera; // 이 방에 할당된 카메라

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 모든 가상 카메라의 우선순위를 낮추고, 이 방의 카메라만 우선순위를 높임
            // (시네마신은 Priority가 가장 높은 카메라를 메인으로 비춤)

            // 씬에 있는 모든 VirtualCamera 찾기
            CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var cam in allCameras)
            {
                cam.Priority = 0; // 전부 우선순위 낮춤
            }

            // 이 방의 카메라만 우선순위를 높여서 전환됨
            if (roomCamera != null)
            {
                roomCamera.Priority = 10;
                Debug.Log("방 이동: 카메라 전환 완료");
            }
        }
    }
}