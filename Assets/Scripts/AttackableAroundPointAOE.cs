using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableAroundPointAOE : MonoBehaviour
{
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private Map m_Map;

    private void Awake()
    {
        m_Map = FindObjectOfType<Map>();
    }

    public void Attack()
    {
        for (int i = m_MapObject.Pos.x - m_Range; i < m_MapObject.Pos.x + m_Range + 1; i++)
        {
            for (int j = m_MapObject.Pos.y - m_Range; j < m_MapObject.Pos.y + m_Range + 1; j++)
            {
                if (m_Map.GetMapObjectByVector(new Vector2Int(i,j))!= null && m_Map.GetMapObjectByVector(new Vector2Int(i,j))!= m_MapObject)
                {
                    if (m_Map.GetMapObjectByVector(new Vector2Int(i, j)).GetComponent<Destructible>() != null)
                    {
                        print(m_Map.GetMapObjectByVector(new Vector2Int(i, j)));
                        m_Map.GetMapObjectByVector(new Vector2Int(i, j)).GetComponent<Destructible>().CurrentHP-= m_Damage;
                    }
                }
                
            }
        }
    }
    
}
