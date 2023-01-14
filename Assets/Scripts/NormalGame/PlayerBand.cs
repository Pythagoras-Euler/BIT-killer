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
    public Room room;
    public WebLink wl;

    
    public bool needRefreshFlag = false;//万一需要外界强制刷新，可以修改这个Flag
    public GameObject doubtPan;
    public Image doubtIcon;
    public GameObject electPan;
    public GameObject electBtn;
    public GameObject policeStar;
    public GameObject statePan;
    public GameObject villagerPan;
    public GameObject wolfPan;
    public GameObject witchPan;
    public GameObject witchSaveBtn;
    public GameObject witchPoiBtn;
    public GameObject seerPan;
    public GameObject votePan;
    public GameObject idenIcon;

    public Text playerNum;
    public Text playerName;

    private string myName = "";
    private bool IAmAlive = false;

    private bool isEmpty = true;

    private string targetName = "";
    private bool targetIsDead = false;
    PlayerAssignment.Character targetCharacter;
    PlayerAssignment.Character myCharacter;
    PlayerAssignment.Character NowMark;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        playerNum.text = seatNum.ToString();
        playerName.text = room.players[seatNum - 1];
        userName = userInfo.username;
        playerAssignment = GameObject.FindGameObjectWithTag("PlayerAssignment").GetComponent<PlayerAssignment>();
        
        Debug.Log(gameControl.players[seatNum - 1]);
        Debug.Log(playerName.text);
        gameControl = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();

        NowMark = PlayerAssignment.Character.UNDEF;
        targetCharacter = PlayerAssignment.Character.UNDEF;

        InfoRefresh();

    }

    PlayerAssignment.Character GetChara(string Name)
    {
        string thisnBandIden = gameControl.playerCharacterMap[Name].ToString();
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
        
         InfoRefresh();
        

        PlayerCheckLive();
        TargetDeadCheck();
        MyInfo();


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
                Ending();
                break;
        }
    }

    void InfoRefresh()
    {
        playerNum.text = seatNum.ToString();
        if (gameControl.players[seatNum - 1] != "")
        {
            isEmpty = false;
            targetName = gameControl.players[seatNum - 1];
            playerName.text = targetName;
            myName = playerAssignment.name;
            myCharacter = playerAssignment.playerCharacter;
        }
        else
        {
            isEmpty = true;
        }
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

        targetCharacter = GetChara(targetName);

        myCharacter = GetChara(myName);
    }

    void MyInfo()//更新视野
    {
        ISeen();

        if(myCharacter == PlayerAssignment.Character.WITCH)//女巫视野
        {
            WolfSeen();
        }
        else if(myCharacter == PlayerAssignment.Character.PROPHET)//狼人视野
        {
            ProphetSeen();
        }

        if(IAmAlive == false)//死人视野
        {
            AllSeen();
        }
    }
    void IdenIconDisplay(bool displayGod = false)//TODO 显示正确的IdenIcon,true显示神官身份
    {

    }
    void ISeen()
    {
        if(IsMe())
        {
            idenIcon.SetActive(true);
            IdenIconDisplay(true);
        }
        DisplayPolice();
    }

    void AllSeen()
    {
        idenIcon.SetActive(true);
        IdenIconDisplay();
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
                else//显示信息但是按钮无效 //没做队友投票显示之前没用
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
        data1["content"]["voter"] = myName;
        data1["content"]["target"] = targetName;
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);

    }

    void WolfSeen()
    {
        if (!IsMe())
        {
            if (targetCharacter == PlayerAssignment.Character.WOLF)
            {
                idenIcon.SetActive(true);
                IdenIconDisplay(false);
            }
            else
            {
                idenIcon.SetActive(false);
                IdenIconDisplay(false);
            }
        }
    }

    void WitchAct()//就直接做救/毒二选一吧，写起来简单一些 TODO IMPORTANT 女巫只能毒一次！
    {
        if (myCharacter == PlayerAssignment.Character.WITCH && gameControl.hasDown == false)
        {
            if (IAmAlive == true && targetIsDead == false)
            {
                witchPan.SetActive(true);
                if(targetName == gameControl.dayEvent.killed && gameControl.canDo[0] > 0)//目标为本回合死人且解药没用过时可使用
                {
                    witchSaveBtn.SetActive(true);
                }
                else
                {
                    witchSaveBtn.SetActive(false);
                }
                if(gameControl.canDo[1] > 0)//毒药没用过时可使用
                {
                    witchPoiBtn.SetActive(true);
                }
                else
                {
                    witchPoiBtn.SetActive(false);
                }
            }
            else
            {
                witchPan.SetActive(false);
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
        gameControl.canDo[1] -= 1;
        //TODO 发送消息
        JsonData data1 = new JsonData();
        data1["type"] = "witch";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = room.roomID;
        data1["content"]["target"] = targetName;
        data1["content"]["drug"] = "POISON";
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);
    }

    public void WitchSave()
    {

        gameControl.hasDown = true;
        witchPan.SetActive(false);
        gameControl.canDo[0] -= 1;
        //TODO 发送消息
        JsonData data1 = new JsonData();
        data1["type"] = "witch";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = room.roomID;
        data1["content"]["target"] = targetName;
        data1["content"]["drug"] = "ANTIDOTE";
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);
    }
    
    bool IsMe()
    {
        if (targetName == myName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ProphetSeen()
    {
        if (!IsMe())
        {
            if (ProphetChecked(targetName))//需要一个Examined表
            {
                if (targetCharacter == PlayerAssignment.Character.WOLF)
                {
                    idenIcon.SetActive(true);
                    IdenIconDisplay(false);
                }
                else
                {
                    idenIcon.SetActive(true);
                    IdenIconDisplay(false);
                }
            }
            else
            {
                idenIcon.SetActive(false);
            }
        }
    }

    bool ProphetChecked(string name)
    {
        //GameControl里面维护一个表显示验过的玩家身份
        //好像不需要发后端？
        bool isChecked = false;
        for (int i = 0; i < 7; i++)
        {
            //if遍历一遍checked数组
            if (gameControl.seenPlayers[i] == name)
            {
                isChecked = true;
            }
        }
        return isChecked;
    }

    void ProphetAct()
    {
        if (myCharacter == PlayerAssignment.Character.WITCH && IAmAlive == true && gameControl.hasDown == false)
        {
            seerPan.SetActive(true);
        }
        else
        {
            seerPan.SetActive(false);
        }
    }

    public void ProphetExamining()
    {
        int i;
        gameControl.hasDown = true;

        for (i = 0; i < 7 && gameControl.seenPlayers[i] == ""; i++) ;

        gameControl.seenPlayers[i] = targetName;

        //TODO 发送消息
        JsonData data1 = new JsonData();
        data1["type"] = "prophet";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = room.roomID;
        data1["content"]["target"] = targetName;
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);
    }


    public void MarkBtn()
    {
        NowMark++;
        //doubtIcon.sprite = ''  ;//TODO ICON显示
        //重新加载图片

        switch (NowMark)
        {
            case PlayerAssignment.Character.PROPHET:
                break;
            case PlayerAssignment.Character.VILLAGE:
                break;
            case PlayerAssignment.Character.WITCH:
                break;
            case PlayerAssignment.Character.WOLF:
                break;
            case PlayerAssignment.Character.UNDEF:
                break;
        }

    }





    //目标（player）死没死
    void TargetDeadCheck()
    {
        if(gameControl.playerStateMap[targetName].ToString() == "true" || gameControl.playerStateMap[targetName].ToString() == "True")
        {
            targetIsDead = false;
            statePan.SetActive(false);
        }
        else 
        {
            targetIsDead = true;
            statePan.SetActive(true);
        }


    }


    void LastWords()
    {

    }

    //自己（user）死没死
    void PlayerCheckLive()
    {
        IAmAlive = playerAssignment.playerState;
    }

    void ElectPolice()
    {
        if (gameControl.hasDown == false && targetIsDead == false && IAmAlive == true)
        {
            electPan.SetActive(true);
            electBtn.SetActive(true);
            policeStar.SetActive(false);
        }
        else
        {
            electBtn.SetActive(false);
        }
    }

    public void VotePolice()
    {
        electPan.SetActive(false);
        //TODO 发送投票信息
        JsonData data1 = new JsonData();
        data1["type"] = "elect";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = room.roomID;
        data1["content"]["voter"] = myName;
        data1["content"]["target"] = targetName;
        string klJson = data1.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);
    }

    void DisplayPolice()
    {
        if(gameControl.dayEvent.nowPolice == targetName )
        {
            electPan.SetActive(true);
            electBtn.SetActive(false);
            policeStar.SetActive(true);
        }
    }

    private void ChangePolice()
    {
        if (gameControl.hasDown == false && targetIsDead == false)
        {
            electPan.SetActive(true);
            electBtn.SetActive(true);
            policeStar.SetActive(false);
        }
        else
        {
            electBtn.SetActive(false);
        }
    }

    void VoteKill()
    {
        //send btn enable
        if (gameControl.hasDown == false && targetIsDead == false && IAmAlive == true)
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
        //TODO 信息发送
        JsonData dataVote = new JsonData();
        dataVote["type"] = "vote";
        dataVote["content"] = new JsonData();
        dataVote["content"]["roomID"] = room.roomID;
        dataVote["content"]["voter"] = myName;
        dataVote["content"]["target"] = targetName;
        string klJson = dataVote.ToJson();
        Debug.Log(klJson);
        wl.Send(klJson);
    }

    void Ending()
    {
        AllSeen();
    }

}
