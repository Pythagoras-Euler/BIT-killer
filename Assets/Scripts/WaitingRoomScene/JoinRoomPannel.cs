using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinRoomPannel : MonoBehaviour
{
    public string username;
    public GameObject roomPasswordField;
    public GameObject roomPasswordText;
    public GameObject joinBtn;
    public Text roomInfoDisplay;
    public Text retMsg;
    public WebLink wl;
    public long roomID;
    public string roomName;
    public bool hasPassword;
    public bool canJoin;
    public string roomOwner;
    public string[] roomMembers=new string[7];
    public int memberCount;
    [SerializeField] GameObject joinRoomPannel;
    public GameObject roomInfo;


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
        JsonData retjoinaroom = JsonMapper.ToObject(wl.receiveJson);
        if (retjoinaroom["type"].ToString() == "join room") // ��֤��Ϣ����
        {

            Debug.Log(retjoinaroom["success"]);
            if (retjoinaroom["success"].ToString() == "True")
            {
                // ���淿����Ϣ
                roomID =long.Parse(retjoinaroom["content"]["roomID"].ToString());
                roomInfo.GetComponent<Room>().roomID = roomID;
                roomName = retjoinaroom["content"]["roomName"].ToString();
                roomInfo.GetComponent<Room>().roomName = roomName;
                roomOwner = retjoinaroom["content"]["creator"].ToString();
                roomInfo.GetComponent<Room>().creator = roomOwner;
                roomInfo.GetComponent<Room>().iscurcreator = roomOwner == username ? true : false;
                memberCount = int.Parse(retjoinaroom["content"]["playerCount"].ToString());
                roomInfo.GetComponent<Room>().playerCount = memberCount + 1; // ���뵱ǰ���
                roomInfo.GetComponent<Room>().players = new string[memberCount + 1];
                roomMembers = new string[memberCount + 1];
                for (int i = 0; i < memberCount; i++)
                {
                    roomMembers[i] = retjoinaroom["content"]["players"][i].ToString();
                    roomInfo.GetComponent<Room>().players[i] = roomMembers[i];
                }
                roomInfo.GetComponent<Room>().players[memberCount] = username;
                roomInfo.GetComponent<Room>().password = roompassword;
                roomInfo.GetComponent<Room>().full = roomInfo.GetComponent<Room>().playerCount == 7 ? true : false;
                roomInfo.GetComponent<Room>().gaming = false;
                // ��roominfo�Ž�Dontdestroy��
                roomInfo.transform.parent = GameObject.FindGameObjectWithTag("DontDestroy").transform;
                // �л�����
                Debug.Log("���뷿��ɹ��������л�����");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                //TODO:��ʾ����ʧ����Ϣ
                retMsg.text = retjoinaroom["message"].ToString();

            }
        }

        JsonData retmsg = JsonMapper.ToObject(wl.receiveJson);
        if (retmsg["type"].ToString() == "get a room") // ��֤��Ϣ����
        {
            Debug.Log("get a room");            
            bool retSuc = retmsg["success"].ToString() == "True" ? true : false;

            if (retSuc == true)
            {
                roomID = long.Parse(retmsg["content"]["roomID"].ToString());
                roomName = retmsg["content"]["roomName"].ToString();
                hasPassword = retmsg["content"]["password"].ToString() == ""?false:true;
                roomOwner = retmsg["content"]["creator"].ToString();
                memberCount = int.Parse(retmsg["content"]["playerCount"].ToString());
                roomMembers = new string[memberCount];
                for (int i = 0; i < memberCount; i++)
                {
                    roomMembers[i] = retmsg["content"]["players"][i].ToString();
                }
                canJoin = true;
            }
        }
    }

    private void RoomInfoDisplay()
    {
        string rM = "";
        for(int i = 0;i<roomMembers.Length;i++)
        {
            rM = rM + "," + roomMembers[i];
        }
        roomInfoDisplay.text = "����ţ�"+ roomID + " \n ������"+roomOwner + " \n ��Ա:" + rM + " \n ������"+ memberCount + "/7 \n ";
        //TODO ��ʽ�е�����,string[]��Ҫ������ʾ��ʽ
        if (canJoin)
            retMsg.text = "�÷���ɼ���";
    }

    string roompassword;
    public void JoinBtn()
    {
        // ���ͼ�������
        if (hasPassword)
            roompassword = roomPasswordText.GetComponent<Text>().text;
        else
            roompassword = "";
        JsonData data1 = new JsonData();
        data1["type"] = "join room";
        data1["content"] = new JsonData();
        data1["content"]["user"] = username;
        data1["content"]["roomID"] = roomID;
        data1["content"]["password"] = roompassword;
        string jrJson = data1.ToJson();
        Debug.Log(jrJson);
        wl.Send(jrJson);

        
        
    }
    public void CloseBtn()
    {
        joinRoomPannel.SetActive(false);
        Debug.Log("joinroompanel False");
    }
}
