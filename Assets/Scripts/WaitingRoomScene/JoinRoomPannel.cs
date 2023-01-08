using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPannel : MonoBehaviour
{
    public string username;
    public GameObject roomPasswordField;
    public GameObject roomPasswordText;
    public GameObject joinBtn;
    public Text roomInfoDisplay;
    public Text retMsg;
    public WebLink wl;
    public int roomID;
    public bool hasPassword;
    public bool canJoin;
    public string roomOwner;
    public string[] roomMembers;
    public int memberCount;


    private void Start()
    {
        username = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>().username;
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        
    }
    private void Update()
    {

        RoomInfoDisplay();
        if (!canJoin)
        {
            joinBtn.SetActive(false);
            roomPasswordField.SetActive(false);
        }
        else 
        {
            if (!hasPassword)
            {
                roomPasswordField.SetActive(false);
            }
            else
            {
                roomPasswordField.SetActive(true);
            }
            joinBtn.SetActive(true);
        }
    }

    private void RoomInfoDisplay()
    {
        roomMembers = new string[] { "lbw", "lx", "lcy", "lgy" };
        roomInfoDisplay.text = "����ţ�"+ roomID + " \n ������"+roomOwner + " \n ��Ա:" + roomMembers + " \n ������"+ memberCount + "/7 \n ";
        //TODO ��ʽ�е�����,string[]��Ҫ������ʾ��ʽ
    }
    public void JoinBtn()
    {
        // ���ͼ�������
        string roompassword;
        if (hasPassword)
            roompassword = roomPasswordText.GetComponent<Text>().text;
        else
            roompassword = "";
        JsonData data1 = new JsonData();
        data1["type"] = "join room";
        data1["content"] = new JsonData();
        data1["content"]["username"] = username;
        data1["content"]["roomID"] = roomID;
        data1["content"]["password"] = roompassword;
        string jrJson = data1.ToJson();
        Debug.Log(jrJson);
        wl.Send(jrJson);

        // ��������Ϣ
        // ��Ȼ��BOM�����⣬�����ȷ�һ��jsontest.txt������
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retjoinaroom = JsonMapper.ToObject(json);
        Debug.Log(retjoinaroom["success"]);
        if (retjoinaroom["success"].ToString() == "True")
        {
            //TODO:�л�����
        }
        else
        {
            //TODO:��ʾ����ʧ����Ϣ
            retMsg.text = "���뷿��ʧ�ܣ�������";
        }
        
    }
    public void CloseBtn()
    {
        this.gameObject.SetActive(false);
    }
}
