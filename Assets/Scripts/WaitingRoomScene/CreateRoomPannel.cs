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
        Debug.Log("创建房间");
        // 发送json
        CreateARoom crar = new CreateARoom("create room", creator, password, roomName);
        string crarJson = JsonMapper.ToJson(crar);
        Debug.Log(crarJson);
        wl.Send(crarJson);

        
    }
    private void Update()
    {
        // 处理返回消息
        Debug.Log(wl.receiveJson);
        //string json = wl.receiveJson;
        JsonData retcreatearoom = JsonMapper.ToObject(wl.receiveJson);
        if (retcreatearoom["type"].ToString() == "create room") // 验证消息类型
        {
            if (retcreatearoom["success"].ToString() == "True")
            {
                // 保存房间信息
                Room curroom = roomInfo.GetComponent<Room>();
                curroom.roomID = long.Parse(retcreatearoom["content"]["roomID"].ToString());
                curroom.roomName = retcreatearoom["content"]["roomName"].ToString();
                curroom.creator = retcreatearoom["content"]["creator"].ToString();
                curroom.iscurcreator = true;
                curroom.playerCount = 1; // 加入当前玩家
                curroom.players = new string[1];
                curroom.players[0] = username;
                curroom.password = retcreatearoom["content"]["password"].ToString();
                curroom.full = false;
                curroom.gaming = false;
                // 把roominfo放进Dontdestroy里
                roomInfo.transform.parent = GameObject.FindGameObjectWithTag("DontDestroy").transform;
                // 切换场景
                Debug.Log("加入房间成功，即将切换场景");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                //TODO:显示加入失败信息
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
