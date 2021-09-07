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
    //�n�ʂ̏�ԁi�^�C���̎�ށj
    enum GroundStatus
    {
        Normal, //�����i�f�t�H�j
        Dry,    //��������
        Wet     //��������
    }

    //�n�ʂ͈̔́i�^�C�����j
    const int GroundAreaW = 40;
    const int GroundAreaH = 30;
    //�n�ʂ̊e���
    GroundStatus[,] groundStatus = new GroundStatus[GroundAreaW, GroundAreaH];

    //�n�ʂ̊e�^�C���̎�ށiGroundStatus�̏��Ɋe�^�C�����Z�b�g�j
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
        int x = ToolFramePosInt().x;
        int z = ToolFramePosInt().z;
    }

    /// <summary>
    /// �W���E���̃A�N�V����
    /// </summary>
    void JoroAcion()
    {

    }

    /// <summary>
    /// ��܂��A�N�V����
    /// </summary>
    void TaneAction()
    {

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
