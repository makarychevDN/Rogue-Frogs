﻿using System;
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

    private void Awake()
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
        m_Player.GetComponent<MapObject>().ActiveObjectsQueue = m_CurrentLevel.Queue;
        m_Player.GetComponent<MapObject>().ActiveObjectsQueue = m_CurrentLevel.Queue;
        m_Player.GetComponent<MapObject>().Map = m_CurrentLevel.Map;

        var attackables = m_Player.GetComponentsInChildren<AttackableInDirection>();
    }
}
