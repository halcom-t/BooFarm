using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        //スワイプ中
        if (Input.GetMouseButton(0))
        {
            //タップしている位置を取得（画面中央 0,0）
            float tapPosX = Input.mousePosition.x - Screen.width / 2;
            float tapPosY = Input.mousePosition.y - Screen.height / 2;

            //移動
            transform.Translate(new Vector2(tapPosX, tapPosY).normalized * Time.deltaTime * 2);
            
        }
    }
}
