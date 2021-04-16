using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointIcon : MonoBehaviour
{
    [SerializeField] private GameObject m_FullIcon;

    public GameObject FullIcon
    {
        get => m_FullIcon;
    }
}
