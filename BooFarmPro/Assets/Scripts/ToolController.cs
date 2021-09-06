using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �v���C���[�̓����
/// </summary>
public class ToolController : MonoBehaviour
{
    //�n�ʃ^�C���֘A===============================================
    //�n�ʂ̃^�C���F���
    enum TileIndexNum
    {
        Normal, //�����i�f�t�H�j
        Dry,    //��������
        Wet     //��������
    }

    //���͈̔�
    const int FarmAreaW = 40;
    const int FarmAreaH = 30;
    //���̊e���
    TileIndexNum[,] groundStatus = new TileIndexNum[FarmAreaW, FarmAreaH];

    //�n�ʂ̊e�^�C���̎��
    [SerializeField] TileBase[] groundTiles;
    //�n�ʂ̃^�C���}�b�v(obj)
    [SerializeField] GameObject groundObj;
    //�n�ʂ̃^�C���}�b�v���
    Tilemap groundTilemap;

    //����֘A====================================================
    //����
    public enum ToolStatus
    {
        None,   //�����Ȃ�
        Kuwa,   //�N��
        Joro,   //�W���E��
        Tane    //�^�l
    }
    //�������̓���
    [System.NonSerialized] public ToolStatus nowTool = ToolStatus.None;
    //����̎g�p�͈�
    public GameObject toolFrame;

    //�앨�֘A====================================================
    //�앨�v���t�@�u
    [SerializeField] GameObject cropPre;

    //�앨�̏��
    enum CropStatus
    {
        None,
        Seed    //��
    }
    //���̊e�앨�̐������
    CropStatus[,] cropStatus = new CropStatus[FarmAreaW, FarmAreaH];



    // Start is called before the first frame update
    void Start()
    {
        //�n�ʂ̃^�C���}�b�v���
        groundTilemap = groundObj.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �e����̃A�N�V����
    /// </summary>
    public void ToolAction()
    {

        //�������Ă��铹��ŃA�N�V������؂�ւ�
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
    /// �N���̃A�N�V����
    /// </summary>
    void KuwaAction()
    {
        //�����^�C��(def)�Ȃ�A���������̃^�C���ɕύX
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Normal])
        {
            groundTilemap.SetTile(ToolFramePosInt(), groundTiles[(int)TileIndexNum.Dry]);
        }
    }

    /// <summary>
    /// �W���E���̃A�N�V����
    /// </summary>
    void JoroAcion()
    {
        //���������Ȃ�A���������̃^�C���ɕύX
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Dry])
        {
            groundTilemap.SetTile(ToolFramePosInt(), groundTiles[(int)TileIndexNum.Wet]);
        }
    }

    /// <summary>
    /// ��܂��A�N�V����
    /// </summary>
    void TaneAction()
    {
        //������Ȃ��}�X�Ȃ��܂����Ȃ�
        if (groundTilemap.GetTile(ToolFramePosInt()) == groundTiles[(int)TileIndexNum.Normal]) return;
        //�앨���A���Ă���}�X�Ȃ��܂����Ȃ�
        if (cropStatus[ToolFramePosInt().x, ToolFramePosInt().y] != CropStatus.None) return;

        //��܂�����
        {
            //�앨obj�̐���
            Instantiate(cropPre, ToolFramePos(), Quaternion.identity);
            //�앨�̏�Ԃ����ԂɕύX
            cropStatus[ToolFramePosInt().x, ToolFramePosInt().y] = CropStatus.Seed;
        }
    }

    /// <summary>
    /// ����̎g�p�ʒu�i�}�X�j�̍X�V����
    /// </summary>
    /// <param name="direction">�v���C���[�̈ړ������i�x�N�g���j</param>
    public void UpdateToolFrame(Vector2 direction)
    {
        //����̎g�p�ʒu�i�v���C���[�̐��ʈʒu�j�̌v�Z�@=�@�v���C���[�ʒu�@�{�@�ړ������i�x�N�g���j
        Vector3 toolPos = transform.position + new Vector3(direction.x, direction.y, 0);
        //����̎g�p�ʒu���}�X�ڒP�ʂɒ���
        toolPos = new Vector3(Mathf.Floor(toolPos.x) + 0.5f, Mathf.Floor(toolPos.y) + 0.5f, 0);

        //����̎g�p�ʒu���X�V
        toolFrame.transform.position = toolPos;
    }

    /// <summary>
    /// ����g�p�}�X�̈ʒu���^�C���ʒu�w��`��(int)�Ŏ擾
    /// </summary>
    /// <returns></returns>
    Vector3Int ToolFramePosInt()
    {
        int x = (int)(toolFrame.transform.position.x - 0.5f);
        int y = (int)(toolFrame.transform.position.y - 0.5f);

        return new Vector3Int(x, y, 0);
    }

    /// <summary>
    /// ����g�p�}�X�̈ʒu���^�C���ʒu�w��`��(float)�Ŏ擾
    /// </summary>
    /// <returns></returns>
    Vector3 ToolFramePos()
    {
        float x = toolFrame.transform.position.x - 0.5f;
        float y = toolFrame.transform.position.y - 0.5f;

        return new Vector3(x, y, 0);
    }
}
