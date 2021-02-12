using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int m_MaxHP;
    [SerializeField] private int m_CurrentHP;
    [Header("Setup")]
    [SerializeField] ActiveObjectsQueue m_ActiveObjectsQueue;
    [SerializeField] HpUI m_HpUI;

    private void Reset()
    {
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_HpUI = GetComponentInChildren<HpUI>();
    }

    private void Start()
    {
        m_HpUI.SetValue(CurrentHP);
    }

    public int MaxHP { get => m_MaxHP; set => m_MaxHP = value; }
    public int CurrentHP { 

        get => m_CurrentHP;         
        set
        {
            m_CurrentHP = Mathf.Clamp(value, 0, MaxHP);
            m_HpUI.SetValue(CurrentHP);

            if (m_CurrentHP == 0)
            {
                var temp = GetComponent<MapObject>();
                m_ActiveObjectsQueue.RemoveCharacterFromStack(temp);
                FindObjectOfType<Map>().Cells[temp.Pos.x, temp.Pos.y] = null;
                gameObject.SetActive(false);
            }
        }
    }
}
