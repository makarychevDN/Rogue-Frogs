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

    [Header("Events")] 
    [SerializeField] private UnityEvent OnSpentActionPoints;
    [SerializeField] private UnityEvent OnPointsRegeneration;
    [SerializeField] private UnityEvent OnPointsValueChanged;

    public int CurrentPoints 
    { 
        get => currentPoints; 
        set 
        { 
            if(currentPoints >= value)
                OnSpentActionPoints?.Invoke();

            currentPoints = value;
            OnPointsValueChanged.Invoke();
        } 
    }
    
    private void Start()
    {
        if (GetComponentInChildren<ActionPointsUI>() != null)
            OnPointsValueChanged.AddListener(GetComponentInChildren<ActionPointsUI>().ReFillIcons);
    }

    public void RestorePoints()
    {
        currentPoints += pointsRegeneration;
        currentPoints = Mathf.Clamp(currentPoints, 0, maxPoints);
        OnPointsValueChanged.Invoke();
        OnPointsRegeneration?.Invoke();
    }

    public void RestoreAllPoints()
    {
        currentPoints = maxPoints;
        OnPointsValueChanged.Invoke();
    }
}
