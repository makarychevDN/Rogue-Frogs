using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private QueueCell m_QueuePanelPrefab;
    [SerializeField] private float m_IndentMultiplier;
    private List<MapObject> m_Characters;
    private List<MonoBehaviour> m_ActiveNowObjects;
    private List<QueueCell> m_Cells;
    private MapObject m_CurrentCharacter;
    private int m_QueueCount;
    private float m_SkipTurnDelay = 0.5f;
    [SerializeField] private SpawnManager m_SpawnManager;
    private float m_CheckActiveNowObjectsTime = 0.6f;
    private float m_CheckActiveNowObjectsTimer; 
    private void Awake()
    {
        m_ActiveNowObjects = new List<MonoBehaviour>();
        FindAllActiveMapObjects();
        SortActiveObjects();
        m_CurrentCharacter = m_Characters[0];
        InitPanels();
        StartNextAction();
    }

    public void AddToActiveObjectsList(MonoBehaviour mapObject)
    {
        m_ActiveNowObjects.Add(mapObject);
    }

    public void RemoveFromActiveObjectsList(MonoBehaviour mapObject)
    {
        m_ActiveNowObjects.Remove(mapObject);
    }
    
    private void Update()
    {
        bool isSomeoneActiveNow = true;
        if (m_ActiveNowObjects.Count == 0)
        {
            m_CheckActiveNowObjectsTimer += Time.deltaTime;
            if (m_CheckActiveNowObjectsTimer > m_CheckActiveNowObjectsTime)
            {
                isSomeoneActiveNow = false;
            }
        }
        else
        {
            m_CheckActiveNowObjectsTimer = 0;
        }
        print(isSomeoneActiveNow);
    }

    #region QueueVisualisation
    public void FindAllActiveMapObjects()
    {
        var temp = FindObjectsOfType<ActionPointsContainer>();
        m_Characters = new List<MapObject>();
        
        foreach (var item in temp)
        {
            m_Characters.Add(item.GetComponent<MapObject>());
        }
    }

    public void SortActiveObjects()
    {
        InitiativeComparer ic = new InitiativeComparer();
        m_Characters.Sort(ic);
    }

    public void InitPanels()
    {
        m_Cells = new List<QueueCell>();
        for (int i = 0; i < m_Characters.Count; i++)
        {
            var spawnedPanel = Instantiate(m_QueuePanelPrefab, transform);
            spawnedPanel.transform.localPosition = Vector3.right * i * m_IndentMultiplier - Vector3.right * (m_Characters.Count-1) * 0.5f * m_IndentMultiplier;
            spawnedPanel.SetSprite(m_Characters[i].Sprite);
            m_Cells.Add(spawnedPanel);
        }
        
        m_Cells[0].ActiveCell.SetActive(true);
    }
    
    public void RearrangeCells()
    {
        for (int i = 0; i < m_Cells.Count; i++)
        {
            m_Cells[i].transform.localPosition = Vector3.right * i * m_IndentMultiplier - Vector3.right * (m_Characters.Count-1) * 0.5f * m_IndentMultiplier;
        }
    }
    
    public void InitPanel(MapObject mapObject)
    {
        var spawnedPanel = Instantiate(m_QueuePanelPrefab, transform);

        int count = m_Characters.IndexOf(mapObject);
        
        spawnedPanel.SetSprite(mapObject.Sprite);
        m_Cells.Insert(count, spawnedPanel);
    }
    
    #endregion

    public void StartNextAction()
    {
        m_CurrentCharacter.GetComponent<BaseInput>().DoSomething();
    }
    
    public void RemoveCharacterFromStack(MapObject mapObject)
    {
        if (m_Characters.Contains(mapObject))
        {
            int index = m_Characters.IndexOf(mapObject);
            m_Characters.RemoveAt(index);
            m_Cells[index].gameObject.SetActive(false);
            m_Cells.RemoveAt(index);
            RearrangeCells();
        
            if(mapObject == m_CurrentCharacter)
                SkipTheTurn();
        }
    }

    public void SkipTheTurn()
    {
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();
        
        if(m_CurrentCharacter.SkipTurnAnimation != null)
            m_CurrentCharacter.SkipTurnAnimation.SetActive(false);
        if(m_QueueCount < m_Cells.Count)
            m_Cells[m_QueueCount].ActiveCell.SetActive(false);
        
        DisablePlayerInput();

        m_QueueCount++;

        if (m_QueueCount >= m_Characters.Count)
        {
            m_QueueCount = 0;
            if(m_SpawnManager != null)
                m_SpawnManager.IncrementCyclesCount();
        }
        
        m_Cells[m_QueueCount].ActiveCell.SetActive(true);

        m_CurrentCharacter = m_Characters[m_QueueCount];

        StartNextAction();
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
            m_Characters.Add(mapObject);
            SortActiveObjects();
            InitPanel(mapObject);
            RearrangeCells(); 
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
