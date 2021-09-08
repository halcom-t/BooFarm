using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// �^�C�����
/// </summary>
[System.Serializable]
public class TileData
{
    public int tileX;           //�^�C����X���W
    public int tileY;           //�^�C����Y���W
    public int groundStatus;    //�n�ʂ̏��
    //public int cropStatus;      //�앨�̐������
    //public int cropType;        //�앨�̎��
}

/// <summary>
/// �Z�[�u�f�[�^�̍\��
/// </summary>
[System.Serializable]
public class SaveData
{
    public float time;              //�Q�[�����F����
    public int day;                 //�Q�[�����F���t
    public List<TileData> tileData; //�e�^�C���̏��
}


/// <summary>
/// �Q�[���̊Ǘ�
/// </summary>
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
    /// �Q�[���f�[�^�̕ۑ�
    /// </summary>
    /// <param name="tileData">�S�^�C�����(�㏑�����Ȃ�����null��n��)</param>
    public void Save(List<TileData> tileData = null)
    {
        SaveData data = new SaveData();
        data.time = gameTime;
        data.day = gameDay;
        if (tileData != null)
        {
            data.tileData = new List<TileData>(tileData);
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
    /// �Q�[���f�[�^�̃��[�h
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
    }

} 
