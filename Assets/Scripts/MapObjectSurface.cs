using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapObjectSurface : MapObject
{
    [Header("Events")] 
    [SerializeField] private UnityEvent OnStepIn;
    [SerializeField] private UnityEvent OnStepOut;

    public void ActivateOnStepInEvent()
    {
        OnStepIn?.Invoke();
    }
    
    public void ActivateOnStepOutEvent()
    {
        OnStepOut?.Invoke();
    }
}
