using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Range;
    [SerializeField] private int m_ActionCost;

    public int Damage { get => m_Damage; set => m_Damage = value; }
    public int Range { get => m_Range; set => m_Range = value; }
    public int ActionCost { get => m_ActionCost; set => m_ActionCost = value; }
}
