using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Sprite m_Sprite; //todo - вынести или вырезать нахой. udp. пока придержим

    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private Movable m_Movable;
    [SerializeField] private Destructible m_Destructible;
    [SerializeField] private GameObject m_SkipTurnAnimation; // todo удалить
    [SerializeField] private SkipTurnModule m_SkipTurnModule;
    [SerializeField] private AnimationsStateMashine m_AnimationsStateMashine;
    [SerializeField] private MapObjSpriteRotator m_MapObjSpriteRotator;
    [SerializeField] private ShowUI m_ShowUI;

    [Header("Level References")] 
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_Queue;

    #region Properties
    
    private Vector2Int m_Pos;
    public Vector2Int Pos
    {
        get => m_Pos; 
        set => m_Pos = value; 
        
    }

    public Sprite Sprite
    {
        get => m_Sprite;
        set => m_Sprite = value;
    }

    public GameObject SkipTurnAnimation
    {
        get => m_SkipTurnAnimation;
        set => m_SkipTurnAnimation = value;
    }

    public ActionPointsContainer ActionPointsContainerModule
    {
        get => m_ActionPointsContainer;
        set => m_ActionPointsContainer = value;
    }

    public Movable MovableModule
    {
        get => m_Movable;
        set => m_Movable = value;
    }

    public Destructible DestructibleModule
    {
        get => m_Destructible;
        set => m_Destructible = value;
    }

    public SkipTurnModule SkipTurnModule1
    {
        get => m_SkipTurnModule;
        set => m_SkipTurnModule = value;
    }

    public MapObjSpriteRotator SpriteRotator
    {
        get => m_MapObjSpriteRotator;
        set => m_MapObjSpriteRotator = value;
    }

    public ShowUI ShowUI
    {
        get => m_ShowUI;
        set => m_ShowUI = value;
    }

    public Map Map
    {
        get => m_Map;
        set => m_Map = value;
    }

    public ActiveObjectsQueue ActiveObjectsQueue
    {
        get => m_Queue;
        set => m_Queue = value;
    }

    public AnimationsStateMashine AnimationStateMashine
    {
        get => m_AnimationsStateMashine;
        set => m_AnimationsStateMashine = value;
    }

    #endregion
    void Awake()
    {
        m_Map = FindObjectOfType<Map>();
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
        m_Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }
}
