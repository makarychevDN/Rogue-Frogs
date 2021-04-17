using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [Header("References Setup")]
    [SerializeField] private MapObject m_MapObject;
    [Header("Setup")]
    [SerializeField] private int m_MaxHP;
    [SerializeField] private int m_CurrentHP;
    [SerializeField] private float m_AnimationTime;
    [SerializeField] private int m_ScoreCost;
    
    [SerializeField] private HpUI m_HpUI;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnApplyDamage;
    [SerializeField] private UnityEvent OnApplyHealing;
    [SerializeField] private UnityEvent OnDied;

    private void Start()
    {
        m_HpUI.SetValue(CurrentHP);
    }

    public int MaxHP { get => m_MaxHP; set => m_MaxHP = value; }
    public int CurrentHP { 

        get => m_CurrentHP;         
        set
        {
            if (value < m_CurrentHP)
            {
                OnApplyDamage?.Invoke();
            }

            if (value > m_CurrentHP)
            {
                OnApplyHealing?.Invoke();
            }
            
            m_CurrentHP = Mathf.Clamp(value, 0, MaxHP);
            m_HpUI.SetValue(CurrentHP);

            if (m_CurrentHP == 0)
            {
                m_MapObject.ActiveObjectsQueue.AddToActiveObjectsList(this);
                FindObjectOfType<Map>().SetMapObjectByVector(GetComponent<MapObject>().Pos, null);
                FindObjectOfType<Score>().AddScore(m_ScoreCost);
                OnDied?.Invoke();
                m_MapObject.AnimationStateMashine.ActivateDeathAnim();
                Invoke("RemoveObject", m_AnimationTime);
            }
        }
    }

    private void RemoveObject()
    {
        var temp = GetComponent<MapObject>();
        m_MapObject.ActiveObjectsQueue.RemoveCharacterFromStack(temp);
        Destroy(gameObject);
        m_MapObject.ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
    }

    public void StartHitAnimation()
    {
        m_MapObject.AnimationStateMashine.ActivateApplyDamageAnim();
    }
    
    public void StoptHitAnimation()
    {
        m_MapObject.AnimationStateMashine.ActivateStayAnim();
    }
}
