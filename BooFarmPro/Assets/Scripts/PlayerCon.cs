using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�֘A
/// </summary>
public class PlayerCon : MonoBehaviour
{

    //����
    enum ToolStatus{
        None, 
        Kuwa    //�N��
    }

    //�^�b�v�����ʒu
    Vector3 tapPos;

    //���������Ԍv���p�^�C�}�[
    float longTapTimer;

    //�^�b�v���莞��
    const float tapTime = 0.15f;

    void Start()
    {
        
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
            if (isTap())TapAction();
            
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
        if (isTap()) return;

        //�X���C�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
        float swipePosX = Input.mousePosition.x - Screen.width / 2;
        float swipePosY = Input.mousePosition.y - Screen.height / 2;

        //�ړ�
        transform.Translate(new Vector2(swipePosX, swipePosY).normalized * Time.deltaTime * 2);
    }

    /// <summary>
    /// �^�b�v�ɂ��e�A�N�V����
    /// </summary>
    void TapAction()
    {

    }

    /// <summary>
    /// �^�b�v������
    /// �������_�ł̒��������^�b�v�ɊY�����邩�𔻒肷��
    /// </summary>
    /// <returns>�^�b�v=true</returns>
    bool isTap()
    {
        //�^�b�v�@=�@���������Ԃ��^�b�v���莞�Ԉȓ��@���@�^�b�v�ʒu������Ă��Ȃ�
        return longTapTimer <= tapTime && tapPos == Input.mousePosition;
    }

}
