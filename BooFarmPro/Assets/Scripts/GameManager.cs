using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Text dayTime;



    // Start is called before the first frame update
    void Start()
    {
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
    /// ゲーム内日付のカウント処理
    /// </summary>
    public void GameDayCount()
    {
        //日付を1日増やす
        gameDay++;
        //時間をリセット
        gameTime = DayStartTime;
        //UI反映
        GameDayReflect();
    }

    /// <summary>
    /// UIに日付を反映
    /// </summary>
    void GameDayReflect()
    {
        dayTime.text = gameDay + "日";
        Debug.Log(gameDay);
    }

} 
