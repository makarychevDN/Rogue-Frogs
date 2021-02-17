using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private GameObject m_QueuePanelPrefab;
    [SerializeField] private float m_IndentMultiplier;
    [SerializeField] private List<MapObject> m_Characters;
    [SerializeField] private MapObject m_CurrentCharacter;
    [SerializeField] private int m_QueueCount;

    private void Start()
    {
        var temp = FindObjectsOfType<ActionPointsContainer>();

        foreach (var item in temp)
        {
            if (item.GetComponent<PlayerInput>() != null)
            {
                m_Characters.Insert(0, item.GetComponent<MapObject>());
                item.GetComponent<PlayerInput>().CanInput = true;
            }

            else
            {
                m_Characters.Add(item.GetComponent<MapObject>());
            }
        }

        for (int i = 0; i < m_Characters.Count; i++)
        {
            var spawnedPanel = Instantiate(m_QueuePanelPrefab, transform);
            spawnedPanel.transform.localPosition += Vector3.right * i * m_IndentMultiplier;
        }
    }

    public void StartNextAction()
    {
        var temp = m_CurrentCharacter.GetComponent<PlayerInput>();
        if (temp != null)
        {
            temp.CanInput = true;
        }
        else
        {
            m_CurrentCharacter.GetComponent<BaseAI>().DoSomething();
        }
    }

    public void RemoveCharacterFromStack(MapObject mapObject)
    {
        m_Characters.Remove(mapObject);
    }

    public void SkipTheTurn()
    {
        var temp = m_CurrentCharacter.GetComponent<PlayerInput>();
        if (temp != null)
        {
            temp.CanInput = false;
        }

        m_QueueCount++;

        if (m_QueueCount >= m_Characters.Count)
        {
            m_QueueCount = 0;
        }

        m_CurrentCharacter = m_Characters[m_QueueCount];
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();

        StartNextAction();
    }
}
