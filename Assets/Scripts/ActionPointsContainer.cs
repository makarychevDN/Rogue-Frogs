using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPointsContainer : MonoBehaviour
{
    [Header("Characteristics")]
    [SerializeField] private int maxPoints;
    [SerializeField] private int currentPoints;
    [SerializeField] private int pointsRegeneration;

    [Header("Setup")]
    [SerializeField] private ActionPointsUI UIVisualisation;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnSpentActionPoints;
    [SerializeField] private UnityEvent OnPointsRegeneration;
   
    public int CurrentPoints 
    { 
        get => currentPoints; 
        set 
        { 
            if(currentPoints >= value)
                OnSpentActionPoints?.Invoke();

            currentPoints = value;
            UIVisualisation.ReFillIcons(currentPoints);
        } 
    }
    
    private void Reset()
    {
        if(GetComponentInChildren<ActionPointsUI>() != null)
            UIVisualisation = GetComponentInChildren<ActionPointsUI>();
    }

    public void RestorePoints()
    {
        currentPoints += pointsRegeneration;
        currentPoints = Mathf.Clamp(currentPoints, 0, maxPoints);
        UIVisualisation.ReFillIcons(currentPoints);
        OnPointsRegeneration?.Invoke();
    }

    public void RestoreAllPoints()
    {
        currentPoints = maxPoints;
        UIVisualisation.ReFillIcons(currentPoints);
    }
}
