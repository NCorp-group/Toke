using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPreviewController : MonoBehaviour
{
    public enum RoomType
    {
        UNASSIGNED,
        ITEM_DROP,
        PENNINGAR_DROP,
        SHOP,
        BOSS
    }

    [SerializeField]
    private int roomId;
    
    public RoomType roomType = RoomType.UNASSIGNED;
    
    [SerializeField]
    private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        RoomManager.OnRoomComplete += ActivatePreview;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void ActivatePreview(RoomType room1, RoomType room2)
    {
        if (roomType == RoomType.UNASSIGNED)
        {
            switch (roomId)
            {
                case 1:
                    roomType = room1;
                    break;
                case 2:
                    roomType = room2;
                    break;
                default:
                    roomType = RoomType.PENNINGAR_DROP;
                    break;
            }
        }
        Debug.Log("roomType: " + roomType);
        
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = sprites[(int) roomType];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}