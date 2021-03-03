using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAI : BaseAI
{
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private ActiveObjectsQueue m_Queue;
    
    public override void DoSomething()
    {
        if (m_ActionPointsContainer.CurrentPoints > 2)
        {
            GetComponent<Destructible>().CurrentHP -= 10000;
        }

        else
        {
            m_Queue.SkipTurnWithAnimation();   
        }
    }

    private void Awake()
    {
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
    }
}
