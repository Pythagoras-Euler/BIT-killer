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
    [SerializeField] GameControl gameControl;
    [SerializeField] GameObject VillagerScrollView;
    [SerializeField] GameObject WolfScrollView;
    public GameObject TeamBtn;
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
        playerAssignment = GameObject.Find("/PlayerAssignment").GetComponent<PlayerAssignment>();
        gameControl = GameObject.Find("/GameControl").GetComponent<GameControl>();
        if(playerAssignment.playerCharacter != PlayerAssignment.Character.WOLF)
        {
            TeamBtn.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameControl.gameState == GameControl.GameState.DISCUSS) // 只有在讨论阶段允许接收
        {
            RecChat();
        }
        else if (gameControl.gameState == GameControl.GameState.WORDS) // 只有在遗言阶段允许接收遗言
        {
            recLastWord();
        }
    }
    public void RecChat() // 时刻获取聊天消息
    {
       
        string json = wl.receiveJson;
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
                        VillagerChatMsg.text += "（遗言）"+ recChatJson["content"]["username"].ToString() + "：" + recChatJson["content"]["message"].ToString()+"\n";
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
        if(gameControl.gameState == GameControl.GameState.DISCUSS) // 只有讨论阶段允许发送
        {
            // 发送json
            string sendMsg = sendMsgText.text;
            JsonData data1 = new JsonData();
            data1["type"] = "send message";
            data1["content"] = new JsonData();
            data1["content"]["roomID"] = curroom.roomID;
            data1["content"]["username"] = userInfo.username;
            if (ChatChannelState == ChatChannel.WOLVES)
                data1["content"]["channel"] = "WOLVES";
            else
                data1["content"]["channel"] = "ALL";
            data1["content"]["message"] = sendMsg;
            string chatJson = data1.ToJson();
            Debug.Log(chatJson);
            wl.Send(chatJson);
        }
        
    }
    public void recLastWord()
    {
        ;
        string json = wl.receiveJson;
        JsonData recLastWordJson = JsonMapper.ToObject(json);
        if (recLastWordJson["type"].ToString() == "last words")
        {
            if (recLastWordJson["success"].ToString() == "True")
            {
                if (long.Parse(recLastWordJson["content"]["roomID"].ToString()) == curroom.roomID)
                {
                        VillagerChatMsg.text += recLastWordJson["content"]["username"].ToString() + "：" + recLastWordJson["content"]["message"].ToString() + "\n";
                    
                }
            }
            else
            {
                Debug.Log("接收消息失败");
            }
        }
    }
    public void SendLastWord()
    {
        if (gameControl.gameState == GameControl.GameState.WORDS)//只有在遗言阶段可以发表遗言
        {
            if (gameControl.dayEvent.killed == playerAssignment.playerName) // 当被刀的人是该客户端玩家
            {
                // 发送json
                string lastwordsMsg = sendMsgText.text;
                JsonData data1 = new JsonData();
                data1["type"] = "last words";
                data1["content"] = new JsonData();
                data1["content"]["roomID"] = curroom.roomID;
                data1["content"]["username"] = userInfo.username;
                data1["content"]["channel"] = "ALL";
                data1["content"]["message"] = lastwordsMsg;
                string chatJson = data1.ToJson();
                Debug.Log(chatJson);
                wl.Send(chatJson);
            }
        }
    }
    public void ChangeToAll()
    {
        ChatChannelState = ChatChannel.ALL;
        // 不同显示
        VillagerScrollView.SetActive(true);
        WolfScrollView.SetActive(false);
    }
    public void ChangeToWolves()
    {
        ChatChannelState = ChatChannel.WOLVES;
        WolfScrollView.SetActive(true);
        VillagerScrollView.SetActive(false);

    }
}
