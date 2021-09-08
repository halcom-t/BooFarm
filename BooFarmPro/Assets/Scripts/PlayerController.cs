using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �v���C���[�̊�{����
/// </summary>
public class PlayerController : MonoBehaviour
{
    //����I�u�W�F�N�g�֘A=======================================
    //����ł���I�u�W�F�N�g�̃^�O
    enum OperationObjTags
    {
        None,
        Home,   //UFO
        Bed,    //�x�b�h
        Eixt    //UFO�̏o��
    }
    //���ݐڐG���̃A�N�V�����I�u�W�F�N�g�i�^�O�j
    string nowOperationObjTag = "";

    //����֘A====================================================
    //�^�b�v�����ʒu
    Vector3 tapPos;
    //���������Ԍv���p�^�C�}�[
    float longTapTimer;
    //�^�b�v���莞��
    const float tapTime = 0.15f;

    //�R���|�[�l���g==============================================
    Rigidbody2D rigidbody;
    ToolController toolCon;
    GameManager gameManager;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        toolCon = GetComponent<ToolController>();
        //transform.GetChild(0) = �q�v�f�i�J�����j���擾�z��
        gameManager = transform.GetChild(0).gameObject.GetComponent<GameManager>();
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
    /// �������ɏ��������̓T�u�N���X�iPlayerToolController�j�ɂ��e�����o�邩�璍��
    /// </summary>
    /// <returns>�T�u�N���X��direction�i�ړ��x�N�g���j���擾����p�̖߂�l</returns>
    void Move()
    {
        //�^�b�v�Ȃ�ړ����������Ȃ�
        if (IsTap()) return;

        //UI�G���A�Ȃ�ړ����������Ȃ�
        if (IsTappingUIArea())
        {
            //�ړ����x�����Z�b�g
            rigidbody.velocity = new Vector2(0, 0);
            return;
        }

        //�X���C�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //�ړ�
        Vector2 direction = new Vector2(swipePosX, swipePosY).normalized;
        rigidbody.velocity = direction * 3;

        //�ړ����A����̎g�p�ʒu�i�}�X�j���X�V
        if (toolCon != null) toolCon.UpdateToolFrame(direction);
        
    }

    /// <summary>
    /// �^�b�v�ɂ��A�N�V����
    /// </summary>
    void TapAction()
    {
        //UI�G���A�Ȃ珈�������Ȃ�
        if (IsTappingUIArea()) return;

        //�ڐG���̑���I�u�W�F�N�g�A�N�V�����i����A�N�V�������D��j
        if (nowOperationObjTag != "")
        {
            OperationObjAcion();
            return;
        }
        
        //����A�N�V����
        if (toolCon != null) toolCon.ToolAction();
    }

    /// <summary>
    /// ����I�u�W�F�N�g�̃A�N�V����
    /// </summary>
    void OperationObjAcion()
    {
        //UFO(���_)
        if (nowOperationObjTag == OperationObjTags.Home.ToString())
        {
            //�f�[�^���Z�[�u�i�^�C�����X�V�j
            gameManager.Save(toolCon.GetTileData());
            //UFO�ɓ���iHomeScene�̃��[�h�j
            SceneManager.LoadScene("Home");
        }
        //�x�b�h
        else if (nowOperationObjTag == OperationObjTags.Bed.ToString())
        {
            //�f�[�^�𗂓��i��6: 00�j�ɂ��ăZ�[�u
            gameManager.SaveNextDayData();
        }
        //���_�o��
        else if (nowOperationObjTag == OperationObjTags.Eixt.ToString())
        {
            //�_���ɖ߂�
            SceneManager.LoadScene("Main");
        }
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
        //�ڐG���̃I�u�W�F�N�g�̃^�O�ۑ�
        nowOperationObjTag = collision.gameObject.tag;
    }

    /// <summary>
    /// �I�u�W�F�N�g���痣�ꂽ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�ڐG���̃I�u�W�F�N�g�̃^�O���Z�b�g
        if (nowOperationObjTag == collision.gameObject.tag)
        {
            nowOperationObjTag = "";
        }
    }

}
