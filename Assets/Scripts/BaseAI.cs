using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected MapObject m_MapObject;
    protected ActionPointsContainer m_ActionPointsContainer;

    protected virtual void Start()
    {
        m_MapObject = GetComponent<MapObject>();
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
    }

    public virtual void DoSomething()
    {
        //print(transform.name + "'s Turn!");
    }
}
