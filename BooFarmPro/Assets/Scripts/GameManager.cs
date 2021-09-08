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
    [SerializeField] Text dayText;

    //�R���|�[�l���g==================================
    ToolController toolCon;

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[��ToolController���擾�iMain�V�[���ł����A�^�b�`����Ă��Ȃ��j
        GameObject playerObj = this.transform.parent.gameObject;
        toolCon = null;
        if (playerObj != null)
        {
            toolCon = playerObj.GetComponent<ToolController>();
        }      

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
    /// �A�v�����I�����鎞
    /// </summary>
    void OnApplicationQuit()
    {
        //ToolController������Ȃ�iMain�V�[���œ���g����󋵂Ȃ�j
        if (toolCon != null)
        {
            //�f�[�^���Z�[�u�i�^�C�����X�V�j
            Save(toolCon.GetTileData());
        }
        else
        {
            //�f�[�^���Z�[�u�i�^�C�����͍X�V���Ȃ��j
            Save();
        }
        Debug.Log("�I��");
    }

    /// <summary>
    /// �A�v�������f�A�ĊJ�i�N���j����鎞
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause(bool pauseStatus)
    {
        //�ꎞ��~
        if (pauseStatus)
        {
            //ToolController������Ȃ�iMain�V�[���œ���g����󋵂Ȃ�j
            if (toolCon != null)
            {
                //�f�[�^���Z�[�u�i�^�C�����X�V�j
                Save(toolCon.GetTileData());
            }
            else
            {
                //�f�[�^���Z�[�u�i�^�C�����͍X�V���Ȃ��j
                Save();
            }
            Debug.Log("���f");
        }
        //�ĊJ���i�N�����j
        else
        {
            Debug.Log("�ĊJ");
        }
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
        //�^�C���f�[�^������Ώ㏑���Anull�Ȃ�����f�[�^�̂܂܏㏑��
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
    /// �f�[�^�𗂓��i��6:00�j�ɂ��ăZ�[�u����
    /// </summary>
    public void SaveNextDayData()
    {
        //���݂̃f�[�^�����[�h
        SaveData nowData = Load();
        //���݂̃f�[�^�����ƂɁA�����̃f�[�^���쐬
        SaveData nextdayData = new SaveData();
        nextdayData.time = DayStartTime;
        nextdayData.day = nowData.day + 1;
        nextdayData.tileData = new List<TileData>();
        //���݂̑S�^�C���f�[�^�����ƂɁA�����̃f�[�^�ɂ��쐬
        foreach (TileData nowTile in nowData.tileData)
        {
            TileData nextdayTile = new TileData();
            nextdayTile.tileX = nowTile.tileX;
            nextdayTile.tileY = nowTile.tileY;
            nextdayTile.groundStatus = nowTile.groundStatus;
            //�������������ɂ���
            if (nextdayTile.groundStatus >= 1)
            {
                nextdayTile.groundStatus = 1;
            }
            nextdayData.tileData.Add(nextdayTile);
        }

        //�����f�[�^�̃Z�[�u
        using (StreamWriter writer = new StreamWriter(Application.dataPath + "/savedata.json", false))
        {
            string jsonstr = JsonUtility.ToJson(nextdayData);
            Debug.Log(jsonstr);
            writer.Write(jsonstr);
            writer.Flush();
        }

        //UI���f
        GameTimeReflect();
        GameDayReflect();
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
    /// UI�ɓ��t�𔽉f
    /// </summary>
    void GameDayReflect()
    {
        dayText.text = gameDay + "��";
    }
} 
