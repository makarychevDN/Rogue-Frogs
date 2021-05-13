using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{    
    [SerializeField] private MapObject m_TopRightWall;
    private MapObject[,,] m_Cells;
    private PathFinder pahtfinder;
    private int sizeX, sizeY;

    public MapObject[,,] Cells { get => m_Cells; set => m_Cells = value; }
    public int SizeY { get => sizeY; set => sizeY = value; }
    public int SizeX { get => sizeX; set => sizeX = value; }

    void Start()
    {
        sizeX = m_TopRightWall.Pos.x + 1;
        sizeY = m_TopRightWall.Pos.y + 1;
        m_Cells = new MapObject[sizeX, sizeY, 2];
        var tempMapObjects = FindObjectsOfType<MapObject>();

        foreach (var item in tempMapObjects)
        {
            if(item is MapObjectSurface)
                m_Cells[item.Pos.x, item.Pos.y, 1] = item;
            else
                m_Cells[item.Pos.x, item.Pos.y, 0] = item;
        }

        pahtfinder = new PathFinder(this);
    }

    public MapObject GetMapObjectByVector(Vector2Int coordinates)
    {
        return m_Cells[coordinates.x, coordinates.y, 0];
    }
    
    public MapObjectSurface GetSurfaceByVector(Vector2Int coordinates)
    {
        return (MapObjectSurface)m_Cells[coordinates.x, coordinates.y, 1];
    }
    
    public void SetMapObjectByVector(Vector2Int coordinates, MapObject mapObject)
    {
        m_Cells[coordinates.x, coordinates.y, 0] = mapObject;
    }
}


public class PathFinder : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private PathFinderNode[,] nodesGrid;
    [SerializeField] private GameObject NodeVisualisation;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private int TargetX;
    [SerializeField] private int TargetY;
    [SerializeField] private int XstartPoint;
    [SerializeField] private int YStartPoint;

    List<PathFinderNode> parentNodes;
    List<PathFinderNode> childNodes;

    private List<Vector2Int> dirVectors;

    public PathFinder(Map map)
    {
        this.map = map;
        InitializeDirVectors();
        InitialaizeNodesGrid(map.SizeX, map.SizeY);
        FindAllNodesNeighbors(map.SizeX, map.SizeY);
    }


    public void InitializeDirVectors()
    {
        dirVectors = new List<Vector2Int>();
        dirVectors.Add(Vector2Int.up);
        dirVectors.Add(Vector2Int.right);
        dirVectors.Add(Vector2Int.down);
        dirVectors.Add(Vector2Int.left);
    }

    public void InitialaizeNodesGrid(int sizeX, int sizeY)
    {
        nodesGrid = new PathFinderNode[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                nodesGrid[i, j] = new PathFinderNode(i, j);

                if (map.Cells[i, j, 0] != null)
                {
                    nodesGrid[i, j].Busy = true;
                }
            }
        }
    }

    public void FindAllNodesNeighbors(int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (nodesGrid[i, j] != null)
                {
                    foreach (var temp in dirVectors)
                    {
                        if (i + temp.x > 0 && i + temp.x < sizeX && j + temp.y > 0 && j + temp.y < sizeY)
                        {
                            if (nodesGrid[i + temp.x, j + temp.y] != null)
                            {
                                nodesGrid[i, j].AddNeighbor(nodesGrid[i + temp.x, j + temp.y]);
                            }
                        }
                    }
                }
            }
        }
    }

    public void StartWave()
    {
        if (nodesGrid[XstartPoint, YStartPoint] != null)
        {
            childNodes = new List<PathFinderNode>();
            childNodes.Add(nodesGrid[XstartPoint, YStartPoint]);
            nodesGrid[XstartPoint, YStartPoint].UsedToPathFinding = true;

            while (childNodes.Count != 0)
            {
                parentNodes = childNodes;
                childNodes = new List<PathFinderNode>();

                foreach (var tempParent in parentNodes)
                {
                    foreach (var tempChild in tempParent.Neighbors)
                    {
                        if (!tempChild.UsedToPathFinding)
                        {
                            tempChild.Previous = tempParent;
                            childNodes.Add(tempChild);
                            tempChild.UsedToPathFinding = true;

                            if (tempChild.Pos == new Vector2Int(TargetX, TargetY))
                            {
                                var tempBackTrackNode = tempChild;

                                while (tempBackTrackNode.Pos != new Vector2Int(XstartPoint, YStartPoint))
                                {
                                    tempBackTrackNode = tempBackTrackNode.Previous;
                                }

                                return;
                            }
                        }
                    }
                }
            }

        }
    }
}

public class PathFinderNode
{
    private int Xpos;
    private int Ypos;
    private Vector2Int pos;
    private List<PathFinderNode> neighbors;
    private bool usedToPathFinding;
    private bool busy;

    private PathFinderNode previous;

    public PathFinderNode(int x, int y)
    {
        Xpos = x;
        Ypos = y;
        pos = new Vector2Int(x, y);
        neighbors = new List<PathFinderNode>();
    }

    public List<PathFinderNode> Neighbors
    {
        get => neighbors; set => neighbors = value;
    }
    public bool UsedToPathFinding { get => usedToPathFinding; set => usedToPathFinding = value; }
    public Vector2Int Pos { get => pos; set => pos = value; }
    public PathFinderNode Previous { get => previous; set => previous = value; }
    public bool Busy { get => busy; set => busy = value; }

    public void AddNeighbor(PathFinderNode neighbor)
    {
        neighbors.Add(neighbor);
    }
}

