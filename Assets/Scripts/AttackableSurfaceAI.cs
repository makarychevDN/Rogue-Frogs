using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableSurfaceAI : BaseAI
{
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private List<Sprite> m_Sprites;
    [SerializeField] private int m_TurnsBeforeHit;
    [SerializeField] private ActiveObjectsQueue m_Queue;

    private void Start()
    {
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
    }
}
