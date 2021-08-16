using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����̑�����UI�̐؂�ւ��Ȃǂ̊Ǘ�����
/// </summary>
public class ToolManager : MonoBehaviour
{
    PlayerCon playerCon;

    //�������̓���A�C�R����\������UI
    [SerializeField] GameObject nowToolIcon;
    Image nowToolIconImage;

    //����A�C�R���摜�iPlayerCon.ToolStatus���j
    [SerializeField] Sprite[] toolIconImages;

    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponent<PlayerCon>();
        nowToolIconImage = nowToolIcon.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ����A�C�R���^�b�v��
    /// </summary>
    /// <param name="iconNum">�^�b�v��������A�C�R���̔ԍ�</param>
    public void TapToolIcon(int iconNum)
    {
        //�������̓���A�C�R�����^�b�v���ꂽ��
        bool isNowTool = (int)playerCon.nowTool == iconNum;

        if (isNowTool)
        {
            //�������O��
            playerCon.nowTool = PlayerCon.ToolStatus.None;
        }
        else
        {
            //�^�b�v��������𑕔�����
            playerCon.nowTool = (PlayerCon.ToolStatus)iconNum;
        }
       
        //����g�p�͈͂̕\���؂�ւ�
        playerCon.toolFrame.SetActive(!isNowTool);

        //�������̓���A�C�R���؂�ւ�
        nowToolIcon.SetActive(!isNowTool);
        nowToolIconImage.sprite = toolIconImages[iconNum - 1];
    }
}
