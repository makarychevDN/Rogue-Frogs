using System;
using UnityEngine;
using UnityEngine.Events;

public class Movable : MonoBehaviour
{
    //
    [SerializeField] private bool m_Pushable;
    [SerializeField] private int m_DefaultPushCost;
    [SerializeField] private int m_DefaultStepCost;
    [SerializeField] private float m_DefaultAnimTime;
    [SerializeField] private AnimType m_DefaultAnimType;

    [Header("Setup")] 
    [SerializeField] private MapObjSpriteRotator m_SpriteRotator;
    [SerializeField] private ActionPointsContainer m_ActionPointsContainer;
    [SerializeField] private MapObject m_MapObject;
    [SerializeField] private Map m_Map;
    [SerializeField] private ActiveObjectsQueue m_ActiveObjectsQueue;

    [Header("Player Setup")]
    [SerializeField] private PlayerInput m_PlayerInput;

    [Header("Events")] 
    [SerializeField] private UnityEvent OnOwnMovementStart;
    [SerializeField] private UnityEvent OnOwnMovementEnd;
    [SerializeField] private UnityEvent OnBePushedMovementStart;
    [SerializeField] private UnityEvent OnBePushedMovementEnd;
    [SerializeField] private UnityEvent OnAnyMovementStart;
    [SerializeField] private UnityEvent OnAnyMovementEnd;
    [SerializeField] private UnityEvent OnPush;


    private Vector2Int m_NextPos;
    private Vector2Int m_CurrentPos;

    private AnimType m_CurrentAnimType;
    private bool m_MoveNow;
    private bool m_PushingNow;
    private bool m_IsNextCellBecameFull;
    
    public bool PushingNow
    {
        get => m_PushingNow;
        set => m_PushingNow = value;
    }
    public bool Pushable
    {
        get => m_Pushable;
        set => m_Pushable = value;
    }

    public ActiveObjectsQueue ObjectsQueue
    {
        get => m_ActiveObjectsQueue;
        set => m_ActiveObjectsQueue = value;
    }

    public Map Map
    {
        get => m_Map;
        set => m_Map = value;
    }

    public int DefaultStepCost
    {
        get => m_DefaultStepCost;
        set => m_DefaultStepCost = value;
    }

    private float m_AnimationTimer;
    private float m_Speed;
    private float m_DistanceDelta = 0.01f;


    private void Reset()
    {
        m_ActionPointsContainer = GetComponent<ActionPointsContainer>();
        m_Map = FindObjectOfType<Map>();
        m_MapObject = GetComponent<MapObject>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_SpriteRotator = GetComponent<MapObjSpriteRotator>();
    }

