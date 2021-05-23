using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableSurfaceAI : BaseInput
{
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private List<Sprite> m_Sprites;
    [SerializeField] private int m_TurnsBeforeHit;
    [SerializeField] private int m_Damage;
    
    private int m_Count;
    
    public override void Act()
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
        m_MapObject.ActiveObjectsQueue.SkipTheTurn();

        TryToDealDamage();
    }

    public void TryToDealDamage()
    {
        if (m_Count == m_TurnsBeforeHit - 1)
        {
            if (m_MapObject.Map.GetMapObjectByVector(m_MapObject.Pos) != null)
            {
                var temp = m_MapObject.Map.GetMapObjectByVector(m_MapObject.Pos).GetComponent<Destructible>();

                if (temp != null)
                {
                    temp.CurrentHP -= m_Damage;
                }
            }
        }
    }
}
