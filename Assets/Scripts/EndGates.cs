using UnityEngine;

public class EndGates : MonoBehaviour
{
    private void OnEnable()
    {
        RoomManager.OnRoomComplete += LowerGates;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomComplete -= LowerGates;
    }

    private void LowerGates(DoorPreviewController.RoomType room1, DoorPreviewController.RoomType room2)
    {
        gameObject.SetActive(false);
    }
}
