﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableAroundPointAOE : ActivatableBehavior
{
    [Header("References Setup")]
    [SerializeField] private MapObject m_MapObject;
    [Header("Setup")] 
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private float m_DefaultDelay;

    public void Attack()
    {
        for (int i = m_MapObject.Pos.x - m_Range; i < m_MapObject.Pos.x + m_Range + 1; i++)
        {
            for (int j = m_MapObject.Pos.y - m_Range; j < m_MapObject.Pos.y + m_Range + 1; j++)
            {
                if (m_MapObject.Map.GetMapObjectByVector(new Vector2Int(i,j))!= null && m_MapObject.Map.GetMapObjectByVector(new Vector2Int(i,j))!= m_MapObject)
                {
                    if (m_MapObject.Map.GetMapObjectByVector(new Vector2Int(i, j)).GetComponent<Destructible>() != null)
                    {
                        m_MapObject.Map.GetMapObjectByVector(new Vector2Int(i, j)).GetComponent<Destructible>().CurrentHP-= m_Damage;
                    }
                }
                
            }
        }

        ActiveNow = false;
    }

    public void AttackWithDelay(float delay)
    {
        ActiveNow = true;
        Invoke("Attack", delay);
    }
    
    public void AttackWithDelay()
    {
        AttackWithDelay(m_DefaultDelay);
    }
    
}
