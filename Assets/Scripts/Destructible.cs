﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int m_MaxHP;
    [SerializeField] private int m_CurrentHP;
    [SerializeField] private float m_AnimationTime;
    [SerializeField] private int m_ScoreCost;
    
    [Header("Setup")]
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;
    [SerializeField] private HpUI m_HpUI;
    [SerializeField] private AnimationsStateMashine m_AnimStateMashine; 

    [Header("Events")] 
    [SerializeField] private UnityEvent OnApplyDamage;
    [SerializeField] private UnityEvent OnApplyHealing;
    [SerializeField] private UnityEvent OnDied;

    private void Reset()
    {
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_HpUI = GetComponentInChildren<HpUI>();
    }

    private void Awake()
    {
        m_HpUI.SetValue(CurrentHP);
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
    }

    public int MaxHP { get => m_MaxHP; set => m_MaxHP = value; }
    public int CurrentHP { 

        get => m_CurrentHP;         
        set
        {
            if (value < m_CurrentHP)
            {
                OnApplyDamage?.Invoke();
            }

            if (value > m_CurrentHP)
            {
                OnApplyHealing?.Invoke();
            }
            
            m_CurrentHP = Mathf.Clamp(value, 0, MaxHP);
            m_HpUI.SetValue(CurrentHP);

            if (m_CurrentHP == 0)
            {
                m_ActiveObjectsQueue.AddToActiveObjectsList(this);
                FindObjectOfType<Map>().SetMapObjectByVector(GetComponent<MapObject>().Pos, null);
                FindObjectOfType<Score>().AddScore(m_ScoreCost);
                OnDied?.Invoke();
                AnimStateMashine.ActivateDeathAnim();
                Invoke("RemoveObject", m_AnimationTime);
            }
        }
    }

    private void RemoveObject()
    {
        var temp = GetComponent<MapObject>();
        m_ActiveObjectsQueue.RemoveCharacterFromStack(temp);
        Destroy(gameObject);
        m_ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
    }

    public void StartHitAnimation()
    {
        AnimStateMashine.ActivateApplyDamageAnim();
    }
    
    public void StoptHitAnimation()
    {
        AnimStateMashine.ActivateStayAnim();
    }
    
    public ActiveObjectsQueue ObjectsQueue
    {
        get => m_ActiveObjectsQueue;
        set => m_ActiveObjectsQueue = value;
    }

    public AnimationsStateMashine AnimStateMashine
    {
        get => m_AnimStateMashine;
        set => m_AnimStateMashine = value;
    }
}
