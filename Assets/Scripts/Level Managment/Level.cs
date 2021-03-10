using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Vector2Int m_PlayerSpawnCoordinates;
    [SerializeField] private Vector3 m_CameraPosition;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_Queue;

    public ActiveObjectsQueue Queue
    {
        get => m_Queue;
        set => m_Queue = value;
    }

    public Map Map
    {
        get => m_Map;
        set => m_Map = value;
    }

    public Vector3 CameraPosition
    {
        get => m_CameraPosition;
        set => m_CameraPosition = value;
    }

    public Vector2Int PlayerSpawnCoordinates
    {
        get => m_PlayerSpawnCoordinates;
        set => m_PlayerSpawnCoordinates = value;
    }

    private void Awake()
    {
        FindObjectOfType<Camera>().transform.position = m_CameraPosition;
        var player = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        player.transform.position = new Vector3(m_PlayerSpawnCoordinates.x, m_PlayerSpawnCoordinates.y, 0);
        player.Pos = m_PlayerSpawnCoordinates;
    }
}
