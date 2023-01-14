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
    public GameObject multyBtn; // ֻ�з����У�������Ϸ��ť
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

    bool clare = false; // ������û�з�����Ѷ

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
        minWaitTime = 0;//TODO һ�����ʵ�ֵ��ȡ���ڼ�ʱ��
        maxWaitTime = 9999;//ͬ��
        multyBtn.SetActive(false);

        Waiting();
        
    }

    // Update is called once per frame
    void Update()//�׶θ����շ��ŵ�GameControl��
    {
        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        switch (gameControl.gameState)//�ж����ڵĽ׶β�ִ����Ӧ����
        {
            case GameControl.GameState.WAIT:
                Debug.Log("wait");
                Waiting();
                break;
            case GameControl.GameState.START://�ַ������
                StartGame();
                break;
            case GameControl.GameState.KILL: // �����ж�
                RetWolfAct();
                break;
            case GameControl.GameState.WITCH:
                RetWitchAct();
                break;
            case GameControl.GameState.PROPHET: // Ԥ�Լ��ж�
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
            case GameControl.GameState.END://���㣬����ʤ��
                
                break;
        }
    }

    


    //�ȴ�ҳ�棬������icon����Ϊ������
    void Waiting()
    {
        SetDay();//TODO

        // ��������Ϣ
        string json = wl.receiveJson;
        Debug.Log(json);
        JsonData retnewjoinroom = JsonMapper.ToObject(json);
        if (retnewjoinroom["type"].ToString() == "join room") // ��֤��Ϣ���ͣ����뷿��
        {
            Debug.Log(retnewjoinroom["success"].ToString()+":"+ retnewjoinroom["content"]["roomID"].ToString()+":"+ room.roomID.ToString());
            if (retnewjoinroom["success"].ToString() == "True" && retnewjoinroom["content"]["roomID"].ToString() == room.roomID.ToString())// �ɹ����뵱ǰ����
            {
                Debug.Log(retnewjoinroom["success"].ToString());
                int playerCount = retnewjoinroom["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                    Debug.Log(room.players[i]);
                }
                
            }
        }
        if (retnewjoinroom["type"].ToString() == "create room") // ��֤��Ϣ���ͣ����뷿��
        {
            if (retnewjoinroom["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)// �ɹ����뵱ǰ����
            {
                int playerCount = retnewjoinroom["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++) {

                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                }
            }
        }
        else if (retnewjoinroom["type"].ToString() == "leave room") // ��֤��Ϣ���ͣ��˳�����
        {
            if (retnewjoinroom["success"].ToString() == "True" && long.Parse(retnewjoinroom["content"]["roomID"].ToString()) == room.roomID)// �ɹ��뿪��ǰ����
            {
                int playerCount = retnewjoinroom["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retnewjoinroom["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                }
            }
        }

        bool everyoneReady = false;
        if (gameControl.players.Length == 7)
        { everyoneReady = true; }

        if (everyoneReady)//������׼��
        {
            multyBtn.SetActive(true);

            // ����������������Կ�ʼ��Ϸ����ֵ
            //sr = new StreamReader(Application.dataPath + "/jsontest.txt");
            //json = sr.ReadToEnd().TrimEnd('\0');
            //Debug.Log(json);
            json = wl.receiveJson;
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

    //�ַ������,���Ե���һ�����ڣ�Ҳ��������������һ��ϵͳ��Ϣ��ͬʱ��ʼ���Ծ���Ϣ
    void StartGame()
    {
        // 
        SetNight();

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
        //�ȴ���������ȷ����ϣ�������͸���������
        string json = wl.receiveJson;
        Debug.Log(json);
        JsonData retJson = JsonMapper.ToObject(json);
        if(retJson["type"].ToString()=="kill")
        {
            if(retJson["success"].ToString() == "True")
            {
                string target = retJson["content"]["target"].ToString();
                // ���͸���������
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    WolfChatMsg.text += "ϵͳ��ȷ�Ϲҿ���Ϊ"+ target + "\n";
                }

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retJson["type"].ToString()=="finish")
        {
            if (retJson["success"].ToString() == "True" && retJson["message"].ToString()=="kill finish")
            {
                VillagerChatMsg.text = "ϵͳ�������Ѳ������" + "\n";
                gameControl.gameState = GameControl.GameState.WITCH; // ����Ů�׻���
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retJson["message"].ToString());
            }
        }
    }

    void RetWitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
    {

        string json = wl.receiveJson;
        //Debug.Log(json);
        JsonData retJson = JsonMapper.ToObject(json);

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // �����Ů��
        {
            if (retJson["type"].ToString() == "kill result")
            {
                if (retJson["success"].ToString() == "True")
                {
                    string target = retJson["content"]["target"].ToString();
                    VillagerChatMsg.text += "ϵͳ����ҹ�ҿ���Ϊ" + target + "\n";
                }
            }
           
        }
        if (retJson["type"].ToString() == "witch"|| retJson["type"].ToString() == "finish") // ���ؽӿ���û��˵�������߾ȵ�����˭����Ҫ�ͻ���ָ��
        {
            if (retJson["success"].ToString() == "True" && retJson["message"].ToString() == "witch finish")
            {
                VillagerChatMsg.text += "ϵͳ���ο���ʦ�Ѳ������" + "\n";
                gameControl.gameState = GameControl.GameState.PROPHET;// Ů�׻��ڽ���������Ԥ�Լһ��ڡ�
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retJson["message"].ToString());
            }
        }
    }    

    void RetProphetAct()
    {

        string json = wl.receiveJson;
        //Debug.Log(json);
        JsonData retJson = JsonMapper.ToObject(json);

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // �����Ԥ�Լ�
        {
            if (retJson["type"].ToString() == "prophet")
            {
                if (retJson["success"].ToString() == "True")
                {
                    string target = retJson["content"]["target"].ToString();
                    string chara = retJson["content"]["character"].ToJson() == "WOLF" ? "���˹ҿƵ���" : "���˺úù������";
                    VillagerChatMsg.text += "ϵͳ������Ŀ��"+target+"Ϊ" + chara + "\n";
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retJson["content"]["character"].ToJson();
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
        
        //TODO ���� ���� ����

        //
    }
    void DiscussAct()
    {
        //������͸�������

        string json = wl.receiveJson;
        //Debug.Log(json);
        JsonData retjson = JsonMapper.ToObject(json);
        
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

                // TODO: VillagerChatMsg+="ϵͳ������ҿƵ�����" + gameControl.dayEvent.killed + "��"

                if(retjson["content"]["electCaptain"].ToString()=="True")
                {
                    // Ʊѡ����
                    gameControl.gameState = GameControl.GameState.ELECT;
                    gameControl.hasDown = false;
                }
                else
                {
                    // TODO:������
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
        // ��������Ϣ

        string json = wl.receiveJson;
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
        else
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

        string json = wl.receiveJson;
        //]Debug.Log(json);
        JsonData retJson = JsonMapper.ToObject(json);
        if(retJson["type"].ToString() == "elect")
        {
            if(retJson["success"].ToString()=="True")
            {
                string result = retJson["content"]["result"].ToString();
                VillagerChatMsg.text += "ϵͳ��ѡ�پ���Ϊ" + result +"\n";
                gameControl.dayEvent.nowPolice = result;
                for(int i = 0; i < retJson["content"]["voterTargetMap"].Count;i++)
                {
                    VillagerChatMsg.text += retJson["content"]["voterTargetMap"][i].Keys.ToString() + "Ͷ����" + retJson["content"]["voterTargetMap"][i].ToString()+"\n";
                }
                gameControl.gameState = GameControl.GameState.DISCUSS;// �������ۻ���
                gameControl.hasDown = false;

            }
        }
        
    }

    void VotePolice()
    {
        //TODO ����ͶƱ��Ϣ
    }


    void VoteKill()
    {
        //send btn enable
    }

    void Ending()
    {
        //��ʾһ��ʤ�߱�
    }

}
