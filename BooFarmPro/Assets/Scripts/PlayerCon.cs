using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{

    Vector2 pos = Vector2.zero;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float speedX = Input.mousePosition.x - Screen.width / 2;
            float speedY = Input.mousePosition.y - Screen.height / 2;
            transform.Translate(new Vector2(speedX , speedY) * Time.deltaTime * 0.01f);
        }
    }
}
