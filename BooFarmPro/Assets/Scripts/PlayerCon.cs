using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�֘A
/// </summary>
public class PlayerCon : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        //�X���C�v��
        if (Input.GetMouseButton(0))
        {
            //�^�b�v���Ă���ʒu���擾�i��ʒ��� 0,0�j
            float tapPosX = Input.mousePosition.x - Screen.width / 2;
            float tapPosY = Input.mousePosition.y - Screen.height / 2;

            //�ړ�
            transform.Translate(new Vector2(tapPosX, tapPosY).normalized * Time.deltaTime * 2);
            
        }
    }
}
