using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawnerAI : BaseInput
{
    [SerializeField] private MapObject m_ThisMapObject;
    [SerializeField] private MapObject m_SpawnObjectPrefab;
    [SerializeField] private MapObject m_Target;
    [SerializeField] private float m_AninationTime;

    private List<Vector2Int> m_VectorsToCheckPlace;
    private Vector2Int m_CurrentSpawnPos;

    private void Awake()
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
        m_ThisMapObject.ActiveObjectsQueue.AddToActiveObjectsList(this);
        m_CurrentSpawnPos = FindEmptyPlacesAroundTarget();
        if (m_CurrentSpawnPos != Vector2Int.zero && m_ThisMapObject.ActionPointsContainerModule.CurrentPoints >= 3)
        {
            m_ThisMapObject.ActionPointsContainerModule.CurrentPoints -= 3;
            m_ThisMapObject.AnimationStateMashine.ActivateAttackAnim();
            Invoke("SpawnAndPush",m_AninationTime);
        }
        else
        {
            m_ThisMapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
            GetComponent<SkipTurnModule>().SkipTurn();
        }
    }

    public void Spawn()
    {
        m_ThisMapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
        var temp = Instantiate(m_SpawnObjectPrefab);
        m_ThisMapObject.Map.SetMapObjectByVector(m_CurrentSpawnPos,temp);
        temp.transform.position = new Vector3(m_CurrentSpawnPos.x, m_CurrentSpawnPos.y);
        temp.Pos = m_CurrentSpawnPos;
        m_ThisMapObject.ActiveObjectsQueue.AddObjectInQueue(temp);
        m_ThisMapObject.AnimationStateMashine.ActivateStayAnim();
        m_ThisMapObject.ActiveObjectsQueue.SkipTheTurn();
    }
    
    public void SpawnAndPush()
    {
        m_ThisMapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
        var temp = Instantiate(m_SpawnObjectPrefab);
        temp.transform.position = transform.position;
        temp.Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
        temp.GetComponent<Movable>().Move(temp.Pos,m_CurrentSpawnPos,AnimType.cos,1f,0,false,true);
        m_ThisMapObject.ActiveObjectsQueue.AddObjectInQueue(temp);
        m_ThisMapObject.AnimationStateMashine.ActivateStayAnim();
        m_ThisMapObject.ActiveObjectsQueue.SkipTheTurn();
    }

    public Vector2Int FindEmptyPlacesAroundTarget()
    {
        List<Vector2Int> results = new List<Vector2Int>();

        foreach (var temp in m_VectorsToCheckPlace)
        {
            if (m_ThisMapObject.Map.GetMapObjectByVector(m_Target.Pos + temp) == null)
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
