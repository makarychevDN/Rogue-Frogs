﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : BaseInput
{
    [Header("Setup")]
    [SerializeField] private Movable m_PlayerMovement;
    [SerializeField] private AttackableInDirection m_PlayerAttackModule;
    private bool m_InputIsPossible;

    public bool InputIsPossible { get => m_InputIsPossible; set => m_InputIsPossible = value; }
    
    private void Update()
    {
        if (m_InputIsPossible)
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<SkipTurnModule>().SkipTurn();
                m_InputIsPossible = false;
            }
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
