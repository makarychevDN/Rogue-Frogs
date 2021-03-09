using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attackable : MonoBehaviour
{
    [Header("Characteristics")]
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private int m_ActionCost;

    [Header("Sprite Visualisation")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite m_Sprite;

    [Header("Animation")] 
    [SerializeField] private AnimationsStateMashine m_AnimStateMashine;
    [SerializeField] private List<GameObject> m_AttackAnimationObjects;
    [SerializeField] private float m_AnimationTime;

    [Header("Setup")] 
    [SerializeField] private MapObjSpriteRotator m_SpriteRotator;
    [SerializeField] private MapObject m_MabObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private DamageUI m_DamageUI;
    [SerializeField] private RangeUI m_RangeUI;
    
    [Header("Player Setup")]
    [SerializeField] private PlayerInput m_PlayerInput;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnAttack;

    private Destructible m_CurrentDestructible;
    public int ActionCost { get => m_ActionCost; set => m_ActionCost = value; }

    public Map Map
    {
        get => m_Map;
        set => m_Map = value;
    }

    public ActiveObjectsQueue ObjectsQueue
    {
        get => m_ActiveObjectsQueue;
        set => m_ActiveObjectsQueue = value;
    }

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
        m_Map = FindObjectOfType<Map>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
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
            MapObject tempMapObject;
            m_SpriteRotator.RotateSprite(input);

            for (int i = 0; i < m_Range; i++)
            {
                tempMapObject = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if (tempMapObject != null)
                {
                    var tempDestructible = tempMapObject.GetComponent<Destructible>();
                    if (tempDestructible != null)
                    {
                        if (m_PlayerInput != null)
                        {
                            m_PlayerInput.InputIsPossible = false;
                        }
                        
                        m_CurrentDestructible = tempDestructible;
                        m_CurrentDestructible.StartHitAnimation();
                        m_ActionPointsContainer.CurrentPoints -= m_ActionCost;
                        m_AnimStateMashine.ActivateAttackAnim();
                        
                        
                        Invoke("DealDamage", m_AnimationTime);

                        foreach (var item in m_AttackAnimationObjects)
                        {
                            item.SetActive(true);
                            item.transform.LookAt2D(Vector2.right, tempMapObject.Pos);
                        }
                        
                        OnAttack?.Invoke();

                        break;
                    }
                }
            }
        }
    }

    private void DealDamage()
    {
        m_CurrentDestructible.CurrentHP -= m_Damage;
        m_CurrentDestructible.StoptHitAnimation();
        m_AnimStateMashine.ActivateStayAnim();

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
