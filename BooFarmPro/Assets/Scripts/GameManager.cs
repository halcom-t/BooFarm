using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //���Ԋ֘A=======================================
    //1���J�n����(6:00)
    const float DayStartTime = 60f * 6;
    //1���̎��Ԍv���^�C�}�[
    static float timeDayCounter = DayStartTime;
    //���ԕ\��UI
    [SerializeField] Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1���̌o�ߎ��Ԃ̌v��
        timeDayCounter += Time.deltaTime;
        //24���𒴂�����0:00�ɂ���
        if(timeDayCounter / 60 > 24) timeDayCounter = 0;
        //UI��1���̌o�ߎ��Ԃ𔽉f
        int h = (int)timeDayCounter / 60;
        int m = (int)timeDayCounter % 60;
        timeText.text = string.Format("{0}:{1:d2}", h, m);
    }
} 
