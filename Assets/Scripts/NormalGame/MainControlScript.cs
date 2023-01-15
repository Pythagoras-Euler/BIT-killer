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
    [SerializeField] ChatPanel chatPanel;
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
    bool killFlag = false;
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
                    gameControl.playerStateMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "True" : "False");
                    
                    playerAssignment.playerCharacter = GetChara();
                    if(playerAssignment.playerCharacter!=PlayerAssignment.Character.WOLF)
                    {
                        chatPanel.TeamBtn.SetActive(false);
                        WolfChatMsg.gameObject.SetActive(false);
                    }
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

    PlayerAssignment.Character GetChara()
    {
        string thisnBandIden = gameControl.playerCharacterMap[userInfo.username].ToString();
        PlayerAssignment.Character thisChe;
        if (thisnBandIden == "VILLAGE")
        {
            thisChe = PlayerAssignment.Character.VILLAGE;
        }
        else if (thisnBandIden == "WOLF")
        {
            thisChe = PlayerAssignment.Character.WOLF;
        }
        else if (thisnBandIden == "PROPHET")
        {
            thisChe = PlayerAssignment.Character.PROPHET;
        }
        else if (thisnBandIden == "WITCH")
        {
            thisChe = PlayerAssignment.Character.WITCH;
        }
        else if (thisnBandIden == "UNDEF")
        {
            thisChe = PlayerAssignment.Character.UNDEF;
        }
        else
        {
            thisChe = PlayerAssignment.Character.UNDEF;
        }

        return thisChe;
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

        if (!sendFlag)
        {
            sendFlag = !sendFlag;//~sendFlag
            VillagerChatMsg.text += "您的本局身份为" + CharacterInChinese() + "\n";
            if(playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)// 如果是狼
                for (int i =0;i<7;i++)
                {
                    if(playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF && gameControl.players[i] != userInfo.username)
                    {

                        VillagerChatMsg.text += "您的同事为" + gameControl.players[i] + "\n";
                        VillagerChatMsg.text += "请致力于让学生们全部挂科吧！\n";
                        break;
                    }
                }
            else if(playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
            {
                VillagerChatMsg.text += "别人的及格与挂科只在你的一念之间\n";
            }
            else if (playerAssignment.playerCharacter == PlayerAssignment.Character.VILLAGE)
            {
                VillagerChatMsg.text += "享受每一刻吧\n";
            }
            else if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET)
            {
                VillagerChatMsg.text += "你可以探查所有人的身份\n";
            }
            else
            {

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
        SetNight();
    
    }

    public bool CountIsDown()
    {
        return countDown.countDownSlider == null || countDown.countDownSlider.value == 0;
    }

    //
    void SetNight()
    {

        HasVictory(); // 检查游戏是否有赢家
        // 开启狼人阶段
        if (CountIsDown())
        {
            // 倒计时结束，开启下一阶段
            gameControl.gameState = GameControl.GameState.KILL;
            Debug.Log("Kill");
            VillagerChatMsg.text += "<b><color=red>系统：现在是教务部操作时间</color></b>\n";
            countDown.setCountDown(0, 0, 30);
        }
        //背景由白天变成黑夜

    }

    void SetDay()
    {

        HasVictory(); // 检查游戏是否有赢家
        //背景由黑夜变成白天
    }

    void RetWolfAct()
    {
        //等待所有狼人确认完毕，这个发送给所有狼人
        if(retjson["type"].ToString()=="kill")
        {
            if(retjson["success"].ToString() == "True")
            {
                string target = retjson["content"]["target"].ToString();
                // 发送给所有狼人
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    if(killFlag == false)
                    {
                        WolfChatMsg.text += "系统：确认挂科人为" + target + "\n";
                    }
                }

                killFlag = true;

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retjson["type"].ToString()=="finish")
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString()=="kill finish")
            {
                if (CountIsDown())
                {
                    VillagerChatMsg.text += "系统：教务部已操作完毕" + "\n";
                    killFlag = false;
                    gameControl.gameState = GameControl.GameState.WITCH; // 进入女巫环节
                    countDown.setCountDown(0, 0, 30);
                    gameControl.hasDown = false;
                    VillagerChatMsg.text += "<b><color=red>系统：现在是任课老师操作时间</color></b>\n";
                }
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    bool witchinformed = false;
    bool witchacted = false;
    void RetWitchAct()//就直接做救/毒二选一吧，写起来简单一些
    {

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // 如果是女巫
        {
            if (retjson["type"].ToString() == "kill result")
            {
                if (retjson["success"].ToString() == "True")
                {
                    if(witchinformed == false)
                    {
                        if (CountIsDown())
                        {
                            string target = retjson["content"]["target"].ToString();
                            VillagerChatMsg.text += "系统：今夜挂科人为" + target + "\n";
                            witchinformed = true;

                        }
                    }
                }
            }
           
        }
        if (retjson["type"].ToString() == "witch"|| retjson["type"].ToString() == "finish") // 返回接口中没有说明毒或者救的人是谁，需要客户端指定
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString() == "witch finish")
            {
                if(witchacted == false)
                {
                    if (CountIsDown())
                    {
                        VillagerChatMsg.text += "系统：任课老师已操作完毕" + "\n";
                        gameControl.gameState = GameControl.GameState.PROPHET;// 女巫环节结束，进入预言家环节。
                        gameControl.hasDown = false;
                        witchacted = true;
                        countDown.setCountDown(0, 0, 30);

                        VillagerChatMsg.text += "系统：现在是网信操作时间" + "\n";
                    }
                }
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    bool proact = false;
    JsonData retjson2;
    bool retjsonFlag = false;
    void RetProphetAct()
    {
        if (retjson["type"].ToString() == "prophet")
        {
            if (retjson["success"].ToString() == "True")
            {
                if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // 如果是预言家
                {
                    string target = retjson["content"]["target"].ToString();
                    
                    string chara = retjson["content"]["character"].ToString() == "WOLF" ? "令人挂科的人" : "让人好好过年的人";
                    Debug.Log(retjson["content"]["character"].ToString());
                    if(proact == false )
                    {
                        VillagerChatMsg.text += "系统：查验目标" + target + "为" + chara + "\n";
                        proact = true;
                    }
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retjson["content"]["character"].ToJson();
                }



            }

        }
        if (proact == true)
        {
            //收night end
            if (retjson["type"].ToString() == "night end" && retjsonFlag == false)
            {
                retjson2 = retjson;
                retjsonFlag = true;
            }

            if (CountIsDown())
                {
                    VillagerChatMsg.text += "系统：网信人员操作完毕\n";
                    clare = false;
                    gameControl.gameState = GameControl.GameState.DISCUSS;
                    gameControl.hasDown = false;
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
   public bool daylock = false;
    public bool electlock = false;
    void DiscussAct()
    {
        SetDay();

        
        JsonData retjson0 = retjsonFlag ? retjson2 : retjson;
        Debug.Log(retjson0["type"].ToString());
        //这个发送给所有人
        if(retjson0["type"].ToString() == "night end")
        {
            if(retjson0["success"].ToString()=="True")
            {
                retjsonFlag = false;

                int playerCnt = retjson0["content"]["players"].Count;
                gameControl.players = new string[playerCnt];
                for(int i =0;i<playerCnt;i++)
                {
                    gameControl.players[i] = retjson0["content"]["players"][i].ToString();
                }
                //gameControl.playerCharacterMap = new Hashtable();
                foreach(var player in gameControl.players)
                {
                    gameControl.playerStateMap[player] = retjson0["content"]["playerStateMap"][player].ToString();
                    Debug.Log(player+gameControl.playerStateMap[player]);

                }
                //for (int i = 0; i < retjson0["content"]["playerCharacterMap"].Count; i++)
                //{
                //    gameControl.playerCharacterMap[retjson0["content"]["playerCharacterMap"][i].Keys.ToString()] = retjson0["content"]["players"][i].ToString();
                //}
                gameControl.captain = retjson0["content"]["captain"].ToString();
                if(clare == false)
                {
                    if(gameControl.dayEvent.killed == ""&& gameControl.dayEvent.poisoned != "")
                        VillagerChatMsg.text += "系统：昨晚挂科的人是"+gameControl.dayEvent.poisoned + "\n";
                    else if(gameControl.dayEvent.killed != "" && gameControl.dayEvent.poisoned == "")
                        VillagerChatMsg.text += "系统：昨晚挂科的人是" + gameControl.dayEvent.killed + "\n";
                    else if(gameControl.dayEvent.killed != "" && gameControl.dayEvent.poisoned != "")
                        VillagerChatMsg.text += "系统：昨晚挂科的人是" + gameControl.dayEvent.killed+"和"+ gameControl.dayEvent.poisoned+"\n";
                    else
                    {
                        VillagerChatMsg.text += "系统：昨晚是平安夜\n";
                    }
                    clare = true;
                    //countDown.setCountDown(0, 1, 23);
                }

                killFlag = false;
                witchinformed = false;
                witchacted = false;
                proact = false;
                electclarelock = false;

                if (retjson0["content"]["electCaptain"].ToString()=="True")
                {
                    // 票选警长
                    if(electlock == false)
                    {

                        VillagerChatMsg.text += "系统：现在是警长选举时间，请各位畅所欲言" + "\n";

                        countDown.setCountDown(0, 0, 45);
                        Debug.Log("setCountDown");

                        electlock = true;

                    }
                    else if (electlock == true && CountIsDown())
                    {
                        gameControl.gameState = GameControl.GameState.ELECT;
                        countDown.setCountDown(0, 0, 10);
                        gameControl.hasDown = false;
                    }
                }
                else
                {
                    // TODO:允许发言
                    if(daylock == false)
                    {
                        VillagerChatMsg.text += "系统：现在是自由讨论时间，请各位畅所欲言" + "\n";
                        daylock = true;
                        countDown.setCountDown(0, 1, 0);
                    }
                    else if(CountIsDown())
                    {
                        VillagerChatMsg.text += "系统：现在是投票环节" + "\n";
                        gameControl.gameState = GameControl.GameState.VOTE;
                        gameControl.hasDown = false;
                        countDown.setCountDown(0, 0, 10);
                    }
                }
            }
        }
        else if ( retjson0["type"].ToString() == "elect")
        {
            Debug.Log(daylock.ToString());
            if (daylock == false)
            {
                VillagerChatMsg.text += "系统：现在是自由讨论时间，请各位畅所欲言" + "\n";
                daylock = true;
                countDown.setCountDown(0, 1, 0);
            }
            if (CountIsDown())
            {
                VillagerChatMsg.text += "系统：现在是投票环节" + "\n";
                gameControl.gameState = GameControl.GameState.VOTE;
                gameControl.hasDown = false;
                countDown.setCountDown(0, 0, 10);
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
    public  bool electclarelock = false;
    void ElectPolice()
    {
        if(retjson["type"].ToString() == "elect")
        {
            if(retjson["success"].ToString()=="True")
            {
                Debug.Log(electclarelock.ToString());
                if(electclarelock==false)
                {
                    Debug.Log(retjson["content"]["result"].ToString());
                    string result = retjson["content"]["result"].ToString();
                    Debug.Log(result);
                    //for(int i =0;i< 7; i++)
                    //{
                        //if(gameControl.playerStateMap[gameControl.players[i]] == "true")
                        //{
                            //Debug.Log(gameControl.players[i] + ":" + retjson["content"]["voterTargetMap"][gameControl.players[i]].ToString());
                            //VillagerChatMsg.text += "系统：" + gameControl.players[i] + "投给了" + retjson["content"]["voterTargetMap"][gameControl.players[i]].ToString() + "\n";

                        //}
                    //}
                    VillagerChatMsg.text += "系统：选举警长为" + result + "\n";
                    gameControl.dayEvent.nowPolice = result;
                    electclarelock = true;
                    gameControl.gameState = GameControl.GameState.DISCUSS;// 进入讨论环节
                    gameControl.hasDown = false;
                    // countDown.setCountDown(0, 1, 0);
                }
            }
        }
        
    }

    public bool voteclare = false;
    void VoteKill()
    {
        // 处理投票返回值
        if(retjson["type"].ToString()=="vote")
        {
            if(retjson["success"].ToString()=="True")
            {
                if(retjson["content"]["tie"].ToString()=="True")
                {
                    VillagerChatMsg.text += "系统：平票，无事发生" + "\n";
                }
                else
                {
                    string voteName = retjson["content"]["result"].ToString();
                    if(voteclare == false)
                    {
                        VillagerChatMsg.text += "系统：投票结果为"+ voteName + "\n";
                        gameControl.playerStateMap[voteName] = "false";
                        voteclare = true;
                    }
                }
                SetNight();
            }
        }
    }

    void Ending()
    {
        //显示一个胜者表
    }

}


/**
 *  _ooOoo_
 * o8888888o
 * 88" . "88
 * (| -_- |)
 *  O\ = /O
 * ___/`---'\____
 * .   ' \\| |// `.
 * / \\||| : |||// \
 * / _||||| -:- |||||- \
 * | | \\\ - /// | |
 * | \_| ''\---/'' | |
 * \ .-\__ `-` ___/-. /
 * ___`. .' /--.--\ `. . __
 * ."" '< `.___\_<|>_/___.' >'"".
 * | | : `- \`.;`\ _ /`;.`/ - ` : | |
 * \ \ `-. \_ __\ /__ _/ .-` / /
 * ======`-.____`-.___\_____/___.-`____.-'======
 * `=---='
 *          .............................................
 *           佛曰：bug泛滥，我已瘫痪！
 */