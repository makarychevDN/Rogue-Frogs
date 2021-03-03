using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawnerAI : BaseAI
{
    [SerializeField] private MapObject m_SpawnObjectPrefab;
    [SerializeField] private MapObject m_Target;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_Queue;

    private List<Vector2Int> m_VectorsToCheckPlace;

    private void Start()
    {
        m_Target = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        m_VectorsToCheckPlace = new List<Vector2Int>();
        m_VectorsToCheckPlace.Add(Vector2Int.up);
        m_VectorsToCheckPlace.Add(Vector2Int.down);
        m_VectorsToCheckPlace.Add(Vector2Int.left);
        m_VectorsToCheckPlace.Add(Vector2Int.right);
    }

    public override void DoSomething()
    {
        var temp = FindEmptyPlacesAroundTarget();
        if (temp != Vector2Int.zero && m_ActionPointsContainer.CurrentPoints >= 3)
        {
            m_ActionPointsContainer.CurrentPoints -= 3;
            Spawn(temp);
            m_Queue.SkipTheTurn();
        }
        else
        {
            m_Queue.SkipTurnWithAnimation();
        }
    }

    public void Spawn(Vector2Int spawnPos)
    {
        var temp = Instantiate(m_SpawnObjectPrefab);
        m_Map.SetMapObjectByVector(spawnPos,temp);
        temp.transform.position = new Vector3(spawnPos.x, spawnPos.y);
        temp.Pos = spawnPos;
        m_Queue.AddObjectInQueue(temp);
    }

    public Vector2Int FindEmptyPlacesAroundTarget()
    {
        List<Vector2Int> results = new List<Vector2Int>();

        foreach (var temp in m_VectorsToCheckPlace)
        {
            if (m_Map.GetMapObjectByVector(m_Target.Pos + temp) == null)
            {
                results.Add(temp);
            }
        }

        if(results.Count != 0)
            return results[Random.Range(0, results.Count - 1)] + m_Target.Pos;
        else
        {
            return Vector2Int.zero;
        }
    }
}
