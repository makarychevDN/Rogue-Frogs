using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingAI : BaseAI
{
    [SerializeField] private ActiveObjectsQueue m_Queue;
    public override void DoSomething()
    {
        m_Queue.SkipTheTurn();
    }
    
    private void Awake()
    {
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
    }
}
