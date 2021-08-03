using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �v���C���[�֘A
/// </summary>
public class PlayerCon : MonoBehaviour
{
    //�����i���݌����Ă�������̓A�j���[�V�����p�����[�^�uDirection�v����擾�j
    enum DirectionType
    {
        front,  //�O�i���ʁj
        back,   //��
        left,   //��
        right   //�E
    }

    //����
    enum ToolStatus{
        None, 
        Kuwa    //�N��
    }
    //�������̓��-------------------------------------�f�o�b�O��None�ɂ���
    ToolStatus nowTool = ToolStatus.Kuwa;

    //����̎g�p�͈�
    [SerializeField] GameObject toolFrame;

   //�n�ʂ̊e�^�C����-----------------------------------�摜�����ւ��K�v�iindex�����邩���j
   [SerializeField] TileBase[] groundTiles;
    //�n�ʂ̃^�C���}�b�v
    [SerializeField] GameObject groundObj;
    //�n�ʂ̃^�C���}�b�v���
    Tilemap groundTilemap;

    //�v���C���[�̃A�j���[�^�[
    Animator anim;

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
        //�A�j���[�^�[
        anim = GetComponent<Animator>();
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
            //����̎g�p�͈͂̌v�Z
            ToolFramePos();
        }
    }

    /// <summary>
    /// ����̎g�p�͈͂̌v�Z
    /// </summary>
    void ToolFramePos()
    {
        float x = Mathf.Floor(transform.position.x);
        float y = Mathf.Floor(transform.position.y);
        float z = 0f;

        //�e�����������Ă���Ƃ��̓���g�p�͈͂̈ʒu���v�Z��-----------------------------------�R�[�h�Z��������
        if (anim.GetInteger("Direction") == (int)DirectionType.front)
        {
            x += 0.5f;
            y -= 0.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.back)
        {
            x += 0.5f;
            y += 1.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.left)
        {
            x -= 0.5f;
            y += 0.5f;
        }
        else if (anim.GetInteger("Direction") == (int)DirectionType.right)
        {
            x += 1.5f;
            y += 0.5f;
        }

        //����g�p�͈͂̈ʒu��ݒ�
        Vector3 toolPos = new Vector3(x, y, z);
        toolFrame.transform.position = toolPos;
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    void Move()
    {
        //�^�b�v�Ȃ�ړ����������Ȃ�
        if (IsTap()) return;

        //�X���C�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //�ړ�
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        transform.Translate(direction * Time.deltaTime * 2);

        //���݂̕������A�j���[�V�����p�����[�^�uDirection�v�ɐݒ�
        anim.SetInteger("Direction", (int)IsDirection(direction));
    }

    /// <summary>
    /// �����Ă�������𔻒�
    /// </summary>
    /// <param name="nowDirection">��������p�̃x�N�g��</param>
    /// <returns>���݌����Ă������</returns>
    DirectionType IsDirection(Vector2 directionVector)
    {
        //�x�N�g������ǂ̕������������邩�i4�����j���聃-----------------------------------�����̏����O�p�֐��g������
        if (Mathf.Abs(directionVector.x) <= Mathf.Abs(directionVector.y))
        {
            if (directionVector.y <= 0) return DirectionType.front;
            else return DirectionType.back;
        }
        else
        {
            if (directionVector.x <= 0) return DirectionType.left;
            else return DirectionType.right;
        }
    }

    /// <summary>
    /// �^�b�v�ɂ��e�A�N�V����
    /// </summary>
    void TapAction()
    {
        //�������̊e�����
        switch (nowTool)
        {
            case ToolStatus.None:   //�����Ȃ�
                break;
            case ToolStatus.Kuwa:   //�N��
                KuwaAction();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �N���������̃A�N�V����
    /// </summary>
    void KuwaAction()
    {
        //groundTilemap.SetTile(toolPos, groundTiles[1]);  
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

}
