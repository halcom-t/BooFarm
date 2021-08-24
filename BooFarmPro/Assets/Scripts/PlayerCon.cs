using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// プレイヤー関連
/// </summary>
public class PlayerCon : MonoBehaviour
{
    //道具関連====================================================
    //道具
    public enum ToolStatus
    {
        None,   //装備なし
        Kuwa,   //クワ
        Joro    //ジョウロ
    }
    //装備中の道具
    [System.NonSerialized]public ToolStatus nowTool = ToolStatus.None;
    //道具の使用範囲
    public GameObject toolFrame;

    //タイル関連==================================================
    //タイル番号
    enum TileIndexNum
    {
        Normal, //草原（デフォ）
        Dry,    //乾いた畑
        Wet     //湿った畑
    }
    //地面の各タイル
    [SerializeField] TileBase[] groundTiles;
    //地面のタイルマップ
    [SerializeField] GameObject groundObj;
    //地面のタイルマップ情報
    Tilemap groundTilemap;

    //操作関連====================================================
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

        //UIの操作中は移動処理をしない
        if(IsTappingUIArea()) return;

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
        //UIの操作中は処理をしない
        if (IsTappingUIArea()) return;

        //装備している道具でアクションを切り替え
        switch (nowTool)
        {
            case ToolStatus.None:
                break;
            case ToolStatus.Kuwa:
                KuwaAction();
                break;
            case ToolStatus.Joro:
                JoroAcion();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// クワのアクション
    /// </summary>
    void KuwaAction()
    {
        //草原タイル(def)なら、乾いた畑のタイルに変更
        if (groundTilemap.GetTile(ToolFrameTilePos()) == groundTiles[(int)TileIndexNum.Normal])
        {
            groundTilemap.SetTile(ToolFrameTilePos(), groundTiles[(int)TileIndexNum.Dry]);
        }      
    }

    /// <summary>
    /// ジョウロのアクション
    /// </summary>
    void JoroAcion()
    {
        //乾いた畑なら、湿った畑のタイルに変更
        if (groundTilemap.GetTile(ToolFrameTilePos()) == groundTiles[(int)TileIndexNum.Dry])
        {
            groundTilemap.SetTile(ToolFrameTilePos(), groundTiles[(int)TileIndexNum.Wet]);
        }
    }

    /// <summary>
    /// 道具使用マスの位置をタイル位置指定形式に変換
    /// </summary>
    /// <returns></returns>
    Vector3Int ToolFrameTilePos()
    {
        int x = (int)(toolFrame.transform.position.x - 0.5f);
        int y = (int)(toolFrame.transform.position.y - 0.5f);

        return new Vector3Int(x, y, 0);
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

}
