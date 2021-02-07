using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int m_MaxHP;
    [SerializeField] private int m_CurrentHP;
    private ActiveObjectsQueue m_CharactersStack;

    private void Start()
    {
        m_CharactersStack = FindObjectOfType<ActiveObjectsQueue>();
    }

    public int MaxHP { get => m_MaxHP; set => m_MaxHP = value; }
    public int CurrentHP { 

        get => m_CurrentHP;         
        set
        {
            m_CurrentHP = Mathf.Clamp(value, 0, MaxHP);

            if (m_CurrentHP == 0)
            {
                var temp = GetComponent<MapObject>();
                m_CharactersStack.RemoveCharacterFromStack(temp);
                FindObjectOfType<Map>().Cells[temp.Pos.x, temp.Pos.y] = null;
                gameObject.SetActive(false);
            }
        }
    }
}
