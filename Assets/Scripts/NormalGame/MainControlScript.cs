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
        minWaitTime = 0;//TODO һ�����ʵ�ֵ��ȡ���ڼ�ʱ��
        maxWaitTime = 9999;//ͬ��
        multyBtn.SetActive(false);

        playerAssignment = GameObject.Find("/PlayerAssignment").GetComponent<PlayerAssignment>();
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        room = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
    }

    // Update is called once per frame
    void Update()//�׶θ����շ��ŵ�GameControl��
    {
        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        switch (gameControl.gameState)//�ж����ڵĽ׶β�ִ����Ӧ����
        {
            case GameControl.GameState.WAIT:
                Waiting();
                break;
            case GameControl.GameState.START://�ַ������
                StartGame();
                break;
            case GameControl.GameState.KILL: // �����ж�
                RetWolfAct();
                break;
            case GameControl.GameState.PROPHET: // Ԥ�Լ��ж�
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
            case GameControl.GameState.END://���㣬����ʤ��
                
                break;
        }
    }


    //�ȴ�ҳ�棬������icon����Ϊ������
    void Waiting()
    {
        SetDay();//TODO

        // ��������Ϣ
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        Debug.Log(json);
        //string json = wl.receiveJson;
        JsonData retnewjoinroom = JsonMapper.ToObject(json);
        if (retnewjoinroom["type"].ToString() == "join room") // ��֤��Ϣ���ͣ����뷿��
        {
            if(retnewjoinroom["success"].ToString()=="True"&&long.Parse(retnewjoinroom["content"]["roomID"].ToString())==room.roomID)// �ɹ����뵱ǰ����
            {
                int playerCount = retnewjoinroom["content"]["players"].Count;
                gameControl.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
            }
        }
        else if (retnewjoinroom["type"].ToString() == "leave room") // ��֤��Ϣ���ͣ��˳�����
        {
            if (retnewjoinroom["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)// �ɹ��뿪��ǰ����
            {
                int playerCount = int.Parse(retnewjoinroom["content"]["playerCount"].ToString());
                gameControl.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
            }
        }

        bool everyoneReady = false;
        if (gameControl.players.Length == 7)
        { everyoneReady = true; }

        if (everyoneReady)//������׼��
        {
            multyBtn.SetActive(true);

            // ����������������Կ�ʼ��Ϸ����ֵ
            sr = new StreamReader(Application.dataPath + "/jsontest.txt");
            json = sr.ReadToEnd().TrimEnd('\0');
            Debug.Log(json);
            //string json = wl.receiveJson;
            JsonData retStartGame = JsonMapper.ToObject(json);
            if (retStartGame["type"].ToString() == "game start") // ��֤��Ϣ���ͣ���ʼ��Ϸ
            {
                if (retStartGame["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)
                {
                    int playerCount = int.Parse(retnewjoinroom["content"]["playerCount"].ToString());
                    gameControl.players = new string[playerCount];
                    for (int i = 0; i < playerCount; i++) {
                        gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
                        gameControl.playerCharacterMap[retnewjoinroom["content"]["players"][i].ToString()] = retnewjoinroom["content"]["playerCharacterMap"][retnewjoinroom["content"]["players"][i].ToString()].ToString();
                        gameControl.playerStateMap[retnewjoinroom["content"]["players"][i].ToString()] = retnewjoinroom["content"]["playerStateMap"][retnewjoinroom["content"]["players"][i].ToString()].ToString()=="True"?true:false;

                    }
                    gameControl.gameState = GameControl.GameState.START;// ������Ϸ״̬
                }
            }

        }
    }

    public void onClickMultyBtn()
    {
        // ����������Ϳ�ʼ��Ϸ����
        // ����json
        JsonData startJson = new JsonData();
        startJson["type"] = "game start";
        startJson["content"] = new JsonData();
        startJson["content"]["roomID"] = room.roomID;
        string startJsonStr = startJson.ToJson();
        wl.Send(startJsonStr);

    }

    //�ַ������,���Ե���һ�����ڣ�Ҳ��������������һ��ϵͳ��Ϣ
    void StartGame()
    {
        // 
        SetNight();
    }

    //
    void SetNight()
    {
        //�����ɰ����ɺ�ҹ
    }

    void SetDay()
    {
        //�����ɺ�ҹ��ɰ���
    }

    void RetWolfAct()
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (playerAssignment.playerState == true)
            {
                //TODO ��Ӧ����һ������ʱģ��
            }
            else//��ʾ��Ϣ���ǰ�ť��Ч
            {
                //TODO
            }
        }
    }

    //BTN��Ӧ�¼�
    void WolfVote()
    {
        wolfPan.SetActive(false);
        //TODO ����ʱ��ʱֹͣ������
        //TODO ����ͶƱ��Ϣ
    }

    void WitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
        {
            if ( playerAssignment.playerState == true)
            {
                witchPan.SetActive(true);
                //TODO ��Ӧ����һ������ʱģ��
            }
            else
            {
                int waittime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
                //TODO �������ʱ����װ��û��
            }
        }
    }    

    void WitchPison()
    {
        witchPan.SetActive(false);
        //TODO ������Ϣ
    }

    void WitchSave()
    {
        // witchPan.SetActive(false);
        // TODO ������Ϣ
    }

    void ProphetAct()
    {
        // seerPan.SetActive(true);
    }

    void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;
        
        //TODO ���� ���� ����

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
        // ��������Ϣ
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd().TrimEnd('\0');
        Debug.Log(json);
        //string json = wl.receiveJson;
        JsonData retgameend = JsonMapper.ToObject(json);
        if (retgameend["type"].ToString() == "game end") // ��֤��Ϣ���ͣ���Ϸ����
        { 
            if(retgameend["success"].ToString() == "True"&&long.Parse(retgameend["content"]["roomID"].ToString()) == room.roomID)
            {
                if(retgameend["winner"].ToString() == "WOLF") // ����ʤ��
                {
                    // TODO:�������ʤ��UI��ʾ
                    Debug.Log("����ʤ��");
                }
                else
                {
                    // TODO:��Ӻ���ʤ��UI��ʾ
                    Debug.Log("����ʤ��");
                }
            }
        }
            int winNum = 0;//0û��Ӯ��1����Ӯ��2����Ӯ

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
        //TODO ����ͶƱ��Ϣ
    }


    void VoteKill()
    {
        //send btn enable
        votePan.SetActive(true);
    }

    void Ending()
    {
        //��ʾһ��ʤ�߱�
    }

}
