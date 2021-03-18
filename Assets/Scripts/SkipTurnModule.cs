using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurnModule : MonoBehaviour
{
    [SerializeField] private ActiveObjectsQueue m_Queue;
    [SerializeField] private GameObject m_SkipTurnAnimationGameObject;
    private float m_SkipTurnAnimationTime = 0.73f;
    void Awake()
    {
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
    }

    void Start()
    {
        m_Queue = FindObjectOfType<ActiveObjectsQueue>();
    }

    public void SkipTurn()
    {
        m_Queue.AddToActiveObjectsList(this);
        m_SkipTurnAnimationGameObject.SetActive(true);
        Invoke("SendMessageToQueue", m_SkipTurnAnimationTime);
    }

    private void SendMessageToQueue()
    {
        m_Queue.RemoveFromActiveObjectsList(this);
        m_SkipTurnAnimationGameObject.SetActive(false);
        m_Queue.SkipTheTurn();
    }
}
