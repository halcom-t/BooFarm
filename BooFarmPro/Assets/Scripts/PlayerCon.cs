using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{
    //方向（現在向いている方向はアニメーションパラメータ「Direction」から取得）
    enum DirectionType
    {
        front,  //前（正面）
        back,   //後
        left,   //左
        right   //右
    }

    //道具
    enum ToolStatus{
        None, 
        Kuwa    //クワ
    }
    //装備中の道具＜-------------------------------------デバッグ後Noneにする
    ToolStatus nowTool = ToolStatus.Kuwa;

    //道具の使用範囲
    [SerializeField] GameObject toolFrame;

   //地面の各タイル＜-----------------------------------画像差し替え必要（index増えるかも）
   [SerializeField] TileBase[] groundTiles;
    //地面のタイルマップ
    [SerializeField] GameObject groundObj;
    //地面のタイルマップ情報
    Tilemap groundTilemap;

    //プレイヤーのアニメーター
    Animator anim;

    //タップした位置
    Vector3 tapPos;
    //長押し時間計測用タイマー
    float longTapTimer;
    //タップ判定時間
    const float tapTime = 0.15f;

    void Start()
    {
        //地面のタイルマップ情報
        groundTilemap = groundObj.GetComponent<Tilemap>();
        //アニメーター
        anim = GetComponent<Animator>();
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
            if (IsTap())TapAction();        
        }

        //長押し中
        if (Input.GetMouseButton(0))
        {
            //長押し時間の計測
            longTapTimer += Time.deltaTime;
            //移動
            Move();
            //道具の使用範囲の計算
            ToolFramePos();
        }
    }

    /// <summary>
    /// 道具の使用範囲の計算
    /// </summary>
    void ToolFramePos()
    {
        float x = Mathf.Floor(transform.position.x);
        float y = Mathf.Floor(transform.position.y);
        float z = 0f;

        //各方向を向いているときの道具使用範囲の位置を計算＜-----------------------------------コード短くしたい
        if (anim.GetInteger("Direction") == (int)DirectionType.front)
        {
            x += 0.5f;
            y -= 0.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.back)
        {
            x += 0.5f;
            y += 1.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.left)
        {
            x -= 0.5f;
            y += 0.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.right)
        {
            x += 1.5f;
            y += 0.5f;
        }

        //道具使用範囲の位置を設定
        Vector3 toolPos = new Vector3(x, y, z);
        toolFrame.transform.position = toolPos;
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //タップなら移動処理をしない
        if (IsTap()) return;

        //スワイプしている位置を取得（画面中央 0,0）
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //移動
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        transform.Translate(direction * Time.deltaTime * 2);

        //現在の方向をアニメーションパラメータ「Direction」に設定
        anim.SetInteger("Direction", (int)IsDirection(direction));
    }

    /// <summary>
    /// 向いている方向を判定
    /// </summary>
    /// <param name="nowDirection">方向判定用のベクトル</param>
    /// <returns>現在向いている方向</returns>
    DirectionType IsDirection(Vector2 directionVector)
    {
        //ベクトルからどの方向を向かせるか（4方向）判定＜-----------------------------------ここの処理三角関数使いたい
        if (Mathf.Abs(directionVector.x) <= Mathf.Abs(directionVector.y))
        {
            if (directionVector.y <= 0) return DirectionType.front;
            else return DirectionType.back;
        }
        else
        {
            if (directionVector.x <= 0) return DirectionType.left;
            else return DirectionType.right;
        }
    }

    /// <summary>
    /// タップによる各アクション
    /// </summary>
    void TapAction()
    {
        //装備中の各道具処理
        switch (nowTool)
        {
            case ToolStatus.None:   //装備なし
                break;
            case ToolStatus.Kuwa:   //クワ
                KuwaAction();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// クワ装備時のアクション
    /// </summary>
    void KuwaAction()
    {
        //groundTilemap.SetTile(toolPos, groundTiles[1]);  
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

}
