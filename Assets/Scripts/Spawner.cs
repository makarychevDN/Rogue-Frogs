using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private MapObject m_SpawnObjectPrefab;
    
    public void Spawn(Vector2Int spawnPos)
    {
        var temp = Instantiate(m_SpawnObjectPrefab);
        FindObjectOfType<Map>().SetMapObjectByVector(spawnPos,temp);
        temp.transform.position = new Vector3(spawnPos.x, spawnPos.y);
        temp.Pos = spawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }
    
    public void Spawn(Transform spawnPostransform)
    {
        Vector2Int spawnPos = new Vector2Int((int) spawnPostransform.position.x, (int) spawnPostransform.position.y);
        var temp = Instantiate(m_SpawnObjectPrefab);
        FindObjectOfType<Map>().SetMapObjectByVector(spawnPos,temp);
        temp.transform.position = new Vector3(spawnPos.x, spawnPos.y);
        temp.Pos = spawnPos;
        FindObjectOfType<ActiveObjectsQueue>().AddObjectInQueue(temp);
    }
}
