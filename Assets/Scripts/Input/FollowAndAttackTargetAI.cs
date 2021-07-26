using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FollowAndAttackTargetAI : BaseInput
{
    [Header("Setup")]
    [Range(0,1)] [SerializeField] private float actionDelay;
    [SerializeField] private MapObject mapObject;
    [SerializeField] private MapObject target;
    [SerializeField] private AttackableInDirection attackable;
    [SerializeField] private bool ignoreTraps;

    private List<Vector2Int> closestWay;
    private List<Vector2Int> verAndHorVectors;
    private bool hitAlready;

    private void Awake()
    {
        target = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        verAndHorVectors = new List<Vector2Int>();
        verAndHorVectors.Add(Vector2Int.up);
        verAndHorVectors.Add(Vector2Int.left);
        verAndHorVectors.Add(Vector2Int.down);
        verAndHorVectors.Add(Vector2Int.right);
    }

    public override void Act()
    {
        CurrentlyActiveObjects.Add(this);
        Invoke("ActWithDelay", actionDelay);
    }

    private void ActWithDelay()
    {
        hitAlready = false;
        TryToHitTarget();
        if (!hitAlready)
        {
            TryToComeCloserToTarget();
        }
        CurrentlyActiveObjects.Remove(this);
    }

    private void TryToHitTarget()
    {
        foreach (var item in verAndHorVectors)
        {
            if (attackable.CheckAttackTargetIsPossible(item, target))
            {
                attackable.Attack(item);
                hitAlready = true;
            }
        }
    }

    private void TryToComeCloserToTarget()
    {
        closestWay = mapObject.Map.Pathfinder.FindWay(mapObject, target, ignoreTraps);

        if (closestWay != null && 
            mapObject.MovableModule.DefaultStepCost <= mapObject.ActionPointsContainerModule.CurrentPoints &&
            closestWay.Count > 1)

        {
            mapObject.MovableModule.Move(mapObject.Pos, closestWay[0]);
        }
        else
        {
            mapObject.SkipTurnModule1.SkipTurn();
        }
    }
}
