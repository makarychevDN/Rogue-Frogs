using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPointsContainer : MonoBehaviour
{
    [SerializeField] private int m_MaxPoints;
    [SerializeField] private int m_CurrentPoints;
    [SerializeField] private int m_PointsRegenerationInTurn;

    [Header("Setup")]
    [SerializeField] private ActionPointsUI m_PointsVisualisation;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnDidSomething;
    [SerializeField] private UnityEvent OnActionPointsRegeneration;
    
    public int MaxPoints { get => m_MaxPoints; set => m_MaxPoints = value; }
    public int CurrentPoints 
    { 
        get => m_CurrentPoints; 
        set 
        { 
            m_CurrentPoints = value;
            m_PointsVisualisation.ReFillIcons(m_CurrentPoints);
            OnDidSomething?.Invoke();
        } 
    }
    
    private void Reset()
    {
        m_PointsVisualisation = GetComponentInChildren<ActionPointsUI>();
    }

    public void ResetPoints()
    {
        m_CurrentPoints += m_PointsRegenerationInTurn;
        m_CurrentPoints = Mathf.Clamp(m_CurrentPoints, 0, m_MaxPoints);
        m_PointsVisualisation.ReFillIcons(m_CurrentPoints);
        OnActionPointsRegeneration?.Invoke();
    }

    public void ResetAllPoints()
    {
        m_CurrentPoints = MaxPoints;
        m_PointsVisualisation.ReFillIcons(m_CurrentPoints);
    }
}
