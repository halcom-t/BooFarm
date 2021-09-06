using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //�����֘A=======================================
    //1���J�n����(6:00)
    const float DayStartTime = 60f * 6;
    //�Q�[�����F����
    static float gameTime = DayStartTime;
    //�Q�[�����F���t
    static int gameDay = 1;
    //���ԕ\��UI
    [SerializeField] Text timeText;
    //���t�\��UI
    [SerializeField] Text dayTime;



    // Start is called before the first frame update
    void Start()
    {
        //����UI�𔽉f
        GameTimeReflect();
        GameDayReflect();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�������Ԃ̃J�E���g
        GameTimeCount();
    }

    /// <summary>
    /// �Q�[�������Ԃ̃J�E���g����
    /// </summary>
    public void GameTimeCount()
    {
        //1���̌o�ߎ��Ԃ̌v��
        gameTime += Time.deltaTime;
        //24���𒴂�����0:00�ɂ���
        if (gameTime / 60 > 24) gameTime = 0;
        //UI���f
        GameTimeReflect();
    }

    /// <summary>
    /// UI��1���̌o�ߎ��Ԃ𔽉f
    /// </summary>
    void GameTimeReflect()
    {
        int h = (int)gameTime / 60;
        int m = (int)gameTime % 60;
        timeText.text = string.Format("{0}:{1:d2}", h, m);
    }

    /// <summary>
    /// �Q�[�������t�̃J�E���g����
    /// </summary>
    public void GameDayCount()
    {
        //���t��1�����₷
        gameDay++;
        //���Ԃ����Z�b�g
        gameTime = DayStartTime;
        //UI���f
        GameDayReflect();
    }

    /// <summary>
    /// UI�ɓ��t�𔽉f
    /// </summary>
    void GameDayReflect()
    {
        dayTime.text = gameDay + "��";
        Debug.Log(gameDay);
    }

} 
