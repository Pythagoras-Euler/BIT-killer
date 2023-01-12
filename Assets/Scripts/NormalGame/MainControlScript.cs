using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
            case GameControl.GameState.PROPHET: // 预言家行动
                ProphetExamine();
                break;
            case GameControl.GameState.WITCH:
                WitchAct();
                break;
            case GameControl.GameState.DISCUSS:
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
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (playerAssignment.playerState == true)
            {
                //TODO 还应该有一个倒计时模块
            }
            else//显示信息但是按钮无效
            {
                //TODO
            }
        }
    }

    //BTN响应事件
    void WolfVote()
    {
        wolfPan.SetActive(false);
        //TODO 倒计时此时停止并重置
        //TODO 发送投票信息
    }

    void WitchAct()//就直接做救/毒二选一吧，写起来简单一些
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
        {
            if ( playerAssignment.playerState == true)
            {
                witchPan.SetActive(true);
                //TODO 还应该有一个倒计时模块
            }
            else
            {
                int waittime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
                //TODO 随机倒计时，假装人没死
            }
        }
    }    

    void WitchPison()
    {
        witchPan.SetActive(false);
        //TODO 发送消息
    }

    void WitchSave()
    {
        // witchPan.SetActive(false);
        // TODO 发送消息
    }

    void ProphetAct()
    {
        // seerPan.SetActive(true);
    }

    void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;
        
        //TODO 发送 验人 请求

        //
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
        Debug.Log(json);
        //string json = wl.receiveJson;
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
        electPan.SetActive(true);
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
