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
    //地面の状態（タイルの種類）
    enum GroundStatus
    {
        Normal, //草原（デフォ）
        Dry,    //乾いた畑
        Wet     //湿った畑
    }

    //地面の範囲（タイル数）
    const int GroundAreaW = 40;
    const int GroundAreaH = 30;
    //地面の各状態
    GroundStatus[,] groundStatus = new GroundStatus[GroundAreaW, GroundAreaH];

    //地面の各タイルの種類（GroundStatusの順に各タイルをセット）
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
        int x = ToolFramePosInt().x;
        int z = ToolFramePosInt().z;
    }

    /// <summary>
    /// ジョウロのアクション
    /// </summary>
    void JoroAcion()
    {

    }

    /// <summary>
    /// 種まきアクション
    /// </summary>
    void TaneAction()
    {

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
