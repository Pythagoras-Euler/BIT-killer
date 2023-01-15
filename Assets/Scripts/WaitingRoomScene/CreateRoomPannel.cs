using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class CreateRoomPannel : MonoBehaviour
{
    [SerializeField] GameObject CreateRoomNameField;
    [SerializeField] GameObject HasPasswordBtn;
    [SerializeField] GameObject SetPasswordField;
    [SerializeField] GameObject CreateRoomPassword;
    [SerializeField] Text retMsg;
    [SerializeField] WebLink wl;
    [SerializeField] GameObject roomInfo;


    public string username;
    private string roomName;
    private string creator;      
    public bool hasPassword=false;
    private string password;

    private void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        roomInfo = GameObject.FindGameObjectWithTag("RoomInfo");
        username = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>().username;
    }
    public void CreateRoom()
    {
        roomName = CreateRoomNameField.GetComponent<Text>().text.ToString();
        creator = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>().username;
        if(hasPassword)
        {
            password = SetPasswordField.GetComponent<Text>().text.ToString();
        }
        else
        {
            password = "";
        }
        Debug.Log("��������");
        // ����json
        CreateARoom crar = new CreateARoom("create room", creator, password, roomName);
        string crarJson = JsonMapper.ToJson(crar);
        Debug.Log(crarJson);
        wl.Send(crarJson);

        
    }
    private void Update()
    {
        // ��������Ϣ
        Debug.Log(wl.receiveJson);
        //string json = wl.receiveJson;
        JsonData retcreatearoom = JsonMapper.ToObject(wl.receiveJson);
        if (retcreatearoom["type"].ToString() == "create room") // ��֤��Ϣ����
        {
            if (retcreatearoom["success"].ToString() == "True")
            {
                // ���淿����Ϣ
                Room curroom = roomInfo.GetComponent<Room>();
                roomInfo.GetComponent<Room>().roomID = long.Parse(retcreatearoom["content"]["roomID"].ToString());
                roomInfo.GetComponent<Room>().roomName = retcreatearoom["content"]["roomName"].ToString();
                roomInfo.GetComponent<Room>().creator = retcreatearoom["content"]["creator"].ToString();
                roomInfo.GetComponent<Room>().iscurcreator = true;
                roomInfo.GetComponent<Room>().playerCount = 1; // ���뵱ǰ���
                roomInfo.GetComponent<Room>().players = new string[7];
                roomInfo.GetComponent<Room>().players[0] = username;
                roomInfo.GetComponent<Room>().password = retcreatearoom["content"]["password"].ToString();
                roomInfo.GetComponent<Room>().full = false;
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
                retMsg.text = retcreatearoom["message"].ToString();

            }
        }
        else
        {
            Debug.Log(retcreatearoom["type"].ToString());
        }
    }
    public void ClosePannel()
    {
        this.gameObject.SetActive(false);
    }
    public void HasPassword()
    {
        hasPassword = !hasPassword;
        if(hasPassword)
        {
            CreateRoomPassword.SetActive(true);
        }
        else
        {
            CreateRoomPassword.SetActive(false);
        }
    }
}
