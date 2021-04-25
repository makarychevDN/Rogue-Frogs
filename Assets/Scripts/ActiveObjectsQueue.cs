using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private QueueCell m_QueuePanelPrefab;
    [SerializeField] private float m_IndentMultiplier;
    [SerializeField] private Transform m_QueueVisualisationParent;
    private CycledLinkedList m_Queue;
    [SerializeField] private List<MonoBehaviour> m_ActiveNowObjects;
    private List<QueueCell> m_Cells;
    [SerializeField] private MapObject m_CurrentCharacter;
    private QueueNode m_CurrentQueueNode;
    private int m_QueueCount;
    private float m_SkipTurnDelay = 0.5f;
    [SerializeField] private SpawnManager m_SpawnManager;

    [SerializeField] private List<MapObject> m_QueueInspector;

    private float m_CheckActiveNowObjectsTime = 0.6f;
    private float m_CheckActiveNowObjectsTimer; 
    
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

        foreach (var item in temp)
        {
            if(item.GetComponent<PlayerInput>()!= null)
            {
                m_Queue.AddFirst(item.GetComponent<MapObject>());
                m_QueueInspector.Insert(0, item.GetComponent<MapObject>());
            }
            else
            {
                m_Queue.Add(item.GetComponent<MapObject>());
                m_QueueInspector.Add(item.GetComponent<MapObject>());
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
        //m_Cells[index].gameObject.SetActive(false);
        //m_Cells.RemoveAt(index);
        //RearrangeCells();

        if (mapObject == m_CurrentCharacter)
            SkipTheTurn();
    }

    public void SkipTheTurn()
    {
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();
        
        if(m_CurrentCharacter.SkipTurnAnimation != null)
            m_CurrentCharacter.SkipTurnAnimation.SetActive(false);
        
        //if(m_QueueCount < m_Cells.Count)
        //m_Cells[m_QueueCount].ActiveCell.SetActive(false);
        
        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = false;
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(false);
        }

        /*if (m_CurrentCharacter.GetComponent<DoNothingAI>() != null)
        {
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(false);
        }*/
        
        DisablePlayerInput();

        m_QueueCount++;

        /*if (m_QueueCount >= m_Queue.Count)
        {
            m_QueueCount = 0;
            if(m_SpawnManager != null)
                m_SpawnManager.IncrementCyclesCount();
        }*/

        //m_Cells[m_QueueCount].ActiveCell.SetActive(true);

        m_CurrentQueueNode = m_CurrentQueueNode.Next;
        m_CurrentCharacter = m_CurrentQueueNode.MapObject;


        if (m_CurrentCharacter.ShowUI != null)
        {
            m_CurrentCharacter.ShowUI.SetActiveUiObjects(true);
            m_CurrentCharacter.ShowUI.MapObjectIsActiveNow = true;
        }

        //StartNextAction();
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
            //SortActiveObjects();
            //InitPanel(mapObject);
            //RearrangeCells(); 
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

class InitiativeComparer : IComparer<MapObject>
{
    public int Compare(MapObject x, MapObject y)
    {
        var tempX = x.GetComponent<InitiativeContainer>().Initiative;
        var tempY = y.GetComponent<InitiativeContainer>().Initiative;
        
        if (tempX > tempY)
            return -1;
        else if(tempX < tempY)
            return 1;

        return 0;
    }
}
