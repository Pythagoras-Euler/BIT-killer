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
    public GameControl gameControl;
    public GameObject multyBtn; // 只有房主有，开启游戏按钮
    [SerializeField] UserInfo userInfo;
    [SerializeField] PlayerAssignment playerAssignment;
    private int minWaitTime;
    private int maxWaitTime;
    private int maxPlayerNum;

    public WebLink wl;
    public Room room;
    [SerializeField] Text WolfChatMsg;
    [SerializeField] Text VillagerChatMsg;
    [SerializeField] Text playerInfoDisplay;
    [SerializeField] Text roomInfoDisplay;
    [SerializeField] CountDownSlider countDown;

    bool clare = false; // 今天有没有发过死讯

    // Start is called before the first frame update
    void Start()
    {

        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        playerAssignment = GameObject.FindGameObjectWithTag("PlayerAssignment").GetComponent<PlayerAssignment>();
        gameControl = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();
        playerInfoDisplay.text = playerAssignment.playerName;
        roomInfoDisplay.text = room.roomID.ToString();

        PoliceId = -1;
        minWaitTime = 0;//TODO 一个合适的值，取决于计时器
        maxWaitTime = 9999;//同上
        multyBtn.SetActive(false);

        Waiting();
        
    }
    string json;
    JsonData retjson;
    bool sendFlag = false;
    // Update is called once per frame
    void Update()//阶段更新收发放到GameControl里
    {

        json = wl.receiveJson;
        Debug.Log(json);
        retjson = JsonMapper.ToObject(json);
        Debug.Log(retjson.ToString());
        Debug.Log(retjson["type"].ToString());

        switch (gameControl.gameState)//判断现在的阶段并执行相应函数
        {
            case GameControl.GameState.WAIT:
                Debug.Log("wait");
                Waiting();
                break;
            case GameControl.GameState.START://分发身份牌
                StartGame();
                break;
            case GameControl.GameState.KILL: // 狼人行动

                HasVictory(); // 检查游戏是否有赢家
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
        if (retjson["type"].ToString() == "join room") // 验证消息类型，加入房间
        {
            Debug.Log(retjson["success"].ToString()+":"+ retjson["content"]["roomID"].ToString()+":"+ room.roomID.ToString());
            if (retjson["success"].ToString() == "True" && retjson["content"]["roomID"].ToString() == room.roomID.ToString())// 成功加入当前房间
            {
                Debug.Log(retjson["success"].ToString());
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // 更新玩家表
                    room.players[i] = gameControl.players[i];
                    Debug.Log(room.players[i]);
                }
                
            }
        }
        if (retjson["type"].ToString() == "create room") // 验证消息类型，加入房间
        {
            if (retjson["success"].ToString() == "True" && long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)// 成功加入当前房间
            {
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++) {

                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // 更新玩家表
                    room.players[i] = gameControl.players[i];
                }
            }
        }
        else if (retjson["type"].ToString() == "leave room") // 验证消息类型，退出房间
        {
            if (retjson["success"].ToString() == "True" && long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)// 成功离开当前房间
            {
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // 更新玩家表
                    room.players[i] = gameControl.players[i];
                }
            }
        }
        else if (retjson["type"].ToString() == "game start") // 验证消息类型，开始游戏
        {
            Debug.Log("success start");
            if (retjson["success"].ToString() == "True" && long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)
            {
                Debug.Log("start");
                int playerCount = retjson["content"]["players"].Count;
                gameControl.players = new string[playerCount];
                gameControl.playerCharacterMap = new Hashtable(7);
                gameControl.playerStateMap = new Hashtable(7);
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // 更新玩家表
                    //gameControl.playerCharacterMap[retjson["content"]["players"][i].ToString()] = retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString();
                    gameControl.playerCharacterMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString());
                    Debug.Log(retjson["content"]["players"][i].ToString() + ":" + retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString());
                    //gameControl.playerStateMap[retjson["content"]["players"][i].ToString()] = retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "true" : "false";
                    gameControl.playerStateMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "true" : "false");
                }
                multyBtn.SetActive(false);
                gameControl.gameState = GameControl.GameState.START;// 更新游戏状态
                return;
            }
        }
        bool everyoneReady = false;
        if (gameControl.players.Length == 7)
        { everyoneReady = true; }

        if (everyoneReady)//所有人准备
        {
            if(room.iscurcreator)
                multyBtn.SetActive(true);

            // 处理服务器发来可以开始游戏返回值
            //sr = new StreamReader(Application.dataPath + "/jsontest.txt");
            //json = sr.ReadToEnd().TrimEnd('\0');
            //Debug.Log(json);
            

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

    string CharacterInChinese()
    {
        string chinese = "谜语人";
        switch (playerAssignment.playerCharacter)
        {
            case PlayerAssignment.Character.VILLAGE:
                chinese = "普通学生";
                break;
            case PlayerAssignment.Character.PROPHET:
                chinese= "网信人员";
                break;
            case PlayerAssignment.Character.WITCH:
                chinese= "任课老师";
                break;
            case PlayerAssignment.Character.WOLF:
                chinese =  "教务部";
                break;
        }
        return chinese;
    }

    //分发身份牌,可以弹出一个窗口，也可以在聊天栏有一个系统消息，同时初始化对局信息
    int count = 0;
    void StartGame()
    {
        // 
        SetNight();
        if (!sendFlag)
        {
            sendFlag = !sendFlag;//~sendFlag
            VillagerChatMsg.text += "您的本局身份为" + CharacterInChinese() + "\n";

            for(int i =0;i<7;i++)
            {
                if(gameControl.playerCharacterMap[gameControl.players[i]].ToString()=="WOLF" && gameControl.players[i] != userInfo.username)
                {

                    VillagerChatMsg.text += "您的同事为" + gameControl.players[i] + "\n";
                    VillagerChatMsg.text += "请致力于让学生们全部挂科吧！";
                    break;
                }
            }

            if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
            {
                gameControl.canDo[0] = 1;
                gameControl.canDo[1] = 1;
            }
            else
            {
                gameControl.canDo[0] = 0;
                gameControl.canDo[1] = 0;
            }
            for (int i = 0; i < 7; i++)
            {
                gameControl.seenPlayers[i] = "";
            }

                countDown.setCountDown(0, 0, 5);
        }
        if (countDown.countDownSlider == null || countDown.countDownSlider.value == 0)
        {
            // 倒计时结束，开启下一阶段
            gameControl.gameState = GameControl.GameState.KILL;
            Debug.Log("Kill");
        }
    
    }

    //
    void SetNight()
    {

        HasVictory(); // 检查游戏是否有赢家
        //背景由白天变成黑夜
    }

    void SetDay()
    {

        HasVictory(); // 检查游戏是否有赢家
        //背景由黑夜变成白天
    }

    void RetWolfAct()
    {
        SetNight();
        //等待所有狼人确认完毕，这个发送给所有狼人
        if(retjson["type"].ToString()=="kill")
        {
            if(retjson["success"].ToString() == "True")
            {
                string target = retjson["content"]["target"].ToString();
                // 发送给所有狼人
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    WolfChatMsg.text += "系统：确认挂科人为"+ target + "\n";
                }

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retjson["type"].ToString()=="finish")
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString()=="kill finish")
            {
                VillagerChatMsg.text = "系统：教务部已操作完毕" + "\n";
                gameControl.gameState = GameControl.GameState.WITCH; // 进入女巫环节
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    void RetWitchAct()//就直接做救/毒二选一吧，写起来简单一些
    {


        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // 如果是女巫
        {
            if (retjson["type"].ToString() == "kill result")
            {
                if (retjson["success"].ToString() == "True")
                {
                    string target = retjson["content"]["target"].ToString();
                    VillagerChatMsg.text += "系统：今夜挂科人为" + target + "\n";
                }
            }
           
        }
        if (retjson["type"].ToString() == "witch"|| retjson["type"].ToString() == "finish") // 返回接口中没有说明毒或者救的人是谁，需要客户端指定
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString() == "witch finish")
            {
                VillagerChatMsg.text += "系统：任课老师已操作完毕" + "\n";
                gameControl.gameState = GameControl.GameState.PROPHET;// 女巫环节结束，进入预言家环节。
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }    

    void RetProphetAct()
    {


        if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // 如果是预言家
        {
            if (retjson["type"].ToString() == "prophet")
            {
                if (retjson["success"].ToString() == "True")
                {
                    string target = retjson["content"]["target"].ToString();
                    string chara = retjson["content"]["character"].ToJson() == "WOLF" ? "令人挂科的人" : "让人好好过年的人";
                    VillagerChatMsg.text += "系统：查验目标"+target+"为" + chara + "\n";
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retjson["content"]["character"].ToJson();
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
        SetDay();
        //这个发送给所有人

        
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
                gameControl.playerCharacterMap = new Hashtable();
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
        if (retjson["type"].ToString() == "game end") // 验证消息类型，游戏结束
        { 
            if(retjson["success"].ToString() == "True"&&long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)
            {
                if(retjson["winner"].ToString() == "WOLF") // 狼人胜利
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
        else
        {
            return;
        }

    }

    void LastWords()
    {
        SetDay();
    }

    bool CheckLive()
    {
        bool isLive = false;
        return isLive;
    }

    void ElectPolice()
    {
        if(retjson["type"].ToString() == "elect")
        {
            if(retjson["success"].ToString()=="True")
            {
                string result = retjson["content"]["result"].ToString();
                VillagerChatMsg.text += "系统：选举警长为" + result +"\n";
                gameControl.dayEvent.nowPolice = result;
                for(int i = 0; i < retjson["content"]["voterTargetMap"].Count;i++)
                {
                    VillagerChatMsg.text += retjson["content"]["voterTargetMap"][i].Keys.ToString() + "投给了" + retjson["content"]["voterTargetMap"][i].ToString()+"\n";
                }
                gameControl.gameState = GameControl.GameState.DISCUSS;// 进入讨论环节
                gameControl.hasDown = false;

            }
        }
        
    }

    void VotePolice()
    {
        //TODO 发送投票信息
    }


    void VoteKill()
    {
        //send btn enable
    }

    void Ending()
    {
        //显示一个胜者表
    }

}
