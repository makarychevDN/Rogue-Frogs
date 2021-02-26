using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjSpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform m_Sprite;
    public void RotateSprite(Vector2Int input)
    {
        if(input == Vector2Int.right)
            m_Sprite.rotation = Quaternion.Euler(0,0,0);
        else if (input == Vector2Int.left)
        {
            m_Sprite.rotation = Quaternion.Euler(0,180,0);
        }
    }
}
