using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsStateMashine : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AnimationClip m_Stay;
    [SerializeField] private AnimationClip m_Attack;
    [SerializeField] private AnimationClip m_ApplyDamage;

    public void ActivateStayAnim()
    {
        if(m_Stay != null)
            m_Animator.Play(m_Stay.name);
    }
    
    public void ActivateAttackAnim()
    {
        if(m_Attack != null)
            m_Animator.Play(m_Attack.name);
    }
    
    public void ActivateApplyDamageAnim()
    {
        if(m_ApplyDamage != null)
            m_Animator.Play(m_ApplyDamage.name);
    }
}
