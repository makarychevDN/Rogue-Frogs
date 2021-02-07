using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> m_ActionPointIcons;
    [SerializeField] private Sprite m_FullIcon;
    [SerializeField] private Sprite m_EmptyIcon;

    public void ReFillIcons(int iconsCount)
    {
        for (int i = 0; i < m_ActionPointIcons.Count; i++)
        {
            m_ActionPointIcons[i].sprite = m_EmptyIcon;
        }

        for (int i = 0; i < iconsCount; i++)
        {
            m_ActionPointIcons[i].sprite = m_FullIcon;
        }
    }

}
