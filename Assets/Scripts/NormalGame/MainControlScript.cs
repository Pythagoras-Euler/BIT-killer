using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainControlScript : MonoBehaviour
{

    int PoliceId;
    [SerializeField] GameControl gameControl;
    public GameObject multyBtn; // 只有房主有，开启游戏按钮
    public GameObject doubtPan;
    public GameObject electPan;
    public GameObject statePan;
    public GameObject villagerPan;
    public GameObject wolfPan;
    public GameObject witchPan;
    public GameObject seerPan;
    public GameObject votePan;
    public GameObject idenIcon;
    [SerializeField] UserInfo userInfo;
    [SerializeField] PlayerAssignment playerAssignment;
    private int minWaitTime;
    private int maxWaitTime;
    private int maxPlayerNum;

    [SerializeField] WebLink wl;
    [SerializeField] Room room;
    [SerializeField] Text WolfChatMsg;
    [SerializeField] Text VillagerChatMsg;

    bool clare = false; // 今天有没有发过死讯

    // Start is called before the first frame update
    void Start()
    {
        PoliceId = -1;
        minWaitTime = 0;//TODO 一个合适的值，取决于计时器
        maxWaitTime = 9999;//同上
        multyBtn.SetActive(false);

        playerAssignment = GameObject.Find("/PlayerAssignment").GetComponent<PlayerAssignment>();
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
    }

    // Update is called once per frame
    void Update()//阶段更新收发放到GameControl里
    {
        HasVictory(); // 检查游戏是否有赢家
        switch (gameControl.gameState)//判断现在的阶段并执行相应函数
        {
            case GameControl.GameState.WAIT:
                Waiting();
                break;
            case GameControl.GameState.START://分发身份牌
                StartGame();
                break;
            case GameControl.GameState.KILL: // 狼人行动
                RetWolfAct();
                break;
            case GameControl.GameState.WITCH:
                RetWitchAct();
                break;
            case GameControl.GameState.PROPHET: // 预言家行动
                RetProphetAct();
                break;
            case GameControl.GameState.DISCUSS:
                DiscussAct();
                break;
            case GameControl.GameState.ELECT:
                ElectPolice();
                break;
            case GameControl.GameState.VOTE:
                VoteKill();
                break;
            case GameControl.GameState.WORDS:
                LastWords();
                break;
            case GameControl.GameState.END://结算，宣布胜者
                
                break;
        }
    }

    


    //等待页面，将所有icon设置为不可用
    void Waiting()
    {
        SetDay();//TODO

        // 处理返回消息
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        Debug.Log(json);
        //string json = wl.receiveJson;
        JsonData retnewjoinroom = JsonMapper.ToObject(json);
        if (retnewjoinroom["type"].ToString() == "join room") // 验证消息类型，加入房间
        {
            if(retnewjoinroom["success"].ToString()=="True"&&long.Parse(retnewjoinroom["content"]["roomID"].ToString())==room.roomID)// 成功加入当前房间
            {
                int playerCount = retnewjoinroom["content"]["players"].Count;
                gameControl.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // 更新玩家表
            }
        }
        else if (retnewjoinroom["type"].ToString() == "leave room") // 验证消息类型，退出房间
        {
            if (retnewjoinroom["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)// 成功离开当前房间
            {
                int playerCount = int.Parse(retnewjoinroom["content"]["playerCount"].ToString());
                gameControl.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // 更新玩家表
            }
        }

        bool everyoneReady = false;
        if (gameControl.players.Length == 7)
        { everyoneReady = true; }

        if (everyoneReady)//所有人准备
        {
            multyBtn.SetActive(true);

            // 处理服务器发来可以开始游戏返回值
            sr = new StreamReader(Application.dataPath + "/jsontest.txt");
            json = sr.ReadToEnd().TrimEnd('\0');
            Debug.Log(json);
            //string json = wl.receiveJson;
            JsonData retStartGame = JsonMapper.ToObject(json);
            if (retStartGame["type"].ToString() == "game start") // 验证消息类型，开始游戏
            {
                if (retStartGame["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)
                {
                    int playerCount = int.Parse(retnewjoinroom["content"]["playerCount"].ToString());
                    gameControl.players = new string[playerCount];
                    for (int i = 0; i < playerCount; i++) {
                        gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // 更新玩家表
                        gameControl.playerCharacterMap[retnewjoinroom["content"]["players"][i].ToString()] = retnewjoinroom["content"]["playerCharacterMap"][retnewjoinroom["content"]["players"][i].ToString()].ToString();
                        gameControl.playerStateMap[retnewjoinroom["content"]["players"][i].ToString()] = retnewjoinroom["content"]["playerStateMap"][retnewjoinroom["content"]["players"][i].ToString()].ToString()=="True"?true:false;

                    }
                    gameControl.gameState = GameControl.GameState.START;// 更新游戏状态
                }
            }

        }
    }

    public void onClickMultyBtn()
    {
        // 向服务器发送开始游戏请求
        // 发送json
        JsonData startJson = new JsonData();
        startJson["type"] = "game start";
        startJson["content"] = new JsonData();
        startJson["content"]["roomID"] = room.roomID;
        string startJsonStr = startJson.ToJson();
        wl.Send(startJsonStr);

    }

    //分发身份牌,可以弹出一个窗口，也可以在聊天栏有一个系统消息
    void StartGame()
    {
        // 
        SetNight();
    }

    //
    void SetNight()
    {
        //背景由白天变成黑夜
    }

    void SetDay()
    {
        //背景由黑夜变成白天
    }

    void RetWolfAct()
    {
        //等待所有狼人确认完毕，这个发送给所有狼人
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retJson = JsonMapper.ToObject(json);
        if(retJson["type"].ToString()=="kill")
        {
            if(retJson["success"].ToString() == "True")
            {
                string target = retJson["content"]["target"].ToString();
                // 发送给所有狼人
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    WolfChatMsg.text += "系统：确认挂科人为"+ target + "\n";
                }

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retJson["type"].ToString()=="finish")
        {
            if (retJson["success"].ToString() == "True" && retJson["message"].ToString()=="kill finish")
            {
                VillagerChatMsg.text = "系统：教务部已操作完毕" + "\n";
                gameControl.gameState = GameControl.GameState.WITCH; // 进入女巫环节
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retJson["message"].ToString());
            }
        }
    }

    void RetWitchAct()//就直接做救/毒二选一吧，写起来简单一些
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retJson = JsonMapper.ToObject(json);

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // 如果是女巫
        {
            if (retJson["type"].ToString() == "kill result")
            {
                if (retJson["success"].ToString() == "True")
                {
                    string target = retJson["content"]["target"].ToString();
                    VillagerChatMsg.text += "系统：今夜挂科人为" + target + "\n";
                }
            }
           
        }
        if (retJson["type"].ToString() == "witch"|| retJson["type"].ToString() == "finish") // 返回接口中没有说明毒或者救的人是谁，需要客户端指定
        {
            if (retJson["success"].ToString() == "True" && retJson["message"].ToString() == "witch finish")
            {
                VillagerChatMsg.text += "系统：任课老师已操作完毕" + "\n";
                gameControl.gameState = GameControl.GameState.PROPHET;// 女巫环节结束，进入预言家环节。
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retJson["message"].ToString());
            }
        }
    }    

    void RetProphetAct()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retJson = JsonMapper.ToObject(json);

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // 如果是预言家
        {
            if (retJson["type"].ToString() == "prophet")
            {
                if (retJson["success"].ToString() == "True")
                {
                    string target = retJson["content"]["target"].ToString();
                    string chara = retJson["content"]["character"].ToJson() == "WOLF" ? "令人挂科的人" : "让人好好过年的人";
                    VillagerChatMsg.text += "系统：查验目标"+target+"为" + chara + "\n";
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retJson["content"]["character"].ToJson();
                    clare = false;
                    gameControl.gameState = GameControl.GameState.DISCUSS;
                    gameControl.hasDown = false;
                    
                }
            }

        }
    }

    void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;
        
        //TODO 发送 验人 请求

        //
    }
    void DiscussAct()
    {
        //这个发送给所有人
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retjson = JsonMapper.ToObject(json);
        
        if(clare == false && retjson["type"].ToString() == "night end")
        {
            if(retjson["success"].ToString()=="True")
            {
                int playerCnt = retjson["content"]["players"].Count;
                gameControl.players = new string[playerCnt];
                for(int i =0;i<playerCnt;i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString();
                }
                gameControl.playerCharacterMap = new HashMap<string, string>();
                for (int i = 0; i < retjson["content"]["playerCharacterMap"].Count; i++)
                {
                    gameControl.playerCharacterMap[retjson["content"]["playerCharacterMap"][i].Keys.ToString()] = retjson["content"]["players"][i].ToString();
                }
                gameControl.captain = retjson["content"]["captain"].ToString();

                clare = true;

                // TODO: VillagerChatMsg+="系统：昨晚挂科的人是" + gameControl.dayEvent.killed + "和"

                if(retjson["content"]["electCaptain"].ToString()=="True")
                {
                    // 票选警长
                    gameControl.gameState = GameControl.GameState.ELECT;
                    gameControl.hasDown = false;
                }
                else
                {
                    // TODO:允许发言
                    gameControl.gameState = GameControl.GameState.VOTE;
                    gameControl.hasDown = false;
                }
            }
        }

    }
    void DeadCheck()
    {

        GameObject targetOP;
        for(int i = 0; i < maxPlayerNum; i++ )
        {
            targetOP = GameObject.Find("PlayerBand" + maxPlayerNum);
            

            //string targetName = targetOP.


        }


    }

    void HasVictory()
    {
        // 处理返回消息
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retgameend = JsonMapper.ToObject(json);
        if (retgameend["type"].ToString() == "game end") // 验证消息类型，游戏结束
        { 
            if(retgameend["success"].ToString() == "True"&&long.Parse(retgameend["content"]["roomID"].ToString()) == room.roomID)
            {
                if(retgameend["winner"].ToString() == "WOLF") // 狼人胜利
                {
                    // TODO:添加狼人胜利UI提示
                    Debug.Log("教务部胜利");
                }
                else
                {
                    // TODO:添加好人胜利UI提示
                    Debug.Log("好人胜利");
                }
            }
        }
            int winNum = 0;//0没人赢，1好人赢，2狼人赢

    }

    void LastWords()
    {

    }

    bool CheckLive()
    {
        bool isLive = false;
        return isLive;
    }

    void ElectPolice()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        JsonData retJson = JsonMapper.ToObject(json);
        if(retJson["type"].ToString() == "elect")
        {
            if(retJson["success"].ToString()=="True")
            {
                string result = retJson["content"]["result"].ToString();
                VillagerChatMsg.text += "系统：选举警长为" + result +"\n";
                gameControl.dayEvent.nowPolice = result;
                for(int i = 0; i < retJson["content"]["voterTargetMap"].Count;i++)
                {
                    VillagerChatMsg.text += retJson["content"]["voterTargetMap"][i].Keys.ToString() + "投给了" + retJson["content"]["voterTargetMap"][i].ToString()+"\n";
                }
                gameControl.gameState = GameControl.GameState.DISCUSS;// 进入讨论环节
                gameControl.hasDown = false;

            }
        }
        
    }

    void VotePolice()
    {
        electPan.SetActive(false);
        //TODO 发送投票信息
    }


    void VoteKill()
    {
        //send btn enable
        votePan.SetActive(true);
    }

    void Ending()
    {
        //显示一个胜者表
    }

}
