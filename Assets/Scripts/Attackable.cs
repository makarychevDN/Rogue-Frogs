using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] private BaseWeapon m_Weapon;
    [SerializeField] private MapObject m_MabObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private CharactersStack m_CharactersStack;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;

    public BaseWeapon Weapon { get => m_Weapon; set => m_Weapon = value; }

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
            m_ActionPointsContainer.CurrentPoints -= m_Weapon.ActionCost;

            for (int i = 0; i < m_Weapon.Range; i++)
            {
                temp = m_Map.GetMapObjectByVector(m_MabObject.Pos + input * (i + 1));

                if (temp != null)
                {
                    if (temp.GetComponent<Destructible>() != null)
                    {
                        temp.GetComponent<Destructible>().CurrentHP -= m_Weapon.Damage;
                        m_CharactersStack.GiveTurnToNext(m_ActionPointsContainer.CurrentPoints);
                    }
                }
            }
        }
    }
}
