using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{
    //道具
    enum ToolStatus{
        None,   //装備なし
        Kuwa,   //クワ
        Joro    //ジョウロ
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
        }
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

        //移動中、道具の使用位置（マス）を更新
        UpdateToolFrame(direction);

    }

    /// <summary>
    /// 道具の使用位置（マス）の更新処理
    /// </summary>
    /// <param name="direction">プレイヤーの移動方向（ベクトル）</param>
    void UpdateToolFrame(Vector2 direction)
    {
        //道具の使用位置（プレイヤーの正面位置）の計算　=　プレイヤー位置　＋　移動方向（ベクトル）
        Vector3 toolPos = transform.position + new Vector3(direction.x, direction.y, 0);
        //道具の使用位置をマス目単位に調整
        toolPos = new Vector3(Mathf.Floor(toolPos.x) + 0.5f, Mathf.Floor(toolPos.y) + 0.5f, 0);

        //道具の使用位置を更新
        toolFrame.transform.position = toolPos;
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
