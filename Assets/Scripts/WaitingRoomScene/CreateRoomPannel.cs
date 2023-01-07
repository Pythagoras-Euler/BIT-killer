using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class CreateRoomPannel : MonoBehaviour
{
    [SerializeField] GameObject CreateRoomNameField;
    [SerializeField] GameObject HasPasswordBtn;
    [SerializeField] GameObject SetPasswordField;
    [SerializeField] GameObject CreateRoomPassword;
    [SerializeField] WebLink wl;

    private string roomName;
    private string creator;      
    public bool hasPassword=false;
    private string password;

    private void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
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

        // TODO:处理返回消息
        // 依然有BOM的问题，这里先放一个jsontest.txt做测试
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retcreatearoom = JsonMapper.ToObject(json);
        Debug.Log(retcreatearoom["content"]["creator"].ToString());
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
