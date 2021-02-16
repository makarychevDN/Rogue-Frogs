using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    //[SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite m_Sprite;
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

    private void Reset()
    {
        //m_SpriteRenderer.sprite = m_Sprite;
    }

    void Awake()
    {
        pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }
}
