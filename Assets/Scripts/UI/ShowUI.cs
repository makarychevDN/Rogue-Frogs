using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_UiObjects;

    private void Start()
    {
        foreach (var temp in m_UiObjects)
        {
            temp.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        foreach (var temp in m_UiObjects)
        {
            temp.SetActive(true);
        }
    }
    
    void OnMouseExit()
    {
        foreach (var temp in m_UiObjects)
        {
            temp.SetActive(false);
        }
    }
}
