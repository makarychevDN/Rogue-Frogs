using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAI : BaseAI
{
    public override void DoSomething()
    {
        FindObjectOfType<ActiveObjectsQueue>().SkipTheTurn();
    }
}
