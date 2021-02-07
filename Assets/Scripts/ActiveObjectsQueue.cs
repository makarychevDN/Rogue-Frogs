using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
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
            }

            else
            {
                m_Characters.Add(item.GetComponent<MapObject>());
            }
        }
    }

    public void StartNextAction()
    {
        if (m_CurrentCharacter.GetComponent<PlayerInput>() != null)
        {
            m_CurrentCharacter.GetComponent<PlayerInput>().CanInput = true;
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
        if (m_CurrentCharacter.GetComponent<PlayerInput>() != null)
        {
            m_CurrentCharacter.GetComponent<PlayerInput>().CanInput = false;
        }

        m_QueueCount++;

        if (m_QueueCount >= m_Characters.Count)
        {
            m_QueueCount = 0;
        }

        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();
        m_CurrentCharacter = m_Characters[m_QueueCount];

        StartNextAction();
    }
}
