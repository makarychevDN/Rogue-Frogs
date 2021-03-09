using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private List<Level> m_Levels;
    [SerializeField] private Level m_CurrentLevel;
    [SerializeField] private int m_LevelCount;
    [SerializeField] private MapObject m_Player;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartNextLevel();
        }
    }

    private void Start()
    {
        m_Player = FindObjectOfType<PlayerInput>().GetComponent<MapObject>();
        var cpawnedlevel = Instantiate(m_CurrentLevel);
        m_CurrentLevel = cpawnedlevel;
        ConnectPlayerToLevel();
    }

    public void StartNextLevel()
    {
        m_CurrentLevel.gameObject.SetActive(false);
        
        m_LevelCount++;
        if (m_LevelCount == m_Levels.Count)
            m_LevelCount = 0;
        
        var cpawnedlevel = Instantiate(m_Levels[m_LevelCount]);
        m_CurrentLevel = cpawnedlevel;
        ConnectPlayerToLevel();
    }

    public void ConnectPlayerToLevel()
    {
        m_Player.GetComponent<Destructible>().ObjectsQueue = m_CurrentLevel.Queue;
        
        m_Player.GetComponent<PlayerInput>().ActiveObjectQueue = m_CurrentLevel.Queue;

        m_Player.GetComponent<Movable>().ObjectsQueue = m_CurrentLevel.Queue;
        m_Player.GetComponent<Movable>().Map = m_CurrentLevel.Map;

        var attackables = m_Player.GetComponentsInChildren<AttackableInDirection>();
        foreach (var temp in attackables)
        {
            temp.ObjectsQueue = m_CurrentLevel.Queue;
            temp.Map = m_CurrentLevel.Map;
        }
    }
}
