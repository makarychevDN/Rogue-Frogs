using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : BaseInput
{
    [Header("Setup")]
    [SerializeField] private Movable m_PlayerMovement;
    [SerializeField] private AttackableInDirection m_PlayerAttackModule;
    private bool m_InputIsPossible;
    private bool m_IsShowAllUINow;
    private List<ShowUI> m_ShowUIList = new List<ShowUI>();
    
    public bool InputIsPossible { get => m_InputIsPossible; set => m_InputIsPossible = value; }
    
    private void Update()
    {
        if (m_InputIsPossible)
        {
            
            CheckKeysPressed(KeyCode.W, Vector2Int.up);
            CheckKeysPressed(KeyCode.A, Vector2Int.left);
            CheckKeysPressed(KeyCode.S, Vector2Int.down);
            CheckKeysPressed(KeyCode.D, Vector2Int.right);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<SkipTurnModule>().SkipTurn();
                m_InputIsPossible = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                
            }
        }
    }

    public void CheckKeysPressed(KeyCode keyCode, Vector2Int vector2Int)
    {
        if (Input.GetKeyDown(keyCode) && Input.GetKey(KeyCode.LeftShift) && m_PlayerAttackModule.CheckAttackIsPossible(vector2Int))
        {
            m_PlayerMovement.Move(vector2Int);
            m_InputIsPossible = false;
        }
        else if (Input.GetKeyDown(keyCode) && m_PlayerAttackModule.CheckAttackIsPossible(vector2Int))
        {
            m_PlayerAttackModule.Attack(vector2Int);
            m_InputIsPossible = false;
        }
        else if (Input.GetKeyDown(keyCode))
        {
            m_PlayerMovement.Move(vector2Int);
            m_InputIsPossible = false;
        }
    }

    private void Reset()
    {
        m_PlayerMovement = GetComponent<Movable>();
        m_PlayerAttackModule = GetComponent<AttackableInDirection>();
    }

    public override void DoSomething()
    {
        InputIsPossible = true;
    }
}
