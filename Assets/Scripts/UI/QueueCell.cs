using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_QueueObjectSpriteRenderer;

    public void SetSprite(Sprite queueObjectSprite)
    {
        m_QueueObjectSpriteRenderer.sprite = queueObjectSprite;
    }
}
