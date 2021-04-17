using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] private List<ActionPointIcon> m_ActionPointIcons;

    public void ReFillIcons(int iconsCount)
    {
        for (int i = 0; i < m_ActionPointIcons.Count; i++)
        {
            m_ActionPointIcons[i].FullIcon.SetActive(false);
        }

        for (int i = 0; i < iconsCount; i++)
        {
            m_ActionPointIcons[i].FullIcon.SetActive(true);
        }
    }

}
