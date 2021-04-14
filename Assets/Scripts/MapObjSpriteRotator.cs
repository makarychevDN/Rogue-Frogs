using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjSpriteRotator : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Sprites;
    
    public void RotateSprite(Vector2Int input)
    {
        foreach (var temp in m_Sprites)
        {
            if(input == Vector2Int.right)
                temp.rotation = Quaternion.Euler(0,0,0);
            else if (input == Vector2Int.left)
            {
                temp.rotation = Quaternion.Euler(0,180,0);
            }
        }

    }
}
