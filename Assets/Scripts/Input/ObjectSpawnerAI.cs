using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawnerAI : BaseInput
{
    [SerializeField] private MapObject m_SpawnObjectPrefab;
    [SerializeField] private MapObject m_Target;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_Queue;
    [SerializeField] private float m_AninationTime;
    [SerializeField] private AnimationsStateMashine m_AnimationsStateMashine;

    private List<Vector2Int> m_VectorsToCheckPlace;
    private Vector2Int m_CurrentSpawnPos;

    private void Awake()
    {
        m_Target = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        m_Map = FindObjectOfType<Map>();
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
        m_VectorsToCheckPlace = new List<Vector2Int>();
        m_VectorsToCheckPlace.Add(Vector2Int.up);
        m_VectorsToCheckPlace.Add(Vector2Int.down);
        m_VectorsToCheckPlace.Add(Vector2Int.left);
        m_VectorsToCheckPlace.Add(Vector2Int.right);
    }

    public override void DoSomething()
    {
        m_CurrentSpawnPos = FindEmptyPlacesAroundTarget();
        if (m_CurrentSpawnPos != Vector2Int.zero && m_ActionPointsContainer.CurrentPoints >= 3)
        {
            m_ActionPointsContainer.CurrentPoints -= 3;
            m_AnimationsStateMashine.ActivateAttackAnim();
            Invoke("Spawn",m_AninationTime);
        }
        else
        {
            m_Queue.SkipTurnWithAnimation();
        }
    }

    public void Spawn()
    {
        var temp = Instantiate(m_SpawnObjectPrefab);
        m_Map.SetMapObjectByVector(m_CurrentSpawnPos,temp);
        temp.transform.position = new Vector3(m_CurrentSpawnPos.x, m_CurrentSpawnPos.y);
        temp.Pos = m_CurrentSpawnPos;
        m_Queue.AddObjectInQueue(temp);
        m_AnimationsStateMashine.ActivateStayAnim();
        m_Queue.SkipTheTurn();
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
