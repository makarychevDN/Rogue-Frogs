using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] private BaseWeapon m_Weapon;

    private MapObject m_MabObject;
    private Map m_Map;
    private ActiveObjectsQueue m_ActiveObjectsQueue;
    private ActionPointsContainer m_ActionPointsContainer;
    private PlayerInput m_PlayerInput;

    public BaseWeapon Weapon { get => m_Weapon; set => m_Weapon = value; }

    private void Start()
    {
        m_Map = FindObjectOfType<Map>();
        m_MabObject = GetComponent<MapObject>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    public bool CheckAttackIsPossible(Vector2Int input)
    {
        if (m_ActionPointsContainer.CurrentPoints >= m_Weapon.ActionCost)
        {
            MapObject temp;

            for(int i = 0; i < m_Weapon.Range; i++)
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
        if (m_ActionPointsContainer.CurrentPoints >= m_Weapon.ActionCost)
        {
            MapObject temp;

            for (int i = 0; i < m_Weapon.Range; i++)
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
        if (m_ActionPointsContainer.CurrentPoints >= m_Weapon.ActionCost)
        {
            MapObject temp;

            for (int i = 0; i < m_Weapon.Range; i++)
            {
                temp = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if (temp != null)
                {
                    if (temp.GetComponent<Destructible>() != null)
                    {
                        if(m_PlayerInput != null)
                        {
                            m_PlayerInput.CanInput = false;
                        }

                        m_ActionPointsContainer.CurrentPoints -= m_Weapon.ActionCost;
                        temp.GetComponent<Destructible>().CurrentHP -= m_Weapon.Damage;

                        if (m_ActionPointsContainer.CurrentPoints != 0)
                        {
                            m_ActiveObjectsQueue.StartNextAction();
                        }
                        else
                        {
                            m_ActiveObjectsQueue.SkipTheTurn();
                        }
                        break;
                    }
                }
            }
        }
    }
}
