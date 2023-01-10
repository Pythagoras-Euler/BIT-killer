using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] Text sendMsgText;
    [SerializeField] Room curroom;
    [SerializeField] UserInfo userInfo;
    [SerializeField] WebLink wl;
    [SerializeField] Text VillagerChatMsg;
    [SerializeField] Text WolfChatMsg;
    [SerializeField] PlayerAssignment playerAssignment;
    [SerializeField] GameControl gameState;
    public enum ChatChannel
    {
        WOLVES, ALL
    }
    public ChatChannel ChatChannelState;
    // Start is called before the first frame update
    void Start()
    {
        curroom = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();  
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        playerAssignment = GameObject.Find("PlayerAssignment").GetComponent<PlayerAssignment>();
    }

    // Update is called once per frame
    void Update()
    {
        RecChat();
    }
    public void RecChat() // 时刻获取聊天消息
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        Debug.Log(json);
        //string json = wl.receiveJson;
        JsonData recChatJson = JsonMapper.ToObject(json);
        if(recChatJson["type"].ToString()=="send message")
        {
            if(recChatJson["success"].ToString()=="True")
            {
                if(long.Parse(recChatJson["content"]["roomID"].ToString())==curroom.roomID)
                {
                    if (recChatJson["content"]["channel"].ToString() == "WOLVES") 
                    {
                        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
                        {
                            WolfChatMsg.text += recChatJson["content"]["username"].ToString() + "：" + recChatJson["content"]["message"].ToString() + "\n";
                        }
                    }
                    else
                    {
                        VillagerChatMsg.text += recChatJson["content"]["username"].ToString() + "：" + recChatJson["content"]["message"].ToString()+"\n";
                    }
                }
            }
            else
            {
                Debug.Log("接收消息失败");
            }
        }
    }
    public void SendBtn()
    {
        // 发送json
        string sendMsg = sendMsgText.text;
        JsonData data1 = new JsonData();
        data1["type"] = "send message";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = curroom.roomID;
        data1["content"]["username"] = userInfo.username;
        if(ChatChannelState == ChatChannel.WOLVES)
            data1["content"]["channel"] = "WOLVES";
        else
            data1["content"]["channel"] = "ALL";
        data1["content"]["message"] = sendMsg;
        string chatJson = data1.ToJson();
        Debug.Log(chatJson);
        wl.Send(chatJson);
    }
    public void recLastWord()
    {

    }
    public void SendLastWord()
    {

    }
    public void ChangeToAll()
    {
        ChatChannelState = ChatChannel.ALL;
        // TODO:按钮显示
    }
    public void ChangeToWolves()
    {
        ChatChannelState = ChatChannel.WOLVES;
    }
}
