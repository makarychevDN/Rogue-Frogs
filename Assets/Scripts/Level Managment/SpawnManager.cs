using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int m_CyclesBeforeSpawn;
    [SerializeField] private int m_Count;
    [SerializeField] protected List<MapObject> m_SpawnObjectPrefab;
    [SerializeField] private Map m_Map;
    protected Vector2Int m_SpawnPos;
    
    public void Spawn(Vector2Int spawnPos)
    {
        var temp = Instantiate(m_SpawnObjectPrefab[Random.Range(0,m_SpawnObjectPrefab.Count-1)]);
        FindObjectOfType<Map>().SetMapObjectByVector(spawnPos,temp);
        temp.transform.position = new Vector3(spawnPos.x, spawnPos.y);
        temp.Pos = spawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }
    
    public virtual void Spawn(Transform spawnPostransform)
    {
        m_SpawnPos = new Vector2Int((int) spawnPostransform.position.x, (int) spawnPostransform.position.y);
        var temp = Instantiate(m_SpawnObjectPrefab[Random.Range(0,m_SpawnObjectPrefab.Count-1)]);
        FindObjectOfType<Map>().SetMapObjectByVector(m_SpawnPos,temp);
        temp.transform.position = new Vector3(m_SpawnPos.x, m_SpawnPos.y);
        temp.Pos = m_SpawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }

    public void IncrementCyclesCount()
    {
        m_Count++;

        if (m_Count == m_CyclesBeforeSpawn)
        {
            print("time to spawn");
            m_Count = 0;
            List<Vector2Int> freeCellPositions = new List<Vector2Int>();
            
            for (int i = 0; i < m_Map.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < m_Map.Cells.GetLength(1); j++)
                {
                    if (m_Map.GetMapObjectByVector(new Vector2Int(i, j)) == null)
                    {
                        freeCellPositions.Add(new Vector2Int(i, j));
                    }
                }
            }

            if (freeCellPositions.Count != 0)
            {
                Spawn(freeCellPositions[Random.Range(0,freeCellPositions.Count-1)]);
            }
        }
    }
}
