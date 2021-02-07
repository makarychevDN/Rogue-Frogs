using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetAI : BaseAI
{
    [SerializeField] private MapObject m_Target;

    private Movable m_Movement;
    private Map m_Map;
    private ActiveObjectsQueue m_charactersStack;
    private List<Vector2Int> m_VerAndHorVectors;

    private void Start()
    {
        m_Movement = GetComponent<Movable>();
        m_MapObject = GetComponent<MapObject>();

        m_Map = FindObjectOfType<Map>();
        m_charactersStack = FindObjectOfType<ActiveObjectsQueue>();

        m_VerAndHorVectors = new List<Vector2Int>();
        m_VerAndHorVectors.Add(Vector2Int.up);
        m_VerAndHorVectors.Add(Vector2Int.left);
        m_VerAndHorVectors.Add(Vector2Int.down);
        m_VerAndHorVectors.Add(Vector2Int.right);
    }

    public override void DoSomething()
    {
        base.DoSomething();
        Vector2Int closestPointToPlayer = FindClosestPointToPlayer();

        if(m_Map.GetMapObjectByVector(m_MapObject.Pos + closestPointToPlayer) == m_Target)
        {
            m_charactersStack.SkipTheTurn();
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
            temp = m_Map.Cells[(m_MapObject.Pos + item).x, (m_MapObject.Pos + item).y];

            if (temp == null || temp == m_Target)
            {
                if ((m_Target.Pos - (m_MapObject.Pos + item)).magnitude < (m_Target.Pos - (m_MapObject.Pos + closestPoint)).magnitude)
                {
                    closestPoint = item;
                }
            }
        }

        return closestPoint;
    }
}
