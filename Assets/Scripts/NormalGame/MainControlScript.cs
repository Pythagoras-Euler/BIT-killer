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
    [SerializeField] ChatPanel chatPanel;
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
    bool killFlag = false;
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
                    gameControl.playerStateMap.Add(retjson["content"]["players"][i].ToString(), retjson["content"]["playerStateMap"][retjson["content"]["players"][i].ToString()].ToString() == "True" ? "True" : "False");
                    
                    playerAssignment.playerCharacter = GetChara();
                    if(playerAssignment.playerCharacter!=PlayerAssignment.Character.WOLF)
                    {
                        chatPanel.TeamBtn.SetActive(false);
                        WolfChatMsg.gameObject.SetActive(false);
                    }
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

    PlayerAssignment.Character GetChara()
    {
        string thisnBandIden = gameControl.playerCharacterMap[userInfo.username].ToString();
        PlayerAssignment.Character thisChe;
        if (thisnBandIden == "VILLAGE")
        {
            thisChe = PlayerAssignment.Character.VILLAGE;
        }
        else if (thisnBandIden == "WOLF")
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

        if (!sendFlag)
        {
            sendFlag = !sendFlag;//~sendFlag
            VillagerChatMsg.text += "���ı������Ϊ" + CharacterInChinese() + "\n";
            if(playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)// �������
                for (int i =0;i<7;i++)
                {
                    if(playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF && gameControl.players[i] != userInfo.username)
                    {

                        VillagerChatMsg.text += "����ͬ��Ϊ" + gameControl.players[i] + "\n";
                        VillagerChatMsg.text += "����������ѧ����ȫ���ҿưɣ�\n";
                        break;
                    }
                }
            else if(playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
            {
                VillagerChatMsg.text += "���˵ļ�����ҿ�ֻ�����һ��֮��\n";
            }
            else if (playerAssignment.playerCharacter == PlayerAssignment.Character.VILLAGE)
            {
                VillagerChatMsg.text += "����ÿһ�̰�\n";
            }
            else if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET)
            {
                VillagerChatMsg.text += "�����̽�������˵����\n";
            }
            else
            {

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
        SetNight();
    
    }

    public bool CountIsDown()
    {
        return countDown.countDownSlider == null || countDown.countDownSlider.value == 0;
    }

    //
    void SetNight()
    {

        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        // �������˽׶�
        if (CountIsDown())
        {
            // ����ʱ������������һ�׶�
            gameControl.gameState = GameControl.GameState.KILL;
            Debug.Log("Kill");
            VillagerChatMsg.text += "<b><color=red>ϵͳ�������ǽ��񲿲���ʱ��</color></b>\n";
            countDown.setCountDown(0, 0, 30);
        }
        //�����ɰ����ɺ�ҹ

    }

    void SetDay()
    {

        HasVictory(); // �����Ϸ�Ƿ���Ӯ��
        //�����ɺ�ҹ��ɰ���
    }

    void RetWolfAct()
    {
        //�ȴ���������ȷ����ϣ�������͸���������
        if(retjson["type"].ToString()=="kill")
        {
            if(retjson["success"].ToString() == "True")
            {
                string target = retjson["content"]["target"].ToString();
                // ���͸���������
                if(playerAssignment.playerCharacter==PlayerAssignment.Character.WOLF)
                {
                    if(killFlag == false)
                    {
                        WolfChatMsg.text += "ϵͳ��ȷ�Ϲҿ���Ϊ" + target + "\n";
                    }
                }

                killFlag = true;

                gameControl.dayEvent.killed = target;
            }
        }
        else if(retjson["type"].ToString()=="finish")
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString()=="kill finish")
            {
                if (CountIsDown())
                {
                    VillagerChatMsg.text += "ϵͳ�������Ѳ������" + "\n";
                    killFlag = false;
                    gameControl.gameState = GameControl.GameState.WITCH; // ����Ů�׻���
                    countDown.setCountDown(0, 0, 30);
                    gameControl.hasDown = false;
                    VillagerChatMsg.text += "<b><color=red>ϵͳ���������ο���ʦ����ʱ��</color></b>\n";
                }
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    bool witchinformed = false;
    bool witchacted = false;
    void RetWitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
    {

        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH) // �����Ů��
        {
            if (retjson["type"].ToString() == "kill result")
            {
                if (retjson["success"].ToString() == "True")
                {
                    if(witchinformed == false)
                    {
                        if (CountIsDown())
                        {
                            string target = retjson["content"]["target"].ToString();
                            VillagerChatMsg.text += "ϵͳ����ҹ�ҿ���Ϊ" + target + "\n";
                            witchinformed = true;

                        }
                    }
                }
            }
           
        }
        if (retjson["type"].ToString() == "witch"|| retjson["type"].ToString() == "finish") // ���ؽӿ���û��˵�������߾ȵ�����˭����Ҫ�ͻ���ָ��
        {
            if (retjson["success"].ToString() == "True" && retjson["message"].ToString() == "witch finish")
            {
                if(witchacted == false)
                {
                    if (CountIsDown())
                    {
                        VillagerChatMsg.text += "ϵͳ���ο���ʦ�Ѳ������" + "\n";
                        gameControl.gameState = GameControl.GameState.PROPHET;// Ů�׻��ڽ���������Ԥ�Լһ��ڡ�
                        gameControl.hasDown = false;
                        witchacted = true;
                        countDown.setCountDown(0, 0, 30);

                        VillagerChatMsg.text += "ϵͳ�����������Ų���ʱ��" + "\n";
                    }
                }
            }
            else
            {
                Debug.Log(retjson["message"].ToString());
            }
        }
    }

    bool proact = false;
    JsonData retjson2;
    bool retjsonFlag = false;
    void RetProphetAct()
    {
        if (retjson["type"].ToString() == "prophet")
        {
            if (retjson["success"].ToString() == "True")
            {
                if (playerAssignment.playerCharacter == PlayerAssignment.Character.PROPHET) // �����Ԥ�Լ�
                {
                    string target = retjson["content"]["target"].ToString();
                    
                    string chara = retjson["content"]["character"].ToString() == "WOLF" ? "���˹ҿƵ���" : "���˺úù������";
                    Debug.Log(retjson["content"]["character"].ToString());
                    if(proact == false )
                    {
                        VillagerChatMsg.text += "ϵͳ������Ŀ��" + target + "Ϊ" + chara + "\n";
                        proact = true;
                    }
                    gameControl.dayEvent.inspected = target;
                    gameControl.dayEvent.checkIden = retjson["content"]["character"].ToJson();
                }



            }

        }
        if (proact == true)
        {
            //��night end
            if (retjson["type"].ToString() == "night end" && retjsonFlag == false)
            {
                retjson2 = retjson;
                retjsonFlag = true;
            }

            if (CountIsDown())
                {
                    VillagerChatMsg.text += "ϵͳ��������Ա�������\n";
                    clare = false;
                    gameControl.gameState = GameControl.GameState.DISCUSS;
                    gameControl.hasDown = false;
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
   public bool daylock = false;
    public bool electlock = false;
    void DiscussAct()
    {
        SetDay();

        
        JsonData retjson0 = retjsonFlag ? retjson2 : retjson;
        Debug.Log(retjson0["type"].ToString());
        //������͸�������
        if(retjson0["type"].ToString() == "night end")
        {
            if(retjson0["success"].ToString()=="True")
            {
                retjsonFlag = false;

                int playerCnt = retjson0["content"]["players"].Count;
                gameControl.players = new string[playerCnt];
                for(int i =0;i<playerCnt;i++)
                {
                    gameControl.players[i] = retjson0["content"]["players"][i].ToString();
                }
                //gameControl.playerCharacterMap = new Hashtable();
                foreach(var player in gameControl.players)
                {
                    gameControl.playerStateMap[player] = retjson0["content"]["playerStateMap"][player].ToString();
                    Debug.Log(player+gameControl.playerStateMap[player]);

                }
                //for (int i = 0; i < retjson0["content"]["playerCharacterMap"].Count; i++)
                //{
                //    gameControl.playerCharacterMap[retjson0["content"]["playerCharacterMap"][i].Keys.ToString()] = retjson0["content"]["players"][i].ToString();
                //}
                gameControl.captain = retjson0["content"]["captain"].ToString();
                if(clare == false)
                {
                    if(gameControl.dayEvent.killed == ""&& gameControl.dayEvent.poisoned != "")
                        VillagerChatMsg.text += "ϵͳ������ҿƵ�����"+gameControl.dayEvent.poisoned + "\n";
                    else if(gameControl.dayEvent.killed != "" && gameControl.dayEvent.poisoned == "")
                        VillagerChatMsg.text += "ϵͳ������ҿƵ�����" + gameControl.dayEvent.killed + "\n";
                    else if(gameControl.dayEvent.killed != "" && gameControl.dayEvent.poisoned != "")
                        VillagerChatMsg.text += "ϵͳ������ҿƵ�����" + gameControl.dayEvent.killed+"��"+ gameControl.dayEvent.poisoned+"\n";
                    else
                    {
                        VillagerChatMsg.text += "ϵͳ��������ƽ��ҹ\n";
                    }
                    clare = true;
                    //countDown.setCountDown(0, 1, 23);
                }

                killFlag = false;
                witchinformed = false;
                witchacted = false;
                proact = false;
                electclarelock = false;

                if (retjson0["content"]["electCaptain"].ToString()=="True")
                {
                    // Ʊѡ����
                    if(electlock == false)
                    {

                        VillagerChatMsg.text += "ϵͳ�������Ǿ���ѡ��ʱ�䣬���λ��������" + "\n";

                        countDown.setCountDown(0, 0, 45);
                        Debug.Log("setCountDown");

                        electlock = true;

                    }
                    else if (electlock == true && CountIsDown())
                    {
                        gameControl.gameState = GameControl.GameState.ELECT;
                        countDown.setCountDown(0, 0, 10);
                        gameControl.hasDown = false;
                    }
                }
                else
                {
                    // TODO:������
                    if(daylock == false)
                    {
                        VillagerChatMsg.text += "ϵͳ����������������ʱ�䣬���λ��������" + "\n";
                        daylock = true;
                        countDown.setCountDown(0, 1, 0);
                    }
                    else if(CountIsDown())
                    {
                        VillagerChatMsg.text += "ϵͳ��������ͶƱ����" + "\n";
                        gameControl.gameState = GameControl.GameState.VOTE;
                        gameControl.hasDown = false;
                        countDown.setCountDown(0, 0, 10);
                    }
                }
            }
        }
        else if ( retjson0["type"].ToString() == "elect")
        {
            Debug.Log(daylock.ToString());
            if (daylock == false)
            {
                VillagerChatMsg.text += "ϵͳ����������������ʱ�䣬���λ��������" + "\n";
                daylock = true;
                countDown.setCountDown(0, 1, 0);
            }
            if (CountIsDown())
            {
                VillagerChatMsg.text += "ϵͳ��������ͶƱ����" + "\n";
                gameControl.gameState = GameControl.GameState.VOTE;
                gameControl.hasDown = false;
                countDown.setCountDown(0, 0, 10);
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
    public  bool electclarelock = false;
    void ElectPolice()
    {
        if(retjson["type"].ToString() == "elect")
        {
            if(retjson["success"].ToString()=="True")
            {
                Debug.Log(electclarelock.ToString());
                if(electclarelock==false)
                {
                    Debug.Log(retjson["content"]["result"].ToString());
                    string result = retjson["content"]["result"].ToString();
                    Debug.Log(result);
                    //for(int i =0;i< 7; i++)
                    //{
                        //if(gameControl.playerStateMap[gameControl.players[i]] == "true")
                        //{
                            //Debug.Log(gameControl.players[i] + ":" + retjson["content"]["voterTargetMap"][gameControl.players[i]].ToString());
                            //VillagerChatMsg.text += "ϵͳ��" + gameControl.players[i] + "Ͷ����" + retjson["content"]["voterTargetMap"][gameControl.players[i]].ToString() + "\n";

                        //}
                    //}
                    VillagerChatMsg.text += "ϵͳ��ѡ�پ���Ϊ" + result + "\n";
                    gameControl.dayEvent.nowPolice = result;
                    electclarelock = true;
                    gameControl.gameState = GameControl.GameState.DISCUSS;// �������ۻ���
                    gameControl.hasDown = false;
                    // countDown.setCountDown(0, 1, 0);
                }
            }
        }
        
    }

    public bool voteclare = false;
    void VoteKill()
    {
        // ����ͶƱ����ֵ
        if(retjson["type"].ToString()=="vote")
        {
            if(retjson["success"].ToString()=="True")
            {
                if(retjson["content"]["tie"].ToString()=="True")
                {
                    VillagerChatMsg.text += "ϵͳ��ƽƱ�����·���" + "\n";
                }
                else
                {
                    string voteName = retjson["content"]["result"].ToString();
                    if(voteclare == false)
                    {
                        VillagerChatMsg.text += "ϵͳ��ͶƱ���Ϊ"+ voteName + "\n";
                        gameControl.playerStateMap[voteName] = "false";
                        voteclare = true;
                    }
                }
                SetNight();
            }
        }
    }

    void Ending()
    {
        //��ʾһ��ʤ�߱�
    }

}


/**
 *  _ooOoo_
 * o8888888o
 * 88" . "88
 * (| -_- |)
 *  O\ = /O
 * ___/`---'\____
 * .   ' \\| |// `.
 * / \\||| : |||// \
 * / _||||| -:- |||||- \
 * | | \\\ - /// | |
 * | \_| ''\---/'' | |
 * \ .-\__ `-` ___/-. /
 * ___`. .' /--.--\ `. . __
 * ."" '< `.___\_<|>_/___.' >'"".
 * | | : `- \`.;`\ _ /`;.`/ - ` : | |
 * \ \ `-. \_ __\ /__ _/ .-` / /
 * ======`-.____`-.___\_____/___.-`____.-'======
 * `=---='
 *          .............................................
 *           ��Ի��bug���ģ�����̱����
 */