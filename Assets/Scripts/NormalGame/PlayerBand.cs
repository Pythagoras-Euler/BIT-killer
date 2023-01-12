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
    public MainControlScript mainControl;
    public GameObject multyBtn; // ֻ�з����У�������Ϸ��ť
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
        targetName = gameControl.players[seatNum - 1];
        playerName.text = targetName;

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

        targetCharacter = thisChara();
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
                else//��ʾ��Ϣ���ǰ�ť��Ч
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

    void WitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
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
        //TODO ������Ϣ

    }

    public void WitchSave()
    {

        gameControl.hasDown = true;
        witchPan.SetActive(false);
        //TODO ������Ϣ
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
        //���¼���ͼƬ
    }


    public void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;

        //TODO ���� ���� ����

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
        //TODO ����ͶƱ��Ϣ
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
