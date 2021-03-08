using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private int m_Score;

    public void AddScore(int score)
    {
        m_Score += score;
        m_Text.text = m_Score.ToString();
    }
}
