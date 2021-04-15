using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_UiObjects;
    [SerializeField] private bool m_AlwaysShow;

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
            foreach (var temp in m_UiObjects)
            {
                temp.SetActive(value);
            }
        }
    }
}
