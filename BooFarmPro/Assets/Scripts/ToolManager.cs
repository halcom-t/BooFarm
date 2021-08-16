using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具の装備やUIの切り替えなどの管理処理
/// </summary>
public class ToolManager : MonoBehaviour
{
    PlayerCon playerCon;

    //装備中の道具アイコンを表示するUI
    [SerializeField] GameObject nowToolIcon;
    Image nowToolIconImage;

    //道具アイコン画像（PlayerCon.ToolStatus順）
    [SerializeField] Sprite[] toolIconImages;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponent<PlayerCon>();
        nowToolIconImage = nowToolIcon.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 道具アイコンタップ時
    /// </summary>
    /// <param name="iconNum">タップした道具アイコンの番号</param>
    public void TapToolIcon(int iconNum)
    {
        //装備中の道具アイコンがタップされたか
        bool isNowTool = (int)playerCon.nowTool == iconNum;

        if (isNowTool)
        {
            //装備を外す
            playerCon.nowTool = PlayerCon.ToolStatus.None;
        }
        else
        {
            //タップした道具を装備する
            playerCon.nowTool = (PlayerCon.ToolStatus)iconNum;
        }
       
        //道具使用範囲の表示切り替え
        playerCon.toolFrame.SetActive(!isNowTool);

        //装備中の道具アイコン切り替え
        nowToolIcon.SetActive(!isNowTool);
        nowToolIconImage.sprite = toolIconImages[iconNum - 1];
    }
}
