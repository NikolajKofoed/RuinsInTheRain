using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Find the player when loading a new scene, and sets the camera to follow
/// </summary>
public class CameraController : Singleton<CameraController>
{
    private CinemachineCamera cinemachineCamera;

    private void Start()
    {
        SetPlayerCameraFollow();
    }
    public void SetPlayerCameraFollow()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = Player2D.Instance.transform;
        }
    }
}
