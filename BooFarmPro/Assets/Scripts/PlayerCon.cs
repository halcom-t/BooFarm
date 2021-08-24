using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �v���C���[�֘A
/// </summary>
public class PlayerCon : MonoBehaviour
{
    //����֘A====================================================
    //����
    public enum ToolStatus
    {
        None,   //�����Ȃ�
        Kuwa,   //�N��
        Joro    //�W���E��
    }
    //�������̓���
    [System.NonSerialized]public ToolStatus nowTool = ToolStatus.None;
    //����̎g�p�͈�
    public GameObject toolFrame;

    //�^�C���֘A==================================================
    //�^�C���ԍ�
    enum TileIndexNum
    {
        Normal, //�����i�f�t�H�j
        Dry,    //��������
        Wet     //��������
    }
    //�n�ʂ̊e�^�C��
    [SerializeField] TileBase[] groundTiles;
    //�n�ʂ̃^�C���}�b�v
    [SerializeField] GameObject groundObj;
    //�n�ʂ̃^�C���}�b�v���
    Tilemap groundTilemap;

    //����֘A====================================================
    //�^�b�v�����ʒu
    Vector3 tapPos;
    //���������Ԍv���p�^�C�}�[
    float longTapTimer;
    //�^�b�v���莞��
    const float tapTime = 0.15f;



    void Start()
    {
        //�n�ʂ̃^�C���}�b�v���
        groundTilemap = groundObj.GetComponent<Tilemap>();
    }

    void Update()
    {
        //�^�b�v������
        if (Input.GetMouseButtonDown(0))
        {
            tapPos = Input.mousePosition;
            longTapTimer = 0f;
        }

        //��������
        if (Input.GetMouseButtonUp(0))
        {
            //�^�b�v�Ȃ�A�N�V�������s
            if (IsTap())TapAction();        
        }

        //��������
        if (Input.GetMouseButton(0))
        {
            //���������Ԃ̌v��
            longTapTimer += Time.deltaTime;
            //�ړ�
            Move();
        }
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    void Move()
    {
        //�^�b�v�Ȃ�ړ����������Ȃ�
        if (IsTap()) return;

        //UI�̑��쒆�͈ړ����������Ȃ�
        if(IsTappingUIArea()) return;

        //�X���C�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //�ړ�
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        transform.Translate(direction * Time.deltaTime * 2);

        //�ړ����A����̎g�p�ʒu�i�}�X�j���X�V
        UpdateToolFrame(direction);

    }

    /// <summary>
    /// ����̎g�p�ʒu�i�}�X�j�̍X�V����
    /// </summary>
    /// <param name="direction">�v���C���[�̈ړ������i�x�N�g���j</param>
    void UpdateToolFrame(Vector2 direction)
    {
        //����̎g�p�ʒu�i�v���C���[�̐��ʈʒu�j�̌v�Z�@=�@�v���C���[�ʒu�@�{�@�ړ������i�x�N�g���j
        Vector3 toolPos = transform.position + new Vector3(direction.x, direction.y, 0);
        //����̎g�p�ʒu���}�X�ڒP�ʂɒ���
        toolPos = new Vector3(Mathf.Floor(toolPos.x) + 0.5f, Mathf.Floor(toolPos.y) + 0.5f, 0);

        //����̎g�p�ʒu���X�V
        toolFrame.transform.position = toolPos;
    }

    /// <summary>
    /// �^�b�v�ɂ��e�A�N�V����
    /// </summary>
    void TapAction()
    {
        //UI�̑��쒆�͏��������Ȃ�
        if (IsTappingUIArea()) return;

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
        if (groundTilemap.GetTile(ToolFrameTilePos()) == groundTiles[(int)TileIndexNum.Normal])
        {
            groundTilemap.SetTile(ToolFrameTilePos(), groundTiles[(int)TileIndexNum.Dry]);
        }      
    }

    /// <summary>
    /// �W���E���̃A�N�V����
    /// </summary>
    void JoroAcion()
    {
        //���������Ȃ�A���������̃^�C���ɕύX
        if (groundTilemap.GetTile(ToolFrameTilePos()) == groundTiles[(int)TileIndexNum.Dry])
        {
            groundTilemap.SetTile(ToolFrameTilePos(), groundTiles[(int)TileIndexNum.Wet]);
        }
    }

    /// <summary>
    /// ����g�p�}�X�̈ʒu���^�C���ʒu�w��`���ɕϊ�
    /// </summary>
    /// <returns></returns>
    Vector3Int ToolFrameTilePos()
    {
        int x = (int)(toolFrame.transform.position.x - 0.5f);
        int y = (int)(toolFrame.transform.position.y - 0.5f);

        return new Vector3Int(x, y, 0);
    }

    /// <summary>
    /// �^�b�v������
    /// �������_�ł̒��������^�b�v�ɊY�����邩�𔻒肷��
    /// </summary>
    /// <returns>�^�b�v=true</returns>
    bool IsTap()
    {
        //�^�b�v�@=�@���������Ԃ��^�b�v���莞�Ԉȓ��@���@�^�b�v�ʒu������Ă��Ȃ�
        return longTapTimer <= tapTime && tapPos == Input.mousePosition;
    }

    /// <summary>
    /// UI�X�y�[�X���^�b�v�i����j���Ă��邩
    /// </summary>
    /// <returns>UI�X�y�[�X�̎�true</returns>
    bool IsTappingUIArea()
    {
        //UI�͈́i�c���j��120px�Ƃ��Čv�Z
        return Input.mousePosition.y > Screen.height - 120 || Input.mousePosition.y < 120;
    }

}
