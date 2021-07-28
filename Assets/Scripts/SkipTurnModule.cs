using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurnModule : ActivatableBehavior
{
    [Header("References Setup")] 
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private GameObject m_SkipTurnAnimationGameObject;
    static float m_SkipTurnAnimationTime = 0.73f;
    static float m_SkipTurnDelay = 0.73f;

    public static float SkipTurnDelay { get => m_SkipTurnDelay; set => m_SkipTurnDelay = value; }

    public void SkipTurn()
    {
        ActiveNow = true;
        m_SkipTurnAnimationGameObject.SetActive(true);
        Invoke("StopClockAnimation", m_SkipTurnAnimationTime);
        Invoke("SendMessageToQueue", m_SkipTurnDelay);
    }

    private void SendMessageToQueue()
    {
        ActiveNow = false;
        m_MapObject.ActiveObjectsQueue.SkipTheTurn();
    }
    private void StopClockAnimation()
    {
        m_SkipTurnAnimationGameObject.SetActive(false);
    }
}
