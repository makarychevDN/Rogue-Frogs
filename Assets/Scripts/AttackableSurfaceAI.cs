using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableSurfaceAI : BaseAI
{
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private List<Sprite> m_Sprites;
    [SerializeField] private int m_TurnsBeforeHit;
    [SerializeField] private int m_Damage;
    [SerializeField] private ActiveObjectsQueue m_Queue;

    private void Start()
    {
        m_Map = FindObjectOfType<Map>();
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
    }

    private int m_Count;
    
    public override void DoSomething()
    {
        if (m_Count == m_TurnsBeforeHit-1)
        {
            m_Count = 0;
        }
        else
        {
            m_Count++;
        }
        
        m_SpriteRenderer.sprite = m_Sprites[m_Count];
        m_Queue.SkipTheTurn();

        TryToDealDamage();
    }

    public void TryToDealDamage()
    {
        if (m_Count == m_TurnsBeforeHit - 1)
        {
            if (m_Map.GetMapObjectByVector(m_MapObject.Pos) != null)
            {
                var temp = m_Map.GetMapObjectByVector(m_MapObject.Pos).GetComponent<Destructible>();

                if (temp != null)
                {
                    temp.CurrentHP -= m_Damage;
                }
            }
        }
    }
}
