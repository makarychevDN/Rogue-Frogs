using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAI : BaseInput
{
    [SerializeField] private MapObject m_MapObject;

    public override void DoSomething()
    {
        if (m_MapObject.ActionPointsContainerModule.CurrentPoints == 2)
        {
            m_MapObject.DestructibleModule.CurrentHP -= 10000;
        }

        else
        {
            m_MapObject.SkipTurnModule1.SkipTurn();
        }
    }
}
