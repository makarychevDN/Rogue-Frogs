using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectsQueue : MonoBehaviour
{
    [SerializeField] private QueueCell m_QueuePanelPrefab;
    [SerializeField] private float m_IndentMultiplier;
    private List<MapObject> m_Characters;
    private List<QueueCell> m_Cells;
    private MapObject m_CurrentCharacter;
    private int m_QueueCount;
    private float m_SkipTurnDelay = 0.5f;

    private void Start()
    {
        FindAllActiveMapObjects();
        SortActiveObjects();
        InitPanels();

        m_CurrentCharacter.GetComponent<PlayerInput>().CanInput = true;
    }

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

        m_CurrentCharacter = m_Characters[0];
        m_Cells[0].ActiveCell.SetActive(true);
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

    public void RearrangeCells()
    {
        for (int i = 0; i < m_Cells.Count; i++)
        {
            m_Cells[i].transform.localPosition = Vector3.right * i * m_IndentMultiplier - Vector3.right * (m_Characters.Count-1) * 0.5f * m_IndentMultiplier;
        }
    }
    
    public void RemoveCharacterFromStack(MapObject mapObject)
    {
        int index = m_Characters.IndexOf(mapObject);
        m_Characters.RemoveAt(index);
        m_Cells[index].gameObject.SetActive(false);
        m_Cells.RemoveAt(index);
        RearrangeCells();
    }

    public void SkipTheTurn()
    {
        if(m_CurrentCharacter.SkipTurnAnimation!=null)
            m_CurrentCharacter.SkipTurnAnimation.SetActive(false);
        m_Cells[m_QueueCount].ActiveCell.SetActive(false);
        
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
        
        m_Cells[m_QueueCount].ActiveCell.SetActive(true);

        m_CurrentCharacter = m_Characters[m_QueueCount];
        m_CurrentCharacter.GetComponent<ActionPointsContainer>().ResetPoints();

        StartNextAction();
    }

    public void SkipTurnWithAnimation()
    {
        var temp = m_CurrentCharacter.GetComponent<PlayerInput>();

        if (temp != null)
        {
            temp.CanInput = false;
        }
        
        m_CurrentCharacter.SkipTurnAnimation.SetActive(true);
        Invoke("SkipTheTurn", m_SkipTurnDelay);
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
