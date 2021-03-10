using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsContainer : MonoBehaviour
{
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private Movable m_Movable;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private Destructible m_Destructible;
    [SerializeField] private MapObjSpriteRotator m_SpriteRotator;
    [SerializeField] private InitiativeContainer m_InitiativeContainer;
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private List<AttackableInDirection> m_Attackables;
    [SerializeField] private AnimationsStateMashine m_AnimationsStateMashine;
    
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;

    private void Awake()
    {
        m_MapObject = GetComponent<MapObject>();
        m_Movable = GetComponent<Movable>();
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_Destructible = GetComponent<Destructible>();
        m_SpriteRotator = GetComponentInChildren<MapObjSpriteRotator>();
        m_InitiativeContainer = GetComponent<InitiativeContainer>();
        m_PlayerInput = GetComponent<PlayerInput>();
        
        m_Attackables = new List<AttackableInDirection>();
        foreach (var VARIABLE in m_Attackables)
        {
            m_Attackables.Add(VARIABLE);
        }

        m_AnimationsStateMashine = GetComponentInChildren<AnimationsStateMashine>();
        
        m_Map = FindObjectOfType<Map>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
    }

    private void Reset()
    {
        m_Map = FindObjectOfType<Map>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
    }
}
