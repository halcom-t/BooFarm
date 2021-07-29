using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{

    //道具
    enum ToolStatus{
        None, 
        Kuwa    //クワ
    }

    //タップした位置
    Vector3 tapPos;

    //長押し時間計測用タイマー
    float longTapTimer;

    //タップ判定時間
    const float tapTime = 0.15f;

    void Start()
    {
        
    }

    void Update()
    {

        //タップした時
        if (Input.GetMouseButtonDown(0))
        {
            tapPos = Input.mousePosition;
            longTapTimer = 0f;
        }

        //離した時
        if (Input.GetMouseButtonUp(0))
        {
            //タップならアクション実行
            if (isTap())TapAction();
            
        }

        //長押し中
        if (Input.GetMouseButton(0))
        {
            //長押し時間の計測
            longTapTimer += Time.deltaTime;
            //移動
            Move();        
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //タップなら移動処理をしない
        if (isTap()) return;

        //スワイプしている位置を取得（画面中央 0,0）
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //移動
        transform.Translate(new Vector2(swipePosX, swipePosY).normalized * Time.deltaTime * 2);
    }

    /// <summary>
    /// タップによる各アクション
    /// </summary>
    void TapAction()
    {

    }

    /// <summary>
    /// タップか判定
    /// ※現時点での長押しがタップに該当するかを判定する
    /// </summary>
    /// <returns>タップ=true</returns>
    bool isTap()
    {
        //タップ　=　長押し時間がタップ判定時間以内　＆　タップ位置がずれていない
        return longTapTimer <= tapTime && tapPos == Input.mousePosition;
    }

}
