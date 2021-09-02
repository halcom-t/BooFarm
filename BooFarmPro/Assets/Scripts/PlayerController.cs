using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの基本操作
/// </summary>
public class PlayerController : MonoBehaviour
{
    //操作オブジェクト関連=======================================
    //操作できるオブジェクトのタグ
    enum OperationObjTags
    {
        None,
        Home,   //UFO
        Bed     //ベッド
    }
    //現在接触中のアクションオブジェクト（タグ）
    string nowOperationObjTag;

    //操作関連====================================================
    //タップした位置
    Vector3 tapPos;
    //長押し時間計測用タイマー
    float longTapTimer;
    //タップ判定時間
    const float tapTime = 0.15f;

    //コンポーネント==============================================
    Rigidbody2D rigidbody;
    ToolController toolCon;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        toolCon = GetComponent<ToolController>();
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
            //移動速度をリセット
            rigidbody.velocity = new Vector2(0, 0);
            //タップならアクション実行
            if (IsTap()) TapAction();
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
    /// ※ここに書く処理はサブクラス（PlayerToolController）にも影響が出るから注意
    /// </summary>
    /// <returns>サブクラスでdirection（移動ベクトル）を取得する用の戻り値</returns>
    void Move()
    {
        //タップなら移動処理をしない
        if (IsTap()) return;

        //UIエリアなら移動処理をしない
        if (IsTappingUIArea())
        {
            //移動速度をリセット
            rigidbody.velocity = new Vector2(0, 0);
            return;
        }

        //スワイプしている位置を取得（画面中央 0,0）
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //移動
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        rigidbody.velocity = direction * 3;

        //移動中、道具の使用位置（マス）を更新
        if (toolCon != null) toolCon.UpdateToolFrame(direction);
        
    }

    /// <summary>
    /// タップによるアクション
    /// </summary>
    void TapAction()
    {
        //UIエリアなら処理をしない
        if (IsTappingUIArea()) return;

        //接触中の操作オブジェクトがあるならアクション
        if (nowOperationObjTag != "")
        {
            OperationObjAcion();
            return;
        }

        //道具アクション
        if (toolCon != null) toolCon.ToolAction();
    }

    /// <summary>
    /// 操作オブジェクトのアクション
    /// </summary>
    void OperationObjAcion()
    {
        //UFO(拠点)
        if (nowOperationObjTag == OperationObjTags.Home.ToString())
        {
            //UFOに入る（HomeSceneのロード）
            SceneManager.LoadScene("Home");
        }
    }

    /// <summary>
    /// タップか判定
    /// ※現時点での長押しがタップに該当するかを判定する
    /// </summary>
    /// <returns>タップ=true</returns>
    bool IsTap()
    {
        //タップ　=　長押し時間がタップ判定時間以内　＆　タップ位置がずれていない
        return longTapTimer <= tapTime && tapPos == Input.mousePosition;
    }

    /// <summary>
    /// UIスペースをタップ（操作）しているか
    /// </summary>
    /// <returns>UIスペースの時true</returns>
    bool IsTappingUIArea()
    {
        //UI範囲（縦幅）を120pxとして計算
        return Input.mousePosition.y > Screen.height - 120 || Input.mousePosition.y < 120;
    }

    /// <summary>
    /// オブジェクトに接触した時
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //接触中のオブジェクトのタグ保存
        nowOperationObjTag = collision.gameObject.tag;

        //Debug.Log("ついた：" + collision.gameObject.tag);
        //Debug.Log(nowOperationObjTag);
    }

    /// <summary>
    /// オブジェクトから離れた時
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //接触中のオブジェクトのタグリセット
        if (nowOperationObjTag == collision.gameObject.tag)
        {
            nowOperationObjTag = "";
        }

        //Debug.Log("離れる：" + collision.gameObject.tag);
        //Debug.Log(nowOperationObjTag);
    }

}
