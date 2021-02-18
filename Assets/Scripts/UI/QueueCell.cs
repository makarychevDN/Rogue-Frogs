using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_QueueObjectSpriteRenderer;
    [SerializeField] private GameObject m_ActiveCell;

    public GameObject ActiveCell
    {
        get => m_ActiveCell;
        set => m_ActiveCell = value;
    }

    public void SetSprite(Sprite queueObjectSprite)
    {
        m_QueueObjectSpriteRenderer.sprite = queueObjectSprite;
    }
}
