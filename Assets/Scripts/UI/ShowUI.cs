using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_UiObjects;
    [SerializeField] private bool m_AlwaysShow;
    private bool m_MapObjectIsActiveNow;
    [SerializeField] private bool m_MouseOnObjectNow;

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
        m_MouseOnObjectNow = true;
    }
    
    void OnMouseExit()
    {
        SetActiveUiObjects(false);
        m_MouseOnObjectNow = false;
    }

    public void SetActiveUiObjects(bool value)
    {
        if (!m_AlwaysShow && !m_MapObjectIsActiveNow && !m_MouseOnObjectNow)
        {
            foreach (var temp in m_UiObjects)
            {
                temp.SetActive(value);
            }
        }
    }
}
