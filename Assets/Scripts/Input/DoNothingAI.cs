using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingAI : BaseInput
{
    [SerializeField] private MapObject m_MapObject;
    public override void DoSomething()
    {
        m_MapObject.ActiveObjectsQueue.SkipTheTurn();
    }
}
