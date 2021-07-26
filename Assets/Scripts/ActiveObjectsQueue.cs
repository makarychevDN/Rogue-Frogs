using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private SpawnManager m_SpawnManager;
    [SerializeField] private MapObject m_CurrentCharacter;

    private CycledLinkedList m_Queue;
    private QueueNode m_CurrentQueueNode;
    private bool m_ShowAllUiNow;

    
    private void Awake()
    {
        FindAllActiveMapObjects();
        m_CurrentQueueNode = m_Queue.HeadNode;
        m_CurrentCharacter = m_Queue.HeadNode.MapObject;
    }

    public void FindAllActiveMapObjects()
    {
        var temp = FindObjectsOfType<ActionPointsContainer>();
        m_Queue = new CycledLinkedList();

        m_Queue.AddFirst(FindObjectOfType<PlayerInput>().GetComponent<MapObject>());

        foreach (var item in temp)
        {
            if(item.GetComponent<PlayerInput>() == null)
            {
                if(item.GetComponent<AttackableSurfaceAI>() != null)
                {
                    m_Queue.AddAfterTargetObject( m_Queue.HeadNode.MapObject, item.GetComponent<MapObject>());
                }
                else
                {
                    m_Queue.Add(item.GetComponent<MapObject>());
                }
 
            }

        }
    }
    
    private void Update()
    {
        if (!CurrentlyActiveObjects.SomethingIsActNow)
        {
            StartNextAction();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_ShowAllUiNow = !m_ShowAllUiNow;
        }

        if (m_ShowAllUiNow)
        {
            foreach (var item in m_Queue)
            {
                if (item.MapObject.ShowUI != null)
                {
                    item.MapObject.ShowUI.SetActiveUiObjects(true);
                }
            }
        }
        else
        {
            foreach (var item in m_Queue)
            {
                if (item.MapObject.ShowUI != null)
                {
                    item.MapObject.ShowUI.SetActiveUiObjects(false);
                }
            }
        }
    }

    public void StartNextAction()
    {
        m_CurrentCharacter.GetComponent<BaseInput>().Act();
    }
    
    public void RemoveCharacterFromStack(MapObject mapObject)
    {
        m_Queue.Remove(mapObject);

        if (mapObject == m_CurrentCharacter)
            SkipTheTurn();
    }

    public void SkipTheTurn()
    {
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().RestorePoints();
        
        
        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = false;
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(false);
        }
        
        DisablePlayerInput();


        m_CurrentQueueNode = m_CurrentQueueNode.Next;
        m_CurrentCharacter = m_CurrentQueueNode.MapObject;

        if (m_CurrentQueueNode.MapObject.GetComponent<PlayerInput>() != null)
        {
            m_SpawnManager.IncrementCyclesCount();
        }

        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(true);
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = true;
        }
    }

    public void AddObjectInQueue(MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.Add(mapObject);
        }
    }
    public void AddObjectInQueueAfterPlayer(MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.AddSecond(mapObject);
        }
    }

    public void AddObjectInQueueAfterTarget(MapObject target, MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.AddAfterTargetObject(target, mapObject);
        }
    }

    public void AddObjectInQueueBeforeTarget(MapObject target, MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.AddBeforeTargetObject(target, mapObject);
        }
    }

    public void DisablePlayerInput()
    {
        var temp = m_CurrentCharacter.GetComponent<PlayerInput>();
        if (temp != null)
        {
            temp.InputIsPossible = false;
        }
    }
}
