using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private Sprite m_Sprite; //todo - вынести или вырезать нахой
    [SerializeField] private GameObject m_SkipTurnAnimation;
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

    void Awake()
    {
        m_Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }
}
