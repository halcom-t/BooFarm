using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] GameObject itemIconPre;

    //アイテムのアイコンUIオブジェクト群
    GameObject[] itemIcons = new GameObject[8];

    private void Awake()
    {
        //アイテムのアイコンUIを作成
        for (int i = 0; i < itemIcons.Length; i++)
        {
            itemIcons[i] = Instantiate(itemIconPre, new Vector3(-110 + i * 120, 0, 0), Quaternion.identity);
            itemIcons[i].transform.SetParent(this.transform, false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
