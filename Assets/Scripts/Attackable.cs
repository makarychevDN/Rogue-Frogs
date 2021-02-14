﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [Header("Characteristics")]
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private int m_ActionCost;

    [Header("Animation")]
    [SerializeField] private List<GameObject> m_AttackAnimationObjects;
    [SerializeField] private float m_AnimationTime;

    [Header("Setup")]
    [SerializeField] private MapObject m_MabObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private DamageUI m_DamageUI;
    [SerializeField] private RangeUI m_RangeUI;

    private Destructible m_CurrentDestructible;
    public int ActionCost { get => m_ActionCost; set => m_ActionCost = value; }

    private void Reset()
    {
        m_Map = FindObjectOfType<Map>();
        m_MabObject = GetComponent<MapObject>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_DamageUI = GetComponentInChildren<DamageUI>();
        m_RangeUI = GetComponentInChildren<RangeUI>();
    }

    private void Start()
    {
        m_DamageUI.SetValue(m_Damage);
        m_RangeUI.SetValue(m_Range);
    }

    public bool CheckAttackIsPossible(Vector2Int input)
    {
        if (m_ActionPointsContainer.CurrentPoints >= m_ActionCost)
        {
            MapObject temp;

            for(int i = 0; i < m_Range; i++)
            {
                temp = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if(temp != null)
                {
                    return temp.GetComponent<Destructible>() != null;
                }
            }
        }

        return false;
    }

    public bool CheckAttackTargetIsPossible(Vector2Int input, Destructible target)
    {
        if (m_ActionPointsContainer.CurrentPoints >= m_ActionCost)
        {
            MapObject temp;

            for (int i = 0; i < m_Range; i++)
            {
                temp = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if (temp != null)
                {
                    return temp == target;
                }
            }
        }
        return false;
    }

    public void Attack(Vector2Int input)
    {
        if (m_ActionPointsContainer.CurrentPoints >= m_ActionCost)
        {
            MapObject temp;

            for (int i = 0; i < m_Range; i++)
            {
                temp = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if (temp != null)
                {
                    if (temp.GetComponent<Destructible>() != null)
                    {
                        if (m_PlayerInput != null)
                        {
                            m_PlayerInput.CanInput = false;
                        }

                        m_CurrentDestructible = temp.GetComponent<Destructible>();
                        m_ActionPointsContainer.CurrentPoints -= m_ActionCost;

                        StartCoroutine(DealDamage(m_AnimationTime));

                        foreach (var item in m_AttackAnimationObjects)
                        {
                            item.SetActive(true);
                        }

                        break;
                    }
                }
            }
        }
    }

    private IEnumerator DealDamage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        m_CurrentDestructible.GetComponent<Destructible>().CurrentHP -= m_Damage;

        foreach (var item in m_AttackAnimationObjects)
        {
            item.SetActive(false);
        }

        if (m_ActionPointsContainer.CurrentPoints != 0)
        {
            m_ActiveObjectsQueue.StartNextAction();
        }
        else
        {
            m_ActiveObjectsQueue.SkipTheTurn();
        }

    }
}
