using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected MapObject m_SpawnObjectPrefab;
    [SerializeField] protected MapObject m_ThisMapObject;
    protected Vector2Int m_SpawnPos;
    
    public void Spawn(Vector2Int spawnPos)
    {
        var temp = Instantiate(m_SpawnObjectPrefab);
        FindObjectOfType<Map>().SetMapObjectByVector(spawnPos,temp);
        temp.transform.position = new Vector3(spawnPos.x, spawnPos.y);
        temp.Pos = spawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }
    
    public virtual void Spawn(Transform spawnPostransform)
    {
        m_SpawnPos = new Vector2Int((int) spawnPostransform.position.x, (int) spawnPostransform.position.y);
        var temp = Instantiate(m_SpawnObjectPrefab);
        FindObjectOfType<Map>().SetMapObjectByVector(m_SpawnPos,temp);
        temp.transform.position = new Vector3(m_SpawnPos.x, m_SpawnPos.y);
        temp.Pos = m_SpawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }
    
}
