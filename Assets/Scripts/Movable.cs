using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private bool m_Pushable;
    [SerializeField] private int m_StepCost;
    [SerializeField] private float m_DefaultAnimTime;
    [SerializeField] private AnimType m_DefaultAnimType;

    private ActionPointsContainer m_ActionPointsContainer;
    private MapObject m_MapObject;
    private Map m_Map;
    private CharactersStack m_CharacterStack;

    private Vector2Int m_NextPos;
    private Vector2Int m_CurrentPos;

    private AnimType m_CurrentAnimType;
    private bool m_MoveNow;
    private float m_CurrentTimer;
    private float m_Speed;
    private float m_DistanceDelta = 0.005f;


    private void Start()
    {
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_CurrentAnimType = m_DefaultAnimType;
        m_Map = FindObjectOfType<Map>();
        m_MapObject = GetComponent<MapObject>();
        m_CharacterStack = FindObjectOfType<CharactersStack>();
    }

    void Update()
    {
        if (m_MoveNow)
        {
            if (m_CurrentAnimType == AnimType.linear)
            {
                transform.position = Vector2.Lerp(m_CurrentPos, m_NextPos, m_Speed * m_CurrentTimer);
            }
            else
            {
                transform.position = Vector2.Lerp(m_CurrentPos, m_NextPos, CosLerpFunc(m_CurrentTimer, m_Speed));
            }

            if ((transform.position - new Vector3(m_NextPos.x, m_NextPos.y)).magnitude < m_DistanceDelta)
            {
                StopMovement();
            }

            m_CurrentTimer += Time.deltaTime;

        }
    }

    public void Move(Vector2Int input, AnimType animType, float animTime)
    {
        if (!m_MoveNow)
        {
            if(m_ActionPointsContainer.CurrentPoints >= m_StepCost)
            {
                if (m_Map.GetMapObjectByVector(m_MapObject.Pos + input) == null)
                {
                    m_ActionPointsContainer.CurrentPoints -= m_StepCost;
                    m_CurrentAnimType = animType;
                    m_CurrentPos = m_MapObject.Pos;
                    m_NextPos = m_CurrentPos + input;
                    m_MoveNow = true;
                    m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
                    m_CurrentTimer = 0;
                }
            }
        }
    }

    public void Move(Vector2Int input)
    {
        Move(input, m_DefaultAnimType, m_DefaultAnimTime);
    }

    public void StopMovement()
    {
        transform.position = new Vector3(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y), 0);
        m_MoveNow = false;

        m_Map.Cells[m_MapObject.Pos.x, m_MapObject.Pos.y] = null;
        m_MapObject.Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
        m_Map.Cells[m_MapObject.Pos.x, m_MapObject.Pos.y] = m_MapObject;
        m_CharacterStack.GiveTurnToNext(m_ActionPointsContainer.CurrentPoints);
    }

    public float CosLerpFunc(float timer, float speed)
    {
        return 0.5f + (-Mathf.Cos(timer * speed * Mathf.PI) * 0.5f);
    }
}

public enum AnimType
{
    linear, cos
}