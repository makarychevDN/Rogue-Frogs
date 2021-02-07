using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    private Vector2Int pos;

    public Vector2Int Pos { get => pos; set => pos = value; }

    void Awake()
    {
        pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }

    private void Reset()
    {
        pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
    }
}
