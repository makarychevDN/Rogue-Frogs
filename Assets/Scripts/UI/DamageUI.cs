using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;

    public TextMeshProUGUI Text { get => m_Text; set => m_Text = value; }

    private void Reset()
    {
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(int value)
    {
        m_Text.text = value.ToString();
    }
}
