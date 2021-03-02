﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{    
    [SerializeField] private MapObject m_TopRightWall;
    private MapObject[,,] m_Cells;

    public MapObject[,,] Cells { get => m_Cells; set => m_Cells = value; }

    void Start()
    {
        m_Cells = new MapObject[m_TopRightWall.Pos.x + 1, m_TopRightWall.Pos.y + 1, 2];
        var tempMapObjects = FindObjectsOfType<MapObject>();

        foreach (var item in tempMapObjects)
        {
            //MapObjectSurface tempMapObj = (MapObjectSurface) item;
            if(item is MapObjectSurface)
                m_Cells[item.Pos.x, item.Pos.y, 1] = item;
            else
                m_Cells[item.Pos.x, item.Pos.y, 0] = item;
        }
    }

    public MapObject GetMapObjectByVector(Vector2Int coordinates)
    {
        return m_Cells[coordinates.x, coordinates.y, 0];
    }
    
    public MapObjectSurface GetSurfaceByVector(Vector2Int coordinates)
    {
        return (MapObjectSurface)m_Cells[coordinates.x, coordinates.y, 1];
    }
    
    public void SetMapObjectByVector(Vector2Int coordinates, MapObject mapObject)
    {
        m_Cells[coordinates.x, coordinates.y, 0] = mapObject;
    }

    private void Update() //todo remove this shit vvv
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            print(m_Cells[2, 2, 0]);
        }
    }
}
