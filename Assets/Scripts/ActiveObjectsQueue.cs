using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private SpawnManager m_SpawnManager;
    [SerializeField] private MapObject m_CurrentCharacter;
    [SerializeField] private List<MapObject> m_QueueInspector;

    private CycledLinkedList m_Queue;
    private List<MonoBehaviour> m_ActiveNowObjects;
    private QueueNode m_CurrentQueueNode;
    private float m_SkipTurnDelay = 0.5f;

    
    private void Awake()
    {
        m_QueueInspector = new List<MapObject>();
        m_ActiveNowObjects = new List<MonoBehaviour>();
        FindAllActiveMapObjects();
        m_CurrentQueueNode = m_Queue.HeadNode;
        m_CurrentCharacter = m_Queue.HeadNode.MapObject;
    }

    public void FindAllActiveMapObjects()
    {
        var temp = FindObjectsOfType<ActionPointsContainer>();
        m_Queue = new CycledLinkedList();

        m_Queue.AddFirst(FindObjectOfType<PlayerInput>().GetComponent<MapObject>());
        m_QueueInspector.Add(FindObjectOfType<PlayerInput>().GetComponent<MapObject>());

        foreach (var item in temp)
        {
            if(item.GetComponent<PlayerInput>() == null)
            {
                if(item.GetComponent<AttackableSurfaceAI>() != null)
                {
                    m_Queue.AddSecond(item.GetComponent<MapObject>());
                    m_QueueInspector.Insert(1, item.GetComponent<MapObject>());
                }
                else
                {
                    m_Queue.Add(item.GetComponent<MapObject>());
                    m_QueueInspector.Add(item.GetComponent<MapObject>());
                }
 
            }

        }
    }

    #region ActiveNowObjects

    public void AddToActiveObjectsList(MonoBehaviour something)
    {
        m_ActiveNowObjects.Add(something);
    }

    public void RemoveFromActiveObjectsList(MonoBehaviour something)
    {
        m_ActiveNowObjects.Remove(something);
    }
    #endregion
    
    private void Update()
    {
        if (m_ActiveNowObjects.Count == 0)
        {
            StartNextAction();
        }

        if(Input.GetKey(KeyCode.Mouse0))
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

    #region QueueVisualisation

    /*
    public void SortActiveObjects()
    {
        InitiativeComparer ic = new InitiativeComparer();
    }

    public void InitPanels()
    {
        m_Cells = new List<QueueCell>();
        for (int i = 0; i < m_Queue.Count; i++)
        {
            var spawnedPanel = Instantiate(m_QueuePanelPrefab, transform);
            spawnedPanel.transform.localPosition = Vector3.right * i * m_IndentMultiplier - Vector3.right * (m_Queue.Count-1) * 0.5f * m_IndentMultiplier;
            spawnedPanel.SetSprite(m_Queue[i].Sprite);
            m_Cells.Add(spawnedPanel);
            spawnedPanel.transform.parent = m_QueueVisualisationParent;
        }
        
        m_Cells[0].ActiveCell.SetActive(true);
    }
    
    public void RearrangeCells()
    {
        for (int i = 0; i < m_Cells.Count; i++)
        {
            m_Cells[i].transform.localPosition = Vector3.right * i * m_IndentMultiplier - Vector3.right * (m_Queue.Count-1) * 0.5f * m_IndentMultiplier;
        }
    }
    
    public void InitPanel(MapObject mapObject)
    {
        var spawnedPanel = Instantiate(m_QueuePanelPrefab, transform);

        int count = m_Queue.IndexOf(mapObject);
        
        spawnedPanel.SetSprite(mapObject.Sprite);
        spawnedPanel.transform.parent = m_QueueVisualisationParent;
        m_Cells.Insert(count, spawnedPanel);
    }*/
    
    #endregion

    public void StartNextAction()
    {
        m_CurrentCharacter.GetComponent<BaseInput>().DoSomething();
    }
    
    public void RemoveCharacterFromStack(MapObject mapObject)
    {
        m_Queue.Remove(mapObject);
        m_QueueInspector.Remove(mapObject);

        if (mapObject == m_CurrentCharacter)
            SkipTheTurn();
    }

    public void SkipTheTurn()
    {
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();
        
        if(m_CurrentCharacter.SkipTurnAnimation != null)
            m_CurrentCharacter.SkipTurnAnimation.SetActive(false);
        
        
        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = false;
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(false);
        }
        
        DisablePlayerInput();


        m_CurrentQueueNode = m_CurrentQueueNode.Next;
        m_CurrentCharacter = m_CurrentQueueNode.MapObject;


        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(true);
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = true;
        }
    }

    public void SkipTurnWithAnimation()
    {
        DisablePlayerInput();
        m_CurrentCharacter.SkipTurnAnimation.SetActive(true);
        Invoke("SkipTheTurn", m_SkipTurnDelay);
    }

    public void AddObjectInQueue(MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.Add(mapObject);
            m_QueueInspector.Add(mapObject);
        }
    }
    public void AddObjectInQueueAfterPlayer(MapObject mapObject)
    {
        if (mapObject.GetComponent<ActionPointsContainer>())
        {
            m_Queue.AddSecond(mapObject);
            m_QueueInspector.Insert(1 ,mapObject);
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
