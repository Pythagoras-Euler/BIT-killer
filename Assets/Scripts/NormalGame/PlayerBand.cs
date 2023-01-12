using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBand : MonoBehaviour
{
    [SerializeField] UserInfo userInfo;// 存放玩家名
    [SerializeField] PlayerAssignment playerAssignment;// 存放客户端玩家身份和座位号等信息
    [SerializeField] string userName;
    [SerializeField] int seatNum;
    [SerializeField] GameControl gameControl;
    [SerializeField] Room room;
    [SerializeField] WebLink wl;


    public bool needRefreshFlag = false;//万一需要外界强制刷新，可以修改这个Flag
    public MainControlScript mainControl;
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

    public Text playerNum;
    public Text playerName;

    private string targetName = "";
    PlayerAssignment.Character targetCharacter;
    PlayerAssignment.Character NowMark;

    // Start is called before the first frame update
    void Start()
    {
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        userName = userInfo.username;
        playerAssignment = GameObject.Find("PlayerAssignment").GetComponent<PlayerAssignment>();
        seatNum = playerAssignment.seatNum;
        playerNum.text = seatNum.ToString();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();

        NowMark = PlayerAssignment.Character.UNDEF;
        targetCharacter = PlayerAssignment.Character.UNDEF;

        InfoRefresh();

    }

    PlayerAssignment.Character thisChara()
    {
        string thisnBandIden = gameControl.playerCharacterMap[targetName];
        PlayerAssignment.Character thisChe;
        if (thisnBandIden == "VILLAGE")
        {
            thisChe = PlayerAssignment.Character.VILLAGE;
        }
        else if(thisnBandIden == "WOLF")
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


    // Update is called once per frame
    void Update()
    {
        if(needRefreshFlag == true)
        {
            InfoRefresh();
        }


        switch(gameControl.gameState)//判断现在的阶段并执行相应函数
        {
            case GameControl.GameState.WAIT://等待开始
                Waiting();
                InfoRefresh();
                break;
            case GameControl.GameState.START://分发身份牌
                StartGame();
                InfoRefresh();
                break;
            case GameControl.GameState.KILL://狼人回合
                WolfAct();
                break;
            case GameControl.GameState.PROPHET://预言家回合
                ProphetAct();
                break;
            case GameControl.GameState.WITCH://女巫回合
                WitchAct();
                break;
            case GameControl.GameState.DISCUSS://讨论环节
                break;
            case GameControl.GameState.ELECT://选举警长
                ElectPolice();
                break;
            case GameControl.GameState.VOTE://投票环节
                VoteKill();
                break;
            case GameControl.GameState.WORDS://遗言环节
                LastWords();
                break;
            case GameControl.GameState.END://结算，宣布胜者
                break;
        }
    }

    void InfoRefresh()
    {
        targetName = gameControl.players[seatNum - 1];
        playerName.text = targetName;

    }

    //等待页面，将所有icon设置为不可用
    void Waiting()
    {
        doubtPan.SetActive(false);
        electPan.SetActive(false);
        statePan.SetActive(false);
        villagerPan.SetActive(false);
        wolfPan.SetActive(false);
        witchPan.SetActive(false);
        seerPan.SetActive(false);
        votePan.SetActive(false);
        idenIcon.SetActive(false);
    }

    //分发身份牌,可以弹出一个窗口，也可以在聊天栏有一个系统消息
    void StartGame()
    {
        NowMark = PlayerAssignment.Character.UNDEF;

        InfoRefresh();

        targetCharacter = thisChara();
    }



    void WolfAct()//TODO 狼人是不是应该能看到队友投了谁
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (gameControl.hasDown == false)
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
    }

    //BTN响应事件
    public void WolfVote()
    {
        wolfPan.SetActive(false);
        gameControl.hasDown = true;
        //TODO 倒计时此时停止并重置
        //TODO 发送投票信息
        JsonData data1 = new JsonData();
        data1["type"] = "kill";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] =room.roomID;
        data1["content"]["voter"] = userName;
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);

    }

    void WitchAct()//就直接做救/毒二选一吧，写起来简单一些
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH && gameControl.hasDown == false)
        {
            if (playerAssignment.playerState == true)
            {
                witchPan.SetActive(true);
            }
        }
        else
        {
            witchPan.SetActive(false);
        }
    }

    public void WitchPison()
    {

        gameControl.hasDown = true;
        witchPan.SetActive(false);
        //TODO 发送消息

    }

    public void WitchSave()
    {

        gameControl.hasDown = true;
        witchPan.SetActive(false);
        //TODO 发送消息
    }

    void ProphetAct()
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH && playerAssignment.playerState == true && gameControl.hasDown == false)
        {
            seerPan.SetActive(true);
        }
        else
        {
            seerPan.SetActive(false);
        }
    }

    public void MarkBtn()
    {
        NowMark++;
        //重新加载图片
    }


    public void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;

        //TODO 发送 验人 请求

        //
    }

    void DeadCheck()
    {
        if(playerName)
        {

        }


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
        if (gameControl.hasDown == false)
        {
            electPan.SetActive(true);
        }
        else
        {
            electPan.SetActive(false);
        }
    }

    public void VotePolice()
    {
        electPan.SetActive(false);
        //TODO 发送投票信息
    }


    void VoteKill()
    {
        //send btn enable
        if (gameControl.hasDown == false)
        {
            votePan.SetActive(true);
        }
        else
        {
            votePan.SetActive(false);
        }
    }

    public void MakeVote()
    {
        gameControl.hasDown = true;
        votePan.SetActive(false);
    }

    void Ending()
    {
        Waiting();
    }

}
