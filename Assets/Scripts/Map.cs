using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{    
    [SerializeField] private MapObject m_TopRightWall;
    private MapObject[,] m_Cells;

    public MapObject[,] Cells { get => m_Cells; set => m_Cells = value; }

    void Start()
    {
        m_Cells = new MapObject[m_TopRightWall.Pos.x + 1, m_TopRightWall.Pos.y + 1];
        var temp = FindObjectsOfType<MapObject>();

        foreach (var item in temp)
        {
            m_Cells[item.Pos.x, item.Pos.y] = item;
        }
    }

    public MapObject GetMapObjectByVector(Vector2Int coordinates)
    {
        return m_Cells[coordinates.x, coordinates.y];
    }
}
