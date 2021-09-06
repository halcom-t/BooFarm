using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //時間関連=======================================
    //1日開始時間(6:00)
    const float DayStartTime = 60f * 6;
    //1日の時間計測タイマー
    static float timeDayCounter = DayStartTime;
    //時間表示UI
    [SerializeField] Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1日の経過時間の計測
        timeDayCounter += Time.deltaTime;
        //24時を超えたら0:00にする
        if(timeDayCounter / 60 > 24) timeDayCounter = 0;
        //UIに1日の経過時間を反映
        int h = (int)timeDayCounter / 60;
        int m = (int)timeDayCounter % 60;
        timeText.text = string.Format("{0}:{1:d2}", h, m);
    }
} 
