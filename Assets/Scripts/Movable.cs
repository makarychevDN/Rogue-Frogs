using System;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private bool m_Pushable;
    [SerializeField] private int m_DefaultStepCost;
    [SerializeField] private float m_DefaultAnimTime;
    [SerializeField] private AnimType m_DefaultAnimType;

    [Header("Setup")]
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;

    [Header("Player Setup")]
    [SerializeField] private PlayerInput m_PlayerInput;

    private Vector2Int m_NextPos;
    private Vector2Int m_CurrentPos;

    private AnimType m_CurrentAnimType;
    private bool m_MoveNow;
    private float m_AnimationTimer;
    private float m_Speed;
    private float m_DistanceDelta = 0.005f;


    private void Reset()
    {
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_Map = FindObjectOfType<Map>();
        m_MapObject = GetComponent<MapObject>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        m_CurrentAnimType = m_DefaultAnimType;
    }

    void Update()
    {
        if (m_MoveNow)
        {
            if (m_CurrentAnimType == AnimType.linear)
            {
                transform.position = Vector2.Lerp(m_CurrentPos, m_NextPos, m_Speed * m_AnimationTimer);
            }
            else
            {
                transform.position = Vector2.Lerp(m_CurrentPos, m_NextPos, CosLerpFunc(m_AnimationTimer, m_Speed));
            }

            if ((transform.position - new Vector3(m_NextPos.x, m_NextPos.y)).magnitude < m_DistanceDelta)
            {
                StopMovement();
            }

            m_AnimationTimer += Time.deltaTime;

        }
    }

    public void Move(Vector2Int input, AnimType animType, float animTime, int stepCost)
    {
        if (!m_MoveNow)
        {
            if(m_ActionPointsContainer.CurrentPoints >= stepCost)
            {
                if (m_Map.GetMapObjectByVector(m_MapObject.Pos + input) == null)
                {
                    m_ActionPointsContainer.CurrentPoints -= stepCost;
                    m_CurrentAnimType = animType;
                    m_CurrentPos = m_MapObject.Pos;
                    m_NextPos = m_CurrentPos + input;
                    m_MoveNow = true;
                    m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
                    m_AnimationTimer = 0;

                    if (m_PlayerInput != null)
                        m_PlayerInput.CanInput = false;
                }
            }
        }
    }

    public void Move(Vector2Int input)
    {
        Move(input, m_DefaultAnimType, m_DefaultAnimTime, m_DefaultStepCost);
    }

    public void StopMovement()
    {
        transform.position = new Vector3(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y), 0);
        m_MoveNow = false;

        m_Map.Cells[m_MapObject.Pos.x, m_MapObject.Pos.y] = null;
        m_MapObject.Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
        m_Map.Cells[m_MapObject.Pos.x, m_MapObject.Pos.y] = m_MapObject;

        if (m_ActionPointsContainer.CurrentPoints != 0)
        {
            m_ActiveObjectsQueue.StartNextAction();
        }
        else
        {
            m_ActiveObjectsQueue.SkipTheTurn();
        }
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