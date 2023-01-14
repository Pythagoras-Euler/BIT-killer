using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBand : MonoBehaviour
{
    [SerializeField] UserInfo userInfo;// ��������
    [SerializeField] PlayerAssignment playerAssignment;// ��ſͻ��������ݺ���λ�ŵ���Ϣ
    [SerializeField] string userName;
    [SerializeField] int seatNum;
    [SerializeField] GameControl gameControl;
    public Room room;
    public WebLink wl;

    
    public bool needRefreshFlag = false;//��һ��Ҫ���ǿ��ˢ�£������޸����Flag
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


        switch(gameControl.gameState)//�ж����ڵĽ׶β�ִ����Ӧ����
        {
            case GameControl.GameState.WAIT://�ȴ���ʼ
                Waiting();
                InfoRefresh();
                break;
            case GameControl.GameState.START://�ַ������
                StartGame();
                InfoRefresh();
                break;
            case GameControl.GameState.KILL://���˻غ�
                WolfAct();
                break;
            case GameControl.GameState.PROPHET://Ԥ�Լһغ�
                ProphetAct();
                break;
            case GameControl.GameState.WITCH://Ů�׻غ�
                WitchAct();
                break;
            case GameControl.GameState.DISCUSS://���ۻ���
                break;
            case GameControl.GameState.ELECT://ѡ�پ���
                ElectPolice();
                break;
            case GameControl.GameState.VOTE://ͶƱ����
                VoteKill();
                break;
            case GameControl.GameState.WORDS://���Ի���
                LastWords();
                break;
            case GameControl.GameState.END://���㣬����ʤ��
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

    //�ȴ�ҳ�棬������icon����Ϊ������
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

    //�ַ������,���Ե���һ�����ڣ�Ҳ��������������һ��ϵͳ��Ϣ
    void StartGame()
    {
        NowMark = PlayerAssignment.Character.UNDEF;

        InfoRefresh();

        targetCharacter = GetChara(targetName);

        myCharacter = GetChara(myName);
    }

    void MyInfo()//������Ұ
    {
        ISeen();

        if(myCharacter == PlayerAssignment.Character.WITCH)//Ů����Ұ
        {
            WolfSeen();
        }
        else if(myCharacter == PlayerAssignment.Character.PROPHET)//������Ұ
        {
            ProphetSeen();
        }

        if(IAmAlive == false)//������Ұ
        {
            AllSeen();
        }
    }
    void IdenIconDisplay(bool displayGod = false)//TODO ��ʾ��ȷ��IdenIcon,true��ʾ������
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

    void WolfAct()//TODO �����ǲ���Ӧ���ܿ�������Ͷ��˭
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (gameControl.hasDown == false)
            {
                if (playerAssignment.playerState == true)
                {
                    wolfPan.SetActive(true);
                    //TODO ��Ӧ����һ������ʱģ��
                }
                else//��ʾ��Ϣ���ǰ�ť��Ч //û������ͶƱ��ʾ֮ǰû��
                {
                    //TODO
                }
            }
        }
    }

    //BTN��Ӧ�¼�
    public void WolfVote()
    {
        wolfPan.SetActive(false);
        gameControl.hasDown = true;
        //TODO ����ʱ��ʱֹͣ������
        //TODO ����ͶƱ��Ϣ
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

    void WitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ TODO IMPORTANT Ů��ֻ�ܶ�һ�Σ�
    {
        if (myCharacter == PlayerAssignment.Character.WITCH && gameControl.hasDown == false)
        {
            if (IAmAlive == true && targetIsDead == false)
            {
                witchPan.SetActive(true);
                if(targetName == gameControl.dayEvent.killed && gameControl.canDo[0] > 0)//Ŀ��Ϊ���غ������ҽ�ҩû�ù�ʱ��ʹ��
                {
                    witchSaveBtn.SetActive(true);
                }
                else
                {
                    witchSaveBtn.SetActive(false);
                }
                if(gameControl.canDo[1] > 0)//��ҩû�ù�ʱ��ʹ��
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
        //TODO ������Ϣ
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
        //TODO ������Ϣ
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
            if (ProphetChecked(targetName))//��Ҫһ��Examined��
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
        //GameControl����ά��һ������ʾ�����������
        //������Ҫ����ˣ�
        bool isChecked = false;
        for (int i = 0; i < 7; i++)
        {
            //if����һ��checked����
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

        //TODO ������Ϣ
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
        //doubtIcon.sprite = ''  ;//TODO ICON��ʾ
        //���¼���ͼƬ

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





    //Ŀ�꣨player����û��
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

    //�Լ���user����û��
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
        //TODO ����ͶƱ��Ϣ
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
        //TODO ��Ϣ����
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
