using System;
using Trisibo;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


public class RoomDoor : MonoBehaviour
{
    private bool _usable;
    private Collider2D _door_area;
    private Action action;

    [SerializeField] SceneField nextRoom;
    
   
    void Start()
    {
        
        
        _door_area = GetComponent<Collider2D>();
        Assert.IsNotNull(_door_area);
        // Assert.IsTrue(_door_area.isTrigger);
        // string[] scenes = EditorBuildSettings.scenes
        //     .Where( scene => scene.enabled )
        //     .Select( scene => scene.path )
        //     .ToArray();

        // foreach (var scene in scenes)
        // {
        //     Debug.Log($"scene: {scene}");
        // }
    }

    private void OnEnable()
    {
        RoomManager.OnRoomComplete += MakeDoorUsable;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomComplete -= MakeDoorUsable;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (player.CompareTag("Player") && _usable)
        {
            action = () =>
            {
                //Debug.Log("player can goto next room");
                if (Input.GetKey(KeyCode.E))
                {
                    // TODO: put logic for going to next scene
                    //      RoomManager.OnRoomExit?.Invoke();
                    //Debug.Log("goto next room");
                    // TODO: store global state e.g. weapon projectile health etc.
                    
                    SceneManager.LoadScene(nextRoom.BuildIndex);
                }
            };
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        action = () => { };
    }
    
    private void MakeDoorUsable(DoorPreviewController.RoomType room1, DoorPreviewController.RoomType room2) => _usable = true;
    
    void Update()
    {
        action?.Invoke();
    }
}
