using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// プレイヤーの道具操作
/// </summary>
public class ToolController : MonoBehaviour
{
    //地面タイル関連===============================================
    //地面のタイル：種類
    enum TileIndexNum
    {
        Normal, //草原（デフォ）
        Dry,    //乾いた畑
        Wet     //湿った畑
    }

    //畑の範囲
    const int FarmAreaW = 40;
    const int FarmAreaH = 30;
    //畑の各状態
    TileIndexNum[,] groundStatus = new TileIndexNum[FarmAreaW, FarmAreaH];

    //地面の各タイルの種類
    [SerializeField] TileBase[] groundTiles;
    //地面のタイルマップ(obj)
    [SerializeField] GameObject groundObj;
    //地面のタイルマップ情報
    Tilemap groundTilemap;

    //道具関連====================================================
    //道具
    public enum ToolStatus
    {
        None,   //装備なし
        Kuwa,   //クワ
        Joro,   //ジョウロ
        Tane    //タネ
    }
    //装備中の道具
    [System.NonSerialized] public ToolStatus nowTool = ToolStatus.None;
    //道具の使用範囲
    public GameObject toolFrame;

    //作物関連====================================================
    //作物プレファブ
    [SerializeField] GameObject cropPre;

    //作物の状態
    enum CropStatus
    {
        None,
        Seed    //種
    }
    //畑の各作物の成長状態
    CropStatus[,] cropStatus = new CropStatus[FarmAreaW, FarmAreaH];



    // Start is called before the first frame update
    void Start()
    {
        //地面のタイルマップ情報
        groundTilemap = groundObj.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 各道具のアクション
    /// </summary>
    public void ToolAction()
    {

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
            case ToolStatus.Tane:
                TaneAction();
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
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Normal])
        {
            groundTilemap.SetTile(ToolFramePosInt(), groundTiles[(int)TileIndexNum.Dry]);
        }
    }

    /// <summary>
    /// ジョウロのアクション
    /// </summary>
    void JoroAcion()
    {
        //乾いた畑なら、湿った畑のタイルに変更
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Dry])
        {
            groundTilemap.SetTile(ToolFramePosInt(), groundTiles[(int)TileIndexNum.Wet]);
        }
    }

    /// <summary>
    /// 種まきアクション
    /// </summary>
    void TaneAction()
    {
        //畑じゃないマスなら種まきしない
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Normal]) return;
        //作物が植えてあるマスなら種まきしない
        if (cropStatus[ToolFramePosInt().x, ToolFramePosInt().y] != CropStatus.None) return;

        //種まき処理
        {
            //作物objの生成
            Instantiate(cropPre, ToolFramePos(), Quaternion.identity);
            //作物の状態を種状態に変更
            cropStatus[ToolFramePosInt().x, ToolFramePosInt().y] = CropStatus.Seed;
        }
    }

    /// <summary>
    /// 道具の使用位置（マス）の更新処理
    /// </summary>
    /// <param name="direction">プレイヤーの移動方向（ベクトル）</param>
    public void UpdateToolFrame(Vector2 direction)
    {
        //道具の使用位置（プレイヤーの正面位置）の計算　=　プレイヤー位置　＋　移動方向（ベクトル）
        Vector3 toolPos = transform.position + new Vector3(direction.x, direction.y, 0);
        //道具の使用位置をマス目単位に調整
        toolPos = new Vector3(Mathf.Floor(toolPos.x) + 0.5f, Mathf.Floor(toolPos.y) + 0.5f, 0);

        //道具の使用位置を更新
        toolFrame.transform.position = toolPos;
    }

    /// <summary>
    /// 道具使用マスの位置をタイル位置指定形式(int)で取得
    /// </summary>
    /// <returns></returns>
    Vector3Int ToolFramePosInt()
    {
        int x = (int)(toolFrame.transform.position.x - 0.5f);
        int y = (int)(toolFrame.transform.position.y - 0.5f);

        return new Vector3Int(x, y, 0);
    }

    /// <summary>
    /// 道具使用マスの位置をタイル位置指定形式(float)で取得
    /// </summary>
    /// <returns></returns>
    Vector3 ToolFramePos()
    {
        float x = toolFrame.transform.position.x - 0.5f;
        float y = toolFrame.transform.position.y - 0.5f;

        return new Vector3(x, y, 0);
    }
}
