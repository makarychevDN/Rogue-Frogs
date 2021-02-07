using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Movable m_PlayerMovement;
    [SerializeField] private Attackable m_PlayerAttackModule;
    [SerializeField] private bool m_CanInput;

    public bool CanInput { get => m_CanInput; set => m_CanInput = value; }

    private void Update()
    {
        if (m_CanInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_PlayerMovement.Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_PlayerMovement.Move(Vector2Int.up);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_PlayerMovement.Move(Vector2Int.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_PlayerMovement.Move(Vector2Int.right);
            }


            if (Input.GetKeyDown(KeyCode.S) && m_PlayerAttackModule.CheckAttackIsPossible(Vector2Int.down))
            {
                m_PlayerAttackModule.Attack(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.W) && m_PlayerAttackModule.CheckAttackIsPossible(Vector2Int.up))
            {
                m_PlayerAttackModule.Attack(Vector2Int.up);
            }

            if (Input.GetKeyDown(KeyCode.A) && m_PlayerAttackModule.CheckAttackIsPossible(Vector2Int.left))
            {
                m_PlayerAttackModule.Attack(Vector2Int.left);
            }

            if (Input.GetKeyDown(KeyCode.D) && m_PlayerAttackModule.CheckAttackIsPossible(Vector2Int.right))
            {
                m_PlayerAttackModule.Attack(Vector2Int.right);
            }
        }
    }

    private void Start()
    {
        m_PlayerMovement = GetComponent<Movable>();
    }
}
