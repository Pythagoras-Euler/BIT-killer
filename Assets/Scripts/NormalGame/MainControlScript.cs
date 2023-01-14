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
    [SerializeField] CountDownSlider countDown;

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
    string json;
    JsonData retjson;
    bool sendFlag = false;
    // Update is called once per frame
    void Update()//�׶θ����շ��ŵ�GameControl��
    {

        json = wl.receiveJson;
        Debug.Log(json);
        retjson = JsonMapper.ToObject(json);
        Debug.Log(retjson.ToString());
        Debug.Log(retjson["type"].ToString());

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

                HasVictory(); // �����Ϸ�Ƿ���Ӯ��
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
        if (retjson["type"].ToString() == "join room") // ��֤��Ϣ���ͣ����뷿��
        {
            Debug.Log(retjson["success"].ToString()+":"+ retjson["content"]["roomID"].ToString()+":"+ room.roomID.ToString());
            if (retjson["success"].ToString() == "True" && retjson["content"]["roomID"].ToString() == room.roomID.ToString())// �ɹ����뵱ǰ����
            {
                Debug.Log(retjson["success"].ToString());
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                    Debug.Log(room.players[i]);
                }
                
            }
        }
        if (retjson["type"].ToString() == "create room") // ��֤��Ϣ���ͣ����뷿��
        {
            if (retjson["success"].ToString() == "True" && long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)// �ɹ����뵱ǰ����
            {
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++) {

                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                }
            }
        }
        else if (retjson["type"].ToString() == "leave room") // ��֤��Ϣ���ͣ��˳�����
        {
            if (retjson["success"].ToString() == "True" && long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)// �ɹ��뿪��ǰ����
            {
                int playerCount = retjson["content"]["players"].Count;
                room.playerCount = playerCount;
                gameControl.players = new string[playerCount];
                room.players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                {
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // ������ұ�
                    room.players[i] = gameControl.players[i];
                }
            }
        }
        else if (retjson["type"].ToString() == "game start") // ��֤��Ϣ���ͣ���ʼ��Ϸ
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
                    gameControl.players[i] = retjson["content"]["players"][i].ToString(); // ������ұ�
                    //gameControl.playerCharacterMap[retjson["content"]["players"][i].ToString()] = retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString();
                    gameControl.playerCharacterMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString());
                    Debug.Log(retjson["content"]["players"][i].ToString() + ":" + retjson["content"]["playerCharacterMap"][retjson["content"]["players"][i].ToString()].ToString());
                    //gameControl.playerStateMap[retjson["content"]["players"][i].ToString()] = retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "true" : "false";
                    gameControl.playerStateMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "true" : "false");
                }
                multyBtn.SetActive(false);
                gameControl.gameState = GameControl.GameState.START;// ������Ϸ״̬
                return;
            }
        }
        bool everyoneReady = false;
        if (gameControl.players.Length == 7)
        { everyoneReady = true; }

        if (everyoneReady)//������׼��
        {
            if(room.iscurcreator)
                multyBtn.SetActive(true);

            // ����������������Կ�ʼ��Ϸ����ֵ
            //sr = new StreamReader(Application.dataPath + "/jsontest.txt");
            //json = sr.ReadToEnd().TrimEnd('\0');
            //Debug.Log(json);
            

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

    string CharacterInChinese()
    {
        string chinese = "������";
        switch (playerAssignment.playerCharacter)
        {
            case PlayerAssignment.Character.VILLAGE:
                chinese = "��ͨѧ��";
                break;
            case PlayerAssignment.Character.PROPHET:
                chinese= "������Ա";
                break;
            case PlayerAssignment.Character.WITCH:
                chinese= "�ο���ʦ";
                break;
            case PlayerAssignment.Character.WOLF:
                chinese =  "����";
                break;
        }
        return chinese;
    }

    //�ַ������,���Ե���һ�����ڣ�Ҳ��������������һ��ϵͳ��Ϣ��ͬʱ��ʼ���Ծ���Ϣ
    int count = 0;
    void StartGame()
    {
        // 
        SetNight();
        if (!sendFlag)
        {
            sendFlag = !sendFlag;//~sendFlag
            VillagerChatMsg.text += "���ı������Ϊ" + CharacterInChinese() + "\n";

            for(int i =0;i<7;i++)
            {
                if(gameControl.playerCharacterMap[gameControl.players[i]].ToString()=="WOLF" && gameControl.players[i] != userInfo.username)
                {

                    VillagerChatMsg.text += "����ͬ��Ϊ" + gameControl.players[i] + "\n";
                    VillagerChatMsg.text += "����������ѧ����ȫ���ҿưɣ�";
                    break;
                }
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
        if (countDown.countDownSlider == null || countDown.countDownSlider.value == 0)
        {
            // ����ʱ������������һ�׶�
            gameControl.gameState = GameControl.GameState.KILL;
            Debug.Log("Kill");
        }
    
    }

    //
    void SetNight()
    {

        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        //�����ɰ����ɺ�ҹ
    }

    void SetDay()
    {

        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        //�����ɺ�ҹ��ɰ���
    }

    void RetWolfAct()
    {
        SetNight();
        //�ȴ���������ȷ����ϣ�������͸���������
        if(retjson["type"].ToString()=="kill")
        {
            if(retjson["success"].ToString() == "True")
            {
                string target = retjson["content"]["target"].ToString();
                // ���͸���������
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    WolfChatMsg.text += "ϵͳ��ȷ�Ϲҿ���Ϊ"+ target + "\n";
                }

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retjson["type"].ToString()=="finish")
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString()=="kill finish")
            {
                VillagerChatMsg.text = "ϵͳ�������Ѳ������" + "\n";
                gameControl.gameState = GameControl.GameState.WITCH; // ����Ů�׻���
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    void RetWitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
    {


        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // �����Ů��
        {
            if (retjson["type"].ToString() == "kill result")
            {
                if (retjson["success"].ToString() == "True")
                {
                    string target = retjson["content"]["target"].ToString();
                    VillagerChatMsg.text += "ϵͳ����ҹ�ҿ���Ϊ" + target + "\n";
                }
            }
           
        }
        if (retjson["type"].ToString() == "witch"|| retjson["type"].ToString() == "finish") // ���ؽӿ���û��˵�������߾ȵ�����˭����Ҫ�ͻ���ָ��
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString() == "witch finish")
            {
                VillagerChatMsg.text += "ϵͳ���ο���ʦ�Ѳ������" + "\n";
                gameControl.gameState = GameControl.GameState.PROPHET;// Ů�׻��ڽ���������Ԥ�Լһ��ڡ�
                gameControl.hasDown = false;
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }    

    void RetProphetAct()
    {


        if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // �����Ԥ�Լ�
        {
            if (retjson["type"].ToString() == "prophet")
            {
                if (retjson["success"].ToString() == "True")
                {
                    string target = retjson["content"]["target"].ToString();
                    string chara = retjson["content"]["character"].ToJson() == "WOLF" ? "���˹ҿƵ���" : "���˺úù������";
                    VillagerChatMsg.text += "ϵͳ������Ŀ��"+target+"Ϊ" + chara + "\n";
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retjson["content"]["character"].ToJson();
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
        SetDay();
        //������͸�������

        
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
        if (retjson["type"].ToString() == "game end") // ��֤��Ϣ���ͣ���Ϸ����
        { 
            if(retjson["success"].ToString() == "True"&&long.Parse(retjson["content"]["roomID"].ToString()) == room.roomID)
            {
                if(retjson["winner"].ToString() == "WOLF") // ����ʤ��
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

    void ElectPolice()
    {
        if(retjson["type"].ToString() == "elect")
        {
            if(retjson["success"].ToString()=="True")
            {
                string result = retjson["content"]["result"].ToString();
                VillagerChatMsg.text += "ϵͳ��ѡ�پ���Ϊ" + result +"\n";
                gameControl.dayEvent.nowPolice = result;
                for(int i = 0; i < retjson["content"]["voterTargetMap"].Count;i++)
                {
                    VillagerChatMsg.text += retjson["content"]["voterTargetMap"][i].Keys.ToString() + "Ͷ����" + retjson["content"]["voterTargetMap"][i].ToString()+"\n";
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
