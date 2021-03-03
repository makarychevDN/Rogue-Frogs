using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private Sprite m_Sprite;
    [SerializeField] private GameObject m_SkipTurnAnimation;
    private Vector2Int pos;

    public Vector2Int Pos
    {
        get => pos; 
        set => pos = value; 
        
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
        pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }
}
