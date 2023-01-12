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
        switch(gameControl.gameState)//判断现在的阶段并执行相应函数
        {
            case GameControl.GameState.WAIT:
                Waiting();
                break;
            case GameControl.GameState.START://分发身份牌
                StartGame();
                break;
            case GameControl.GameState.KILL:
                WolfAct();
                break;
            case GameControl.GameState.PROPHET:
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
        Debug.Log(wl.receiveJson);
        Debug.Log(BitConverter.ToString(wl.reault));
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
            if (retnewjoinroom["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)// 成功加入当前房间
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
        }
    }

    //分发身份牌,可以弹出一个窗口，也可以在聊天栏有一个系统消息
    void StartGame()
    {
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

    void WolfAct()//狼人是不是应该能看到队友投了谁
    {
        SetNight();
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (playerAssignment.playerState == true)
            {
                wolfPan.SetActive(true);
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
        witchPan.SetActive(false);
        //TODO 发送消息
    }

    void ProphetAct()
    {
        seerPan.SetActive(true);
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

    int HasVictory()
    {
        int winNum = 0;//0没人赢，1好人赢，2狼人赢

        return winNum;
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
