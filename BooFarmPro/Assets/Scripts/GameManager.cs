using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// タイル情報
/// </summary>
[System.Serializable]
public class TileData
{
    public int tileX;           //タイルのX座標
    public int tileY;           //タイルのY座標
    public int groundStatus;    //地面の状態
    //public int cropStatus;      //作物の成長状態
    //public int cropType;        //作物の種類
}

/// <summary>
/// セーブデータの構造
/// </summary>
[System.Serializable]
public class SaveData
{
    public float time;              //ゲーム内：時間
    public int day;                 //ゲーム内：日付
    public List<TileData> tileData; //各タイルの情報
}


/// <summary>
/// ゲームの管理
/// </summary>
public class GameManager : MonoBehaviour
{
    //日時関連=======================================
    //1日開始時間(6:00)
    const float DayStartTime = 60f * 6;
    //ゲーム内：時間
    static float gameTime = DayStartTime;
    //ゲーム内：日付
    static int gameDay = 1;
    //時間表示UI
    [SerializeField] Text timeText;
    //日付表示UI
    [SerializeField] Text dayText;

    //コンポーネント==================================
    ToolController toolCon;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのToolControllerを取得（Mainシーンでしかアタッチされていない）
        GameObject playerObj = this.transform.parent.gameObject;
        toolCon = null;
        if (playerObj != null)
        {
            toolCon = playerObj.GetComponent<ToolController>();
        }      

        //日時UIを反映
        GameTimeReflect();
        GameDayReflect();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム内時間のカウント
        GameTimeCount();
    }

    /// <summary>
    /// アプリが終了する時
    /// </summary>
    void OnApplicationQuit()
    {
        //ToolControllerがあるなら（Mainシーンで道具が使える状況なら）
        if (toolCon != null)
        {
            //データをセーブ（タイル情報更新）
            Save(toolCon.GetTileData());
        }
        else
        {
            //データをセーブ（タイル情報は更新しない）
            Save();
        }
        Debug.Log("終了");
    }

    /// <summary>
    /// アプリが中断、再開（起動）される時
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause(bool pauseStatus)
    {
        //一時停止
        if (pauseStatus)
        {
            //ToolControllerがあるなら（Mainシーンで道具が使える状況なら）
            if (toolCon != null)
            {
                //データをセーブ（タイル情報更新）
                Save(toolCon.GetTileData());
            }
            else
            {
                //データをセーブ（タイル情報は更新しない）
                Save();
            }
            Debug.Log("中断");
        }
        //再開時（起動時）
        else
        {
            Debug.Log("再開");
        }
    }

    /// <summary>
    /// ゲームデータの保存
    /// </summary>
    /// <param name="tileData">全タイル情報(上書きしない時はnullを渡す)</param>
    public void Save(List<TileData> tileData = null)
    {
        SaveData data = new SaveData();
        data.time = gameTime;
        data.day = gameDay;
        //タイルデータがあれば上書き、nullなら既存データのまま上書き
        if (tileData != null)
        {
            data.tileData = new List<TileData>(tileData);
        }
        else
        {
            SaveData nowData  = Load();
            data.tileData = new List<TileData>(nowData.tileData);
        }

        using (StreamWriter writer = new StreamWriter(Application.dataPath + "/savedata.json", false))
        {
            string jsonstr = JsonUtility.ToJson(data);
            Debug.Log(jsonstr);
            writer.Write(jsonstr);
            writer.Flush();
        }
    }

    /// <summary>
    /// データを翌日（朝6:00）にしてセーブする
    /// </summary>
    public void SaveNextDayData()
    {
        //現在のデータをロード
        SaveData nowData = Load();
        //現在のデータをもとに、翌日のデータを作成
        SaveData nextdayData = new SaveData();
        nextdayData.time = DayStartTime;
        nextdayData.day = nowData.day + 1;
        nextdayData.tileData = new List<TileData>();
        //現在の全タイルデータをもとに、翌日のデータにを作成
        foreach (TileData nowTile in nowData.tileData)
        {
            TileData nextdayTile = new TileData();
            nextdayTile.tileX = nowTile.tileX;
            nextdayTile.tileY = nowTile.tileY;
            nextdayTile.groundStatus = nowTile.groundStatus;
            //畑を乾いた畑にする
            if (nextdayTile.groundStatus >= 1)
            {
                nextdayTile.groundStatus = 1;
            }
            nextdayData.tileData.Add(nextdayTile);
        }

        //翌日データのセーブ
        using (StreamWriter writer = new StreamWriter(Application.dataPath + "/savedata.json", false))
        {
            string jsonstr = JsonUtility.ToJson(nextdayData);
            Debug.Log(jsonstr);
            writer.Write(jsonstr);
            writer.Flush();
        }

        //UI反映
        GameTimeReflect();
        GameDayReflect();
    }

    /// <summary>
    /// ゲームデータのロード
    /// </summary>
    public SaveData Load()
    {
        SaveData data = null;
        using (StreamReader reader = new StreamReader(Application.dataPath + "/savedata.json"))
        {
            string datastr = "";
            datastr = reader.ReadToEnd();
            data = JsonUtility.FromJson<SaveData>(datastr);
        };
        return data;
    }

    /// <summary>
    /// ゲーム内時間のカウント処理
    /// </summary>
    public void GameTimeCount()
    {
        //1日の経過時間の計測
        gameTime += Time.deltaTime;
        //24時を超えたら0:00にする
        if (gameTime / 60 > 24) gameTime = 0;
        //UI反映
        GameTimeReflect();
    }

    /// <summary>
    /// UIに1日の経過時間を反映
    /// </summary>
    void GameTimeReflect()
    {
        int h = (int)gameTime / 60;
        int m = (int)gameTime % 60;
        timeText.text = string.Format("{0}:{1:d2}", h, m);
    }

    /// <summary>
    /// UIに日付を反映
    /// </summary>
    void GameDayReflect()
    {
        dayText.text = gameDay + "日";
    }
} 
