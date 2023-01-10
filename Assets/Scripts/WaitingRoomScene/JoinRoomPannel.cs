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
    public string[] roomMembers;
    public int memberCount;

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
    }

    private void RoomInfoDisplay()
    {
        string rM = roomMembers[0];
        for(int i =1;i<roomMembers.Length;i++)
        {
            rM = rM + "," + roomMembers[i];
        }
        roomInfoDisplay.text = "房间号："+ roomID + " \n 房主："+roomOwner + " \n 成员:" + rM + " \n 人数："+ memberCount + "/7 \n ";
        //TODO 格式有点问题,string[]需要更换显示方式
        if (canJoin)
            retMsg.text = "该房间可加入";
    }
    public void JoinBtn()
    {
        // 发送加入请求
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
        //wl.Send(jrJson);

        // 处理返回消息
        // 依然有BOM的问题，这里先放一个jsontest.txt做测试
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retjoinaroom = JsonMapper.ToObject(json);
        Debug.Log(retjoinaroom["success"]);
        if (retjoinaroom["success"].ToString() == "True")
        {
            // 保存房间信息
            Room curroom = roomInfo.GetComponent<Room>();
            curroom.roomID = roomID;
            curroom.roomName = roomName;
            curroom.creator = roomOwner;
            curroom.iscurcreator = roomOwner == username ? true : false;
            curroom.playerCount = memberCount+1; // 加入当前玩家
            // TODO:继续将玩家填入到玩家数组里面，完善curroom
            curroom.players = new string[memberCount+1];
            for(int i=0;i<memberCount;i++)
            {
                curroom.players[i] = roomMembers[i];
            }
            curroom.players[memberCount] = username;
            curroom.password = roompassword;
            curroom.full = curroom.playerCount == 7 ? true : false;
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
            retMsg.text = retjoinaroom["message"].ToString();
            
        }
        
    }
    public void CloseBtn()
    {
        this.gameObject.SetActive(false);
    }
}
