﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAndAttackTargetAI : BaseAI
{
    [Header("Setup")]
    [Range(0,1)] [SerializeField] private float m_ActionDelay;
    [SerializeField] private MapObject m_ThisMapObject;
    [SerializeField] private MapObject m_Target;
    [SerializeField] private Attackable m_Attackable;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private Movable m_Movement;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;

    private List<Vector2Int> m_VerAndHorVectors;

    private void Reset()
    {
        m_Target = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        m_ThisMapObject = GetComponent<MapObject>();
        m_Attackable = GetComponent<Attackable>();
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_Movement = GetComponent<Movable>();
        m_Map = FindObjectOfType<Map>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
    }

    private void Start()
    {
        m_VerAndHorVectors = new List<Vector2Int>();
        m_VerAndHorVectors.Add(Vector2Int.up);
        m_VerAndHorVectors.Add(Vector2Int.left);
        m_VerAndHorVectors.Add(Vector2Int.down);
        m_VerAndHorVectors.Add(Vector2Int.right);
    }

    public override void DoSomething()
    {
        Invoke("DoSomethingWithDelay", m_ActionDelay);
        
    }

    public void DoSomethingWithDelay()
    {
        Vector2Int closestPointToPlayer = FindClosestPointToPlayer();

        if (m_Map.GetMapObjectByVector(m_ThisMapObject.Pos + closestPointToPlayer) == m_Target)
        {
            if (m_ActionPointsContainer.CurrentPoints >= m_Attackable.ActionCost)
            {
                m_Attackable.Attack(closestPointToPlayer);
            }
            else
            {
                print(transform.name + "Not enough action points to hit player");
                m_ActiveObjectsQueue.SkipTheTurn();
            }
        }
        else
        {
            m_Movement.Move(closestPointToPlayer);
        }
    }

    public Vector2Int FindClosestPointToPlayer()
    {
        Vector2Int closestPoint = Vector2Int.up * 999;
        MapObject temp;

        foreach (var item in m_VerAndHorVectors)
        {
            temp = m_Map.Cells[(m_ThisMapObject.Pos + item).x, (m_ThisMapObject.Pos + item).y];

            if (temp == null || temp == m_Target)
            {
                if ((m_Target.Pos - (m_ThisMapObject.Pos + item)).magnitude < (m_Target.Pos - (m_ThisMapObject.Pos + closestPoint)).magnitude)
                {
                    closestPoint = item;
                }
            }
        }

        return closestPoint;
    }
}
