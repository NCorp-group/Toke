using UnityEngine;

public class DoorPreviewController : MonoBehaviour
{
    public const string ROOM_TYPE = "room_type";
    
    public const int DROP_START = 1;
    public const int DROP_END = 4;
    public enum RoomType
    {
        UNASSIGNED,
        ITEM_DROP,
        PENNINGAR_DROP,
        HEALTH_DROP,
        SHOP,
        BOSS
    }

    [SerializeField]
    private int roomId;

    public RoomType roomType = RoomType.UNASSIGNED;

    [SerializeField]
    private Sprite[] sprites;

    private void OnEnable()
    {
        InteractableArea.OnDoorInteraction += RoomTypeToPlayerPrefs;
        RoomManager.OnRoomComplete += ActivatePreview;
    }

    private void OnDisable()
    {
        InteractableArea.OnDoorInteraction -= RoomTypeToPlayerPrefs;
        RoomManager.OnRoomComplete -= ActivatePreview;
    }

    public static bool writtenToPlayerPrefs = false;
    private void RoomTypeToPlayerPrefs(RoomType obj)
    {
        PlayerPrefs.SetInt(ROOM_TYPE, (int) roomType);
        writtenToPlayerPrefs = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        writtenToPlayerPrefs = false;
        if (roomType == RoomType.UNASSIGNED)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void ActivatePreview(RoomType room1, RoomType room2)
    {
        //Debug.Log("room1: " + room1);
        //Debug.Log("room2: " + room2);
        //Debug.Log("roomType: " + roomType + " " + (int) roomType);
        if (roomType == RoomType.UNASSIGNED)
        {
            roomType = roomId switch
            {
                1 => room1,
                2 => room2,
                _ => RoomType.PENNINGAR_DROP
            };
        }
        //Debug.Log("roomType: " + roomType + " " + (int) roomType);

        var sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.sprite = sprites[(int) roomType - 1];
    }
}
