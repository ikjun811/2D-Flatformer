using UnityEngine;
using Cinemachine;

public class RoomCameraZone : MonoBehaviour
{
    public CinemachineVirtualCamera assignedCamera; // 이 구역에 할당된 카메라

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. 씬에 있는 모든 가상 카메라를 찾아서 Priority를 0으로 낮춤
            CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var cam in allCameras)
            {
                cam.Priority = 0;
            }

            // 2. 이 구역의 카메라만 Priority를 10으로 높여서 메인으로 만듦
            if (assignedCamera != null)
            {
                assignedCamera.Priority = 10;
            }
        }
    }
}