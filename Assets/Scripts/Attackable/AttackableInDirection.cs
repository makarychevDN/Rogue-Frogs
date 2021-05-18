using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackableInDirection : MonoBehaviour
{
    [Header("Characteristics")]
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private int m_ActionCost;
    

    [Header("Setup")]
    [SerializeField] private MapObject m_MabObject;
    [SerializeField] private DamageUI m_DamageUI;
    [SerializeField] private float m_AnimationTime;
    
    [Header("Player Setup")]
    [SerializeField] private PlayerInput m_PlayerInput;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnAttack;

    private Destructible m_CurrentDestructible;
    public int ActionCost { get => m_ActionCost; set => m_ActionCost = value; }
    
    private void Awake()
    {
        m_DamageUI.SetValue(m_Damage);
    }

    public bool CheckAttackIsPossible(Vector2Int input)
    {
        if (m_MabObject.ActionPointsContainerModule.CurrentPoints >= m_ActionCost)
        {
            MapObject temp;

            for(int i = 0; i < m_Range; i++)
            {
                temp = m_MabObject.Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));
                if(temp != null)
                {
                    return temp.GetComponent<Destructible>() != null;
                }
            }
        }

        return false;
    }

    public bool CheckAttackTargetIsPossible(Vector2Int input, MapObject target)
    {
        if (m_MabObject.ActionPointsContainerModule.CurrentPoints  >= m_ActionCost)
        {
            MapObject temp;

            for (int i = 0; i < m_Range; i++)
            {
                temp = m_MabObject.Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));
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
        m_MabObject.ActiveObjectsQueue.AddToActiveObjectsList(this);
        if (m_MabObject.ActionPointsContainerModule.CurrentPoints >= m_ActionCost)
        {
            MapObject tempMapObject;
            m_MabObject.SpriteRotator.RotateSprite(input);

            for (int i = 0; i < m_Range; i++)
            {
                tempMapObject = m_MabObject.Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

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
                        m_MabObject.ActionPointsContainerModule.CurrentPoints -= m_ActionCost;
                        m_MabObject.AnimationStateMashine.ActivateAttackAnim();
                        
                        
                        Invoke("DealDamage", m_AnimationTime);
                        
                        
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
        m_MabObject.AnimationStateMashine.ActivateStayAnim();
        
        
        m_MabObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
        
        if (m_MabObject.ActionPointsContainerModule.CurrentPoints == 0)
        {
            m_MabObject.ActiveObjectsQueue.SkipTheTurn();
        }
    }
}
