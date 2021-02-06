using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsContainer : MonoBehaviour
{
    [SerializeField] private int m_MaxPoints;
    [SerializeField] private int m_CurrentPoints;
    [SerializeField] private int m_PointsRegenerationInTurn;

    public int MaxPoints { get => m_MaxPoints; set => m_MaxPoints = value; }
    public int CurrentPoints { get => m_CurrentPoints; set => m_CurrentPoints = value; }

    public void ResetPoints()
    {
        m_CurrentPoints += m_PointsRegenerationInTurn;
        m_CurrentPoints = Mathf.Clamp(m_CurrentPoints, 0, m_MaxPoints);
    }

    public void ResetAllPoints()
    {
        m_CurrentPoints = MaxPoints;
    }
}
