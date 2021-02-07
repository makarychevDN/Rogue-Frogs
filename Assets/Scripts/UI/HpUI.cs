using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    private TextMeshProUGUI m_Text;

    public TextMeshProUGUI Text { get => m_Text; set => m_Text = value; }

    private void Start()
    {
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(int value)
    {
        m_Text.text = value.ToString();
    }
}
