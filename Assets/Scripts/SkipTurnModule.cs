using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurnModule : MonoBehaviour
{
    [Header("References Setup")] 
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private GameObject m_SkipTurnAnimationGameObject;
    private float m_SkipTurnAnimationTime = 0.73f;

    public void SkipTurn()
    {
        m_MapObject.ActiveObjectsQueue.AddToActiveObjectsList(this);
        m_SkipTurnAnimationGameObject.SetActive(true);
        Invoke("SendMessageToQueue", m_SkipTurnAnimationTime);
    }

    private void SendMessageToQueue()
    {
        m_MapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
        m_SkipTurnAnimationGameObject.SetActive(false);
        m_MapObject.ActiveObjectsQueue.SkipTheTurn();
    }
}
