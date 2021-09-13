using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����̑�����UI�̐؂�ւ��Ȃǂ̊Ǘ�����
/// </summary>
public class UIManager : MonoBehaviour
{
    //�R���|�[�l���g==============================================
    //�v���C���[�̓�����R���|�[�l���g
    [SerializeField] GameObject playerObj;
    ToolController toolCon;

    //�������̓���A�C�R����\������UI��image
    [SerializeField] GameObject nowToolIcon;
    Image nowToolIconImage;

    //UI�֘A=====================================================
    //����A�C�R���摜�iPlayerCon.ToolStatus���j
    [SerializeField] Sprite[] toolIconImages;



    // Start is called before the first frame update
    void Start()
    {
        toolCon = playerObj.gameObject.GetComponent<ToolController>();
        nowToolIconImage = nowToolIcon.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ����A�C�R���^�b�v��
    /// </summary>
    /// <param name="iconNum">�^�b�v��������A�C�R���̔ԍ�(ToolController.ToolStatus�)</param>
    public void TapToolIcon(int iconNum)
    {
        if (toolCon == null) return;

        //�������̓���A�C�R�����^�b�v���ꂽ��
        bool isNowTool = (int)toolCon.nowTool == iconNum;

        if (isNowTool)
        {
            //�������O��
            toolCon.nowTool = ToolController.ToolStatus.None;
        }
        else
        {
            //�^�b�v��������𑕔�����
            toolCon.nowTool = (ToolController.ToolStatus)iconNum;
        }

        //����g�p�͈͂̕\���؂�ւ�
        toolCon.toolFrame.SetActive(!isNowTool);

        //�������̓���A�C�R���؂�ւ�
        nowToolIcon.SetActive(!isNowTool);
        nowToolIconImage.sprite = toolIconImages[iconNum - 1];
    }
}
