using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeContainer : MonoBehaviour
{
    [SerializeField] private int m_Initiative;

    public int Initiative
    {
        get => m_Initiative;
        set => m_Initiative = value;
    }
}
