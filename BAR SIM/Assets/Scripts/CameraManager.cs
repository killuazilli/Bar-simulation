using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera followCamera;
    [SerializeField] private Camera interactionCamera;

    [Header("Follow Camera")]
    [SerializeField] private FollowCameraController followController;

    private void Awake()
    {
        ShowFollowCamera();
    }

    public void FollowNPC(NPCController npc)
    {
        if (npc == null)
            return;

        followController.SetTarget(npc.transform);

        ShowFollowCamera();
    }

    public void ShowFollowCamera()
    {
        followCamera.gameObject.SetActive(true);
        interactionCamera.gameObject.SetActive(false);
    }

    public void ShowInteractionCamera()
    {
        followCamera.gameObject.SetActive(false);
        interactionCamera.gameObject.SetActive(true);
    }

    public void StopFollowing()
    {
        followController.ClearTarget();
    }
}