using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public string roomName;
    public string creator;
    public int playerCount;
    public bool hasPassword;
    public bool gaming;
    public bool full;

    [SerializeField] GameObject roomNameField;
    [SerializeField] GameObject creatorField;
    [SerializeField] GameObject playerCountField;
    [SerializeField] GameObject hasPasswordHint;
    [SerializeField] GameObject gamingHint;
    [SerializeField] GameObject fullHint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        roomNameField.GetComponent<Text>().text = roomName;
        creatorField.GetComponent<Text>().text = creator;
        playerCountField.GetComponent<Text>().text = playerCount.ToString()+"/7";
        // TODO:������������벻ͬ��ɫ��ʾor��ͼ��
        if(hasPassword)
        {
            hasPasswordHint.GetComponent<Text>().text = "��Ҫ����";
        }
        // TODO:�Ƿ�������Ϸ��ʾ��ͬͼ��
        if (gaming)
        {
            gamingHint.GetComponent<Text>().text = "��Ϸ��";
        }
        // TODO:�Ƿ���ԱҲ���ò�ͬͼ��
        if (full)
        {
            fullHint.GetComponent<Text>().text = "����";
        }
    }
}
