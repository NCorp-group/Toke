using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MakeVisibleOnRoomEnd : MonoBehaviour
{
    private TilemapRenderer _tilemapRenderer;
    private void OnEnable()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        RoomManager.OnRoomComplete += EnableTileMapRenderer;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomComplete -= EnableTileMapRenderer;
    }

    private void EnableTileMapRenderer(DoorPreviewController.RoomType room1, DoorPreviewController.RoomType room2)
    {
        _tilemapRenderer.enabled = true;
    }
}
