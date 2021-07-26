using System;
using UnityEngine;
using UnityEngine.Events;

public class Movable : MonoBehaviour
{
    [Header("References Setup")] 
    [SerializeField] private MapObject m_MapObject;
    [Header("Setup")] 
    [SerializeField] private bool m_Pushable;
    [SerializeField] private int m_DefaultPushCost;
    [SerializeField] private int m_DefaultStepCost;
    [SerializeField] private float m_DefaultAnimTime;
    [SerializeField] private AnimType m_DefaultAnimType;
        
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

    #region Properties
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
    public int DefaultStepCost
    {
        get => m_DefaultStepCost;
        set => m_DefaultStepCost = value;
    }

    public int DefaultPushCost
    {
        get => m_DefaultPushCost;
        set => m_DefaultPushCost = value;
    }

    #endregion
    
    private float m_AnimationTimer;
    private float m_Speed;
    private float m_CurrentAnimationTime;
    private float m_DistanceDelta = 0.02f;

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
                m_AnimationTimer = Mathf.Clamp(m_AnimationTimer, 0, m_CurrentAnimationTime);
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

    public void Move(Vector2Int startPos, Vector2Int endPos)
    {
        Move(startPos, endPos, m_DefaultAnimType, m_DefaultAnimTime, m_DefaultStepCost, true, true);
    }

    public void Move(Vector2Int input, AnimType animType, float animTime, int stepCost)
    {
        Move(input, animType, animTime, stepCost, true, true);
    }
    
    public void Move(Vector2Int input, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        if (!m_MoveNow)
        {
            m_MapObject.SpriteRotator.RotateSprite(input);

            if (m_MapObject.Map.GetMapObjectByVector(m_MapObject.Pos + input) == null)
            {
                if (m_MapObject.ActionPointsContainerModule != null && m_MapObject.ActionPointsContainerModule.CurrentPoints >= stepCost)
                {
                    StartMovementSetup(input, animType, animTime, stepCost, isStartCellBecameEmpty, isNextCellBecameFull);
                }
            }
            
            else
            {
                var tempMovable = m_MapObject.Map.GetMapObjectByVector(m_MapObject.Pos + input).GetComponent<Movable>();

                if (tempMovable != null && tempMovable.Pushable && CheckPushIsPossible(input, tempMovable))
                {
                    if (m_MapObject.ActionPointsContainerModule.CurrentPoints >= m_DefaultPushCost)
                    {
                        Push(input, tempMovable);
                    }
                }
            }
        }
    }

    private void StartMovementSetup(Vector2Int input, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        CurrentlyActiveObjects.Add(this);
        if (m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepOutEvent();
        m_MapObject.ActionPointsContainerModule.CurrentPoints -= stepCost;
        m_CurrentAnimType = animType;
        m_CurrentAnimationTime = animTime;
        m_CurrentPos = m_MapObject.Pos;
        m_NextPos = m_CurrentPos + input;
        m_MoveNow = true;
        m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
        m_AnimationTimer = 0;
        m_IsNextCellBecameFull = isNextCellBecameFull;
        if(isStartCellBecameEmpty)
            m_MapObject.Map.SetMapObjectByVector(m_MapObject.Pos, null);
        
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
            m_MapObject.SpriteRotator.RotateSprite(endPos - startPos);

            if (m_MapObject.Map.GetMapObjectByVector(endPos) == null)
            {
                if (m_MapObject.ActionPointsContainerModule !=null && m_MapObject.ActionPointsContainerModule.CurrentPoints >= stepCost)
                {
                    StartMovementSetup(startPos, endPos, animType, animTime, stepCost, isStartCellBecameEmpty, isNextCellBecameFull);
                }
            }
        }
    }
    
    
    private void StartMovementSetup(Vector2Int startPos, Vector2Int endPos, AnimType animType, float animTime, int stepCost, bool isStartCellBecameEmpty, bool isNextCellBecameFull)
    {
        CurrentlyActiveObjects.Add(this);
        if (m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepOutEvent();
        m_MapObject.ActionPointsContainerModule.CurrentPoints -= stepCost;
        m_CurrentAnimType = animType;
        m_CurrentAnimationTime = animTime;
        m_CurrentPos = m_MapObject.Pos;
        m_NextPos = endPos;
        m_MoveNow = true;
        m_Speed = (m_NextPos - m_MapObject.Pos).magnitude / animTime;
        m_AnimationTimer = 0;
        m_IsNextCellBecameFull = isNextCellBecameFull;
        if(isStartCellBecameEmpty)
            m_MapObject.Map.SetMapObjectByVector(m_MapObject.Pos, null);
        
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
        return m_MapObject.Map.GetMapObjectByVector(pushTarget.GetComponent<MapObject>().Pos + input) == null;
    }

    public void Push(Vector2Int input ,Movable pushTarget)
    {
        if (m_MapObject.Map.GetMapObjectByVector( pushTarget.GetComponent<MapObject>().Pos + input) == null)
        {
            m_MapObject.ActionPointsContainerModule.CurrentPoints -= pushTarget.DefaultPushCost;
            OnPush?.Invoke();
            pushTarget.PushingNow = true;
            pushTarget.Move(input, m_DefaultAnimType, m_DefaultAnimTime, 0);
            
            if (m_MapObject.ActionPointsContainerModule.CurrentPoints == 0)
            {
                m_MapObject.ActiveObjectsQueue.SkipTheTurn();
            }
        }
    }

    public void StopMovement()
    {
        CurrentlyActiveObjects.Remove(this);
        transform.position = new Vector3(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y), 0);
        m_MoveNow = false;
        
        m_MapObject.Pos = new Vector2Int(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y));
        
        if(m_IsNextCellBecameFull)
            m_MapObject.Map.SetMapObjectByVector(m_MapObject.Pos, m_MapObject);
        
        OnAnyMovementEnd?.Invoke();
        if(m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos) != null)
            m_MapObject.Map.GetSurfaceByVector(m_MapObject.Pos).ActivateOnStepInEvent();
        
        if (!m_PushingNow)
        {
            OnOwnMovementEnd?.Invoke();
            if (m_MapObject.ActionPointsContainerModule.CurrentPoints == 0)
            {
                m_MapObject.ActiveObjectsQueue.SkipTheTurn();
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