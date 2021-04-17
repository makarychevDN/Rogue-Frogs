using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_UiObjects;
    [SerializeField] private bool m_AlwaysShow;
    private bool m_MapObjectIsActiveNow;

    public bool MapObjectIsActiveNow
    {
        get => m_MapObjectIsActiveNow;
        set => m_MapObjectIsActiveNow = value;
    }

    private void Start()
    {
        SetActiveUiObjects(false);
    }

    void OnMouseEnter()
    {
        SetActiveUiObjects(true);
    }
    
    void OnMouseExit()
    {
        SetActiveUiObjects(false);
    }

    public void SetActiveUiObjects(bool value)
    {
        if (!m_AlwaysShow)
        {
            if (!m_MapObjectIsActiveNow)
            {
                foreach (var temp in m_UiObjects)
                {
                    temp.SetActive(value);
                }
            }
        }
    }
}
