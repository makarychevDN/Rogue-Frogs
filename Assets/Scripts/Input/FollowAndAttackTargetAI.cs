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

    private List<Vector2Int> m_ClosestWay;
    private List<Vector2Int> m_VerAndHorVectors;
    private bool hitAlready;

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
        hitAlready = false;
        TryToHitTarget();

        if (!hitAlready)
        {
            TryToComeCloserToTarget();
        }

        m_ThisMapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);

        /*var path = m_ThisMapObject.Map.Pathfinder.FindWay(m_ThisMapObject, m_Target);
        print(path != null);
        StringBuilder sb = new StringBuilder();

        foreach (var item in path)
        {
            sb.Append(item).Append(" ");
        }

        print(sb.ToString());*/
    }

    private void TryToHitTarget()
    {
        foreach (var item in m_VerAndHorVectors)
        {
            if (m_Attackable.CheckAttackTargetIsPossible(item, m_Target))
            {
                m_Attackable.Attack(item);
                hitAlready = true;
            }
        }
    }

    private void TryToComeCloserToTarget()
    {
        m_ClosestWay = m_ThisMapObject.Map.Pathfinder.FindWay(m_ThisMapObject, m_Target);

        if (m_ClosestWay != null && 
            m_ThisMapObject.MovableModule.DefaultStepCost <= m_ThisMapObject.ActionPointsContainerModule.CurrentPoints &&
            m_ClosestWay.Count > 1)

        {
            m_ThisMapObject.MovableModule.Move(m_ThisMapObject.Pos, m_ClosestWay[0]);
        }
        else
        {
            m_ThisMapObject.SkipTurnModule1.SkipTurn();
        }
    }
}
