using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具の装備やUIの切り替えなどの管理処理
/// </summary>
public class UIManager : MonoBehaviour
{
    //コンポーネント==============================================
    //プレイヤーの道具処理コンポーネント
    [SerializeField] GameObject playerObj;
    ToolController toolCon;

    //装備中の道具アイコンを表示するUIのimage
    [SerializeField] GameObject nowToolIcon;
    Image nowToolIconImage;

    //UI関連=====================================================
    //道具アイコン画像（PlayerCon.ToolStatus順）
    [SerializeField] Sprite[] toolIconImages;



    // Start is called before the first frame update
    void Start()
    {
        toolCon = playerObj.gameObject.GetComponent<ToolController>();
        nowToolIconImage = nowToolIcon.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 道具アイコンタップ時
    /// </summary>
    /// <param name="iconNum">タップした道具アイコンの番号(ToolController.ToolStatus基準)</param>
    public void TapToolIcon(int iconNum)
    {
        if (toolCon == null) return;

        //装備中の道具アイコンがタップされたか
        bool isNowTool = (int)toolCon.nowTool == iconNum;

        if (isNowTool)
        {
            //装備を外す
            toolCon.nowTool = ToolController.ToolStatus.None;
        }
        else
        {
            //タップした道具を装備する
            toolCon.nowTool = (ToolController.ToolStatus)iconNum;
        }

        //道具使用範囲の表示切り替え
        toolCon.toolFrame.SetActive(!isNowTool);

        //装備中の道具アイコン切り替え
        nowToolIcon.SetActive(!isNowTool);
        nowToolIconImage.sprite = toolIconImages[iconNum - 1];
    }
}
