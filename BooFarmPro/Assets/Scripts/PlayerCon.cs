using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

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
        Joro,   //�W���E��
        Tane    //�^�l
    }
    //�������̓���
    [System.NonSerialized] public ToolStatus nowTool = ToolStatus.None;
    //����̎g�p�͈�
    public GameObject toolFrame;

    //�n�ʃ^�C���֘A===============================================
    //�n�ʂ̃^�C���F���
    enum TileIndexNum
    {
        Normal, //�����i�f�t�H�j
        Dry,    //��������
        Wet     //��������
    }

    //�n�ʂ̊e�^�C���̎��
    [SerializeField] TileBase[] groundTiles;
    //�n�ʂ̃^�C���}�b�v(obj)
    [SerializeField] GameObject groundObj;
    //�n�ʂ̃^�C���}�b�v���
    Tilemap groundTilemap;


    //�앨�֘A====================================================
    //�앨�v���t�@�u
    [SerializeField] GameObject cropPre;

    //�앨�̏��
    enum CropStatus
    {
        None,
        Seed    //��
    }
    //���͈̔�
    const int FarmAreaW = 40;
    const int FarmAreaH = 30;
    //�^�C�����̍앨�̏��
    CropStatus[,] tileCropStatus = new CropStatus[FarmAreaW, FarmAreaH];

    //����֘A====================================================
    //�^�b�v�����ʒu
    Vector3 tapPos;
    //���������Ԍv���p�^�C�}�[
    float longTapTimer;
    //�^�b�v���莞��
    const float tapTime = 0.15f;

    Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
            //�ړ����x�����Z�b�g
            rigidbody.velocity = new Vector2(0, 0);
            //�^�b�v�Ȃ�A�N�V�������s
            if (IsTap()) TapAction();
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
        if (IsTappingUIArea()) return;

        //�X���C�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //�ړ�
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        rigidbody.velocity = direction * 3;

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
        if (tileCropStatus[ToolFramePosInt().x, ToolFramePosInt().y] != CropStatus.None) return;

        //��܂�����
        {
            //�앨obj�̐���
            Instantiate(cropPre, ToolFramePos(), Quaternion.identity);
            //�앨�̏�Ԃ����ԂɕύX
            tileCropStatus[ToolFramePosInt().x, ToolFramePosInt().y] = CropStatus.Seed;
        }
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

    /// <summary>
    /// �I�u�W�F�N�g�ɐڐG������
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //UFO(���_)�ɐڐG������
        if (collision.gameObject.tag == "Home")
        {
            //UFO�ɓ���iHomeScene�̃��[�h�j
            SceneManager.LoadScene("Home");
        }
    }

}
