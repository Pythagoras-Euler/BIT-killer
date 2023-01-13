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
    [SerializeField] Room room;
    [SerializeField] WebLink wl;

    
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

    private string targetName = "";
    private bool targetIsDead = false;
    PlayerAssignment.Character targetCharacter;
    PlayerAssignment.Character myCharacter;
    PlayerAssignment.Character NowMark;

    // Start is called before the first frame update
    void Start()
    {
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        userName = userInfo.username;
        playerAssignment = GameObject.FindGameObjectWithTag("PlayerAssignment").GetComponent<PlayerAssignment>();
        //seatNum = playerAssignment.seatNum;
        playerNum.text = seatNum.ToString();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        gameControl = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();

        NowMark = PlayerAssignment.Character.UNDEF;
        targetCharacter = PlayerAssignment.Character.UNDEF;

        InfoRefresh();

    }

    PlayerAssignment.Character GetChara(string Name)
    {
        string thisnBandIden = gameControl.playerCharacterMap[Name];
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
                break;
        }
    }

    void InfoRefresh()
    {
        playerNum.text = seatNum.ToString();

        targetName = gameControl.players[seatNum - 1];
        playerName.text = targetName;
        myName = playerAssignment.name;
        myCharacter = playerAssignment.playerCharacter;
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
        data1["content"]["voter"] = userName;
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
                if(targetName == gameControl.dayEvent.killed)
                {
                    witchSaveBtn.SetActive(true);
                }
                else
                {
                    witchSaveBtn.SetActive(false);
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
        //TODO ������Ϣ

    }

    public void WitchSave()
    {

        gameControl.hasDown = true;
        witchPan.SetActive(false);
        //TODO ������Ϣ
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
        for(int i = 0; i < 7; i++)
        {
            //if����һ��checked����
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


    public void ProphetExamining()
    {
        gameControl.hasDown = true;
        //TODO ������Ϣ

    }



    //Ŀ�꣨player����û��
    void TargetDeadCheck()
    {
        if(gameControl.playerStateMap[targetName] == true)
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
    }

    void Ending()
    {
        Waiting();
    }

}
