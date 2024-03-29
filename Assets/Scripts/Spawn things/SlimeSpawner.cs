﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class SlimeSpawner : Spawner
{
    private List<Vector2Int> m_SpawnDirections;
    private Map m_Map;

    private void Awake()
    {
        m_Map = FindObjectOfType<Map>();
        m_SpawnDirections = new List<Vector2Int>();
        m_SpawnDirections.Add(Vector2Int.down);
        m_SpawnDirections.Add(Vector2Int.left);
        m_SpawnDirections.Add(Vector2Int.right);
        m_SpawnDirections.Add(Vector2Int.up);
    }

    public override void Spawn(Transform spawnPostransform)
    {

        base.Spawn(spawnPostransform);

        List<Vector2Int> emptyCellsPositions = new List<Vector2Int>();
        
        foreach (var VARIABLE in m_SpawnDirections)
        {
            if (m_Map.GetMapObjectByVector(m_SpawnPos + VARIABLE) == null)
            {
                emptyCellsPositions.Add(VARIABLE);   
            }
        }
        
        if (emptyCellsPositions.Count != 0)
        {
            //Spawn(emptyCellsPositions[Random.Range(0, emptyCellsPositions.Count - 1)] + m_SpawnPos);
            m_SpawnPos = new Vector2Int((int) spawnPostransform.position.x, (int) spawnPostransform.position.y);
            var temp = Instantiate(m_SpawnObjectPrefab);
            temp.transform.position = new Vector3(m_SpawnPos.x, m_SpawnPos.y);
            temp.Pos = m_SpawnPos;
            temp.GetComponent<Movable>().Move(emptyCellsPositions[Random.Range(0, emptyCellsPositions.Count - 1)],
                AnimType.cos, 0.1f, 0, false, true);
            FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueueBeforeTarget(m_ThisMapObject, temp);
        }
        
    }
}