    private void Awake()
    {
        m_CurrentAnimType = m_DefaultAnimType;
        m_Map = FindObjectOfType<Map>();
        m_MapObject = GetComponent<MapObject>();
        m_ActiveObjectsQueue = FindObjectOfType<ActiveObjectsQueue>();
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
    
    public void Move(Vector2Int input, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        Move(input, m_DefaultAnimType, m_DefaultAnimTime, m_DefaultStepCost, isStartCellBecameEmpty, isNextCellBecameFull);
    }

    public void Move(Vector2Int input)
    {
        Move(input, m_DefaultAnimType, m_DefaultAnimTime, m_DefaultStepCost);
    }

    public void Move(Vector2Int input, AnimType animType, float animTime, int stepCost)
    {
        Move(input, animType, animTime, stepCost, true, true);
    }
    
    public void Move(Vector2Int input, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        if (!m_MoveNow)
        {
            m_SpriteRotator.RotateSprite(input);

            if (m_Map.GetMapObjectByVector(m_MapObject.Pos + input) == null)
            {
                if (m_ActionPointsContainer!=null && m_ActionPointsContainer.CurrentPoints >= stepCost)
                {
                    StartMovementSetup(input, animType, animTime, stepCost, isStartCellBecameEmpty, isNextCellBecameFull);
                }
            }
            
            else
            {
                var tempMovable = m_Map.GetMapObjectByVector(m_MapObject.Pos + input).GetComponent<Movable>();

                if (tempMovable != null && tempMovable.Pushable && CheckPushIsPossible(input, tempMovable))
                {
                    if (m_ActionPointsContainer.CurrentPoints >= m_DefaultPushCost)
                    {
                        StartMovementSetup(input, animType, animTime, m_DefaultPushCost, isStartCellBecameEmpty, isNextCellBecameFull);
                        Push(input, tempMovable);
                    }
                }
            }
        }
    }

    private void StartMovementSetup(Vector2Int input, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        m_ActiveObjectsQueue.AddToActiveObjectsList(this);
        if(m_Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepOutEvent();
        m_ActionPointsContainer.CurrentPoints -= stepCost;
        m_CurrentAnimType = animType;
        m_CurrentPos = m_MapObject.Pos;
        m_NextPos = m_CurrentPos + input;
        m_MoveNow = true;
        m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
        m_AnimationTimer = 0;
        m_IsNextCellBecameFull = isNextCellBecameFull;
        if(isStartCellBecameEmpty)
            m_Map.SetMapObjectByVector(m_MapObject.Pos, null);
        
        OnAnyMovementStart?.Invoke();

        if (!PushingNow)
        {
            OnOwnMovementStart?.Invoke();
        }
        else
        {
            OnBePushedMovementStart?.Invoke();
        }

        if (m_PlayerInput != null)
            m_PlayerInput.InputIsPossible = false;
    }
    
    public void Move(Vector2Int startPos, Vector2Int endPos, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        if (!m_MoveNow)
        {
            m_SpriteRotator.RotateSprite(endPos - startPos);

            if (m_Map.GetMapObjectByVector(endPos) == null)
            {
                if (m_ActionPointsContainer!=null && m_ActionPointsContainer.CurrentPoints >= stepCost)
                {
                    StartMovementSetup(startPos, endPos, animType, animTime, stepCost, isStartCellBecameEmpty, isNextCellBecameFull);
                }
            }
        }
    }
    
    
    private void StartMovementSetup(Vector2Int startPos, Vector2Int endPos, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        m_ActiveObjectsQueue.AddToActiveObjectsList(this);
        if(m_Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepOutEvent();
        m_ActionPointsContainer.CurrentPoints -= stepCost;
        m_CurrentAnimType = animType;
        m_CurrentPos = m_MapObject.Pos;
        m_NextPos = endPos;
        m_MoveNow = true;
        m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
        m_AnimationTimer = 0;
        m_IsNextCellBecameFull = isNextCellBecameFull;
        if(isStartCellBecameEmpty)
            m_Map.SetMapObjectByVector(m_MapObject.Pos, null);
        
        OnAnyMovementStart?.Invoke();

        if (!PushingNow)
        {
            OnOwnMovementStart?.Invoke();
        }
        else
        {
            OnBePushedMovementStart?.Invoke();
        }

        if (m_PlayerInput != null)
            m_PlayerInput.InputIsPossible = false;
    }

    private bool CheckPushIsPossible(Vector2Int input ,Movable pushTarget)
    {
        return m_Map.GetMapObjectByVector(pushTarget.GetComponent<MapObject>().Pos + input) == null;
    }

    public void Push(Vector2Int input ,Movable pushTarget)
    {
        if (m_Map.GetMapObjectByVector( pushTarget.GetComponent<MapObject>().Pos + input) == null)
        {
            OnPush?.Invoke();
            pushTarget.PushingNow = true;
            pushTarget.Move(input,m_DefaultAnimType, m_DefaultAnimTime,0);
        }
    }

    public void StopMovement()
    {
        m_ActiveObjectsQueue.RemoveFromActiveObjectsList(this);
        transform.position = new Vector3(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y), 0);
        m_MoveNow = false;
        
        m_MapObject.Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
        
        if(m_IsNextCellBecameFull)
            m_Map.SetMapObjectByVector(m_MapObject.Pos, m_MapObject);
        
        OnAnyMovementEnd?.Invoke();
        if(m_Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepInEvent();
        
        if (!m_PushingNow)
        {
            OnOwnMovementEnd?.Invoke();
            if (m_ActionPointsContainer.CurrentPoints == 0)
            {
                m_ActiveObjectsQueue.SkipTheTurn();
            }
        }
        else
        {
            OnBePushedMovementEnd?.Invoke();
            m_PushingNow = false;
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