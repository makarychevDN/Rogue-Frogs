using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{    
    [SerializeField] private MapObject m_TopRightWall;
    private MapObject[,,] m_Cells;
    private PathFinder pathfinder;
    private int sizeX, sizeY;

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

        pathfinder = new PathFinder(this);
    }

    #region PropertiesAndGetSetters
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
        pathfinder.NodesGrid[coordinates.x, coordinates.y].Busy = mapObject != null;
        m_Cells[coordinates.x, coordinates.y, 0] = mapObject;
    }
    public MapObject[,,] Cells { get => m_Cells; set => m_Cells = value; }
    public int SizeY { get => sizeY; set => sizeY = value; }
    public int SizeX { get => sizeX; set => sizeX = value; }
    public PathFinder Pathfinder { get => pathfinder; set => pathfinder = value; }

    #endregion
}


public class PathFinder : MonoBehaviour
{
    private Map map;
    private PathFinderNode[,] nodesGrid;
    List<PathFinderNode> parentNodes;
    List<PathFinderNode> childNodes;
    private List<Vector2Int> dirVectors;

    public PathFinderNode[,] NodesGrid { get => nodesGrid; set => nodesGrid = value; }

    public PathFinder(Map map)
    {
        this.map = map;
        InitializeDirVectors();
        InitialaizeNodesGrid(map.SizeX, map.SizeY);
        FindAllNodesNeighbors(map.SizeX, map.SizeY);
    }
    public void ResetNodes()
    {
        foreach (var item in nodesGrid)
        {
            item.UsedToPathFinding = false;
        }
    }
    public List<Vector2Int> FindWay(MapObject user, MapObject target, bool ignoreTraps)
    {
        ResetNodes();
        return FindWayByWaveAlgorithm(user, target, ignoreTraps);
    }
    private List<Vector2Int> FindWayByWaveAlgorithm(MapObject user, MapObject target, bool ignoreTraps)
    {
        childNodes = new List<PathFinderNode>();
        childNodes.Add(nodesGrid[user.Pos.x, user.Pos.y]);
        nodesGrid[user.Pos.x, user.Pos.y].UsedToPathFinding = true;

        while (childNodes.Count != 0)
        {
            parentNodes = childNodes;
            childNodes = new List<PathFinderNode>();

            foreach (var tempParent in parentNodes)
            {
                foreach (var tempChild in tempParent.Neighbors)
                {
                    if (tempChild.Pos == new Vector2Int(target.Pos.x, target.Pos.y))
                    {
                        tempChild.Previous = tempParent;
                        List<Vector2Int> path = new List<Vector2Int>();
                        var tempBackTrackNode = tempChild;

                        while (tempBackTrackNode.Pos != new Vector2Int(user.Pos.x, user.Pos.y))
                        {
                            path.Insert(0, tempBackTrackNode.Pos);
                            tempBackTrackNode = tempBackTrackNode.Previous;
                        }

                        return path;
                    }
                    else if (!tempChild.UsedToPathFinding && !tempChild.Busy && (map.GetSurfaceByVector(tempChild.Pos) == null || ignoreTraps))
                    {
                        tempChild.Previous = tempParent;
                        childNodes.Add(tempChild);
                        tempChild.UsedToPathFinding = true;
                    }
                }
            }
        }
        return null;
    }

    #region InitPathfinder
    private void InitializeDirVectors()
    {
        dirVectors = new List<Vector2Int>();
        dirVectors.Add(Vector2Int.up);
        dirVectors.Add(Vector2Int.right);
        dirVectors.Add(Vector2Int.down);
        dirVectors.Add(Vector2Int.left);
    }

    private void InitialaizeNodesGrid(int sizeX, int sizeY)
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

    private void FindAllNodesNeighbors(int sizeX, int sizeY)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (nodesGrid[i, j] != null)
                {
                    foreach (var temp in dirVectors)
                    {
                        if (i + temp.x >= 0 && i + temp.x < sizeX && j + temp.y >= 0 && j + temp.y < sizeY)
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

    #endregion

}

public class PathFinderNode
{
    private Vector2Int pos;
    private List<PathFinderNode> neighbors;
    private bool usedToPathFinding;
    private bool busy;

    private PathFinderNode previous;

    public PathFinderNode(int x, int y)
    {
        pos = new Vector2Int(x, y);
        neighbors = new List<PathFinderNode>();
    }

    #region properties
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
    #endregion
}

