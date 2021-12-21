using System.Collections.Generic;
using UnityEngine;

public class DisableCollidersOnRoomEnd : MonoBehaviour
{
    private List<BoxCollider2D> colliders;
    // Start is called before the first frame update

    private void OnEnable()
    {
        RoomManager.OnRoomComplete += (_, _) =>
        {
            //Debug.Log($"in callback colliders = {colliders.Count}");
            if (colliders is not null)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }
        };
    }

    void Start()
    {
        colliders = new List<BoxCollider2D>(GetComponentsInChildren<BoxCollider2D>());
    }
}
