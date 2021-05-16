using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FollowAndAttackTargetAI : BaseInput
{
    [Header("Setup")]
    [Range(0,1)] [SerializeField] private float m_ActionDelay;
    [SerializeField] private MapObject m_ThisMapObject;
    [SerializeField] private MapObject m_Target;
    [SerializeField] private AttackableInDirection m_Attackable;

    private List<Vector2Int> m_VerAndHorVectors;

    private void Awake()
    {
        m_Target = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        m_VerAndHorVectors = new List<Vector2Int>();
        m_VerAndHorVectors.Add(Vector2Int.up);
        m_VerAndHorVectors.Add(Vector2Int.left);
        m_VerAndHorVectors.Add(Vector2Int.down);
        m_VerAndHorVectors.Add(Vector2Int.right);
    }

    public override void DoSomething()
    {
        m_ThisMapObject.ActiveObjectsQueue.AddToActiveObjectsList(this);
        Invoke("DoSomethingWithDelay", m_ActionDelay);
    }

    private void DoSomethingWithDelay()
    {
        Vector2Int closestPointToPlayer = FindClosestPointToPlayer();

        var path = m_ThisMapObject.Map.Pathfinder.FindWay(m_ThisMapObject, m_Target);
        print(path != null);
        StringBuilder sb = new StringBuilder();

        foreach (var item in path)
        {
            sb.Append(item).Append(" ");
        }

        print(sb.ToString());

        if (m_ThisMapObject.Map.GetMapObjectByVector(m_ThisMapObject.Pos + closestPointToPlayer) == m_Target)
        {
            if (m_ThisMapObject.ActionPointsContainerModule.CurrentPoints >= m_Attackable.ActionCost)
            {
                m_Attackable.Attack(closestPointToPlayer);
            }
            else
            {
                print(transform.name + "Not enough action points to hit player");
                GetComponent<SkipTurnModule>().SkipTurn();
            }
        }
        else
        {
            if (closestPointToPlayer != Vector2Int.zero && m_ThisMapObject.ActionPointsContainerModule.CurrentPoints >= m_ThisMapObject.MovableModule.DefaultStepCost)
            {
                m_ThisMapObject.MovableModule.Move(closestPointToPlayer);
            }
            else
            {
                GetComponent<SkipTurnModule>().SkipTurn();
            }
        }
        m_ThisMapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
    }

    public Vector2Int FindClosestPointToPlayer()
    {
        Vector2Int closestPoint = Vector2Int.zero;
        MapObject temp;

        foreach (var item in m_VerAndHorVectors)
        {
            temp = m_ThisMapObject.Map.GetMapObjectByVector(m_ThisMapObject.Pos + item);

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
