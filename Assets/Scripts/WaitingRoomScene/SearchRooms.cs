using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.IO;

public class SearchRooms : MonoBehaviour
{
    public string roomName;
    public string creator;
    public int playerCount;
    public bool hasPassword;
    public bool gaming;
    public bool full;
    public int playerLimit = 7;
    public string roomPassword;

    public WebLink wl;
    public GameObject searchRoomPannel;
    [SerializeField] Text searchIdText;
    [SerializeField] GameObject createRoomPannel;
    [SerializeField] GameObject joinRoomPannel;
    [SerializeField] GameObject roomItem;

    private Text retMsgDisplay;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        GetAllRooms();
        //retMsgDisplay = transform.Find("RetMsg").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        // TODO:���ݷ�����Ϣ���·����б�
            JsonData retmsg = JsonMapper.ToObject(wl.receiveJson);

        if (retmsg["type"].ToString() == "get rooms")
        {
            if (retmsg["success"].ToString() == "True")
            {
                foreach (JsonData roomjson in retmsg["content"])
                {
                    GameObject newroom = GameObject.Instantiate(roomItem) as GameObject;
                    newroom.transform.parent = GameObject.FindGameObjectWithTag("RoomsView").transform;
                    RoomItem newroomitem = newroom.GetComponent<RoomItem>();
                    newroomitem.roomID = long.Parse(roomjson["roomID"].ToString());
                    newroomitem.creator = roomjson["creator"].ToString();
                    newroomitem.roomName = roomjson["roomName"].ToString();
                    newroomitem.roomPassword = roomjson["password"].ToString();
                    newroomitem.hasPassword = newroomitem.roomPassword == "" ? false : true;
                    newroomitem.playerCount = int.Parse(roomjson["playerCount"].ToString());
                    newroomitem.players = new string[roomjson["players"].Count];
                    for (int i = 0; i < roomjson["players"].Count; i++)
                    {
                        newroomitem.players[i] = roomjson["players"][i].ToString();

                    }
                    newroomitem.gaming = roomjson["gaming"].ToString() == "True" ? true : false;
                    newroomitem.full = roomjson["full"].ToString() == "True" ? true : false;

                }
                wl.receiveJson = "";
            }
            else
            {
                Debug.Log("��ȡȫ������ʧ��");
            }
        }
        else if (retmsg["type"].ToString() == "get a random room") // ��֤��Ϣ����
        {

            bool retSuc = retmsg["success"].ToString() == "True" ? true : false;

            if (retSuc == true)
            {
                JoinRoomPannel jrp = joinRoomPannel.GetComponent<JoinRoomPannel>();
                jrp.roomID = long.Parse(retmsg["content"]["roomID"].ToString());
                jrp.roomName = retmsg["content"]["roomName"].ToString();
                jrp.hasPassword = false;
                jrp.roomOwner = retmsg["content"]["creator"].ToString();
                jrp.memberCount = int.Parse(retmsg["content"]["playerCount"].ToString());
                jrp.roomMembers = new string[jrp.memberCount];
                for (int i = 0; i < jrp.memberCount; i++)
                {
                    jrp.roomMembers[i] = retmsg["content"]["players"][i].ToString();
                }
                jrp.canJoin = true;
                if(joinRoomPannel.activeInHierarchy == false)
                {
                    JoinRoom();//TODO:��ȡ������Ϣ�Ĳ�����û�ҵ����ģ��������Ǹ�searchroomô��
                    wl.receiveJson = "";
                }
            }
            else
            {
                if (createRoomPannel.activeInHierarchy == false)
                    CreateRoom();//�Ҳ�������ʹ�������
            }
        }


    }

    //��ȡȫ�������б�
    public void GetAllRooms()
    {
        // ��������
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        wl.Send(getRoomsJson);
    }

    //�������������
    public void OpenSearchRoomPannel()
    {
        searchRoomPannel.SetActive(true);
    }

    //�ر������������
    public void CloseSearchRoomPannel()
    {
        searchRoomPannel.SetActive(false);
    }



    //������뷿��
    public void RandomJoin()
    {
        // ���������������

        JsonData randomjoin = new JsonData();
        randomjoin["type"] = "get a random room";
        randomjoin["content"] = null;
        string jsonstr = randomjoin.ToJson();
        Debug.Log(jsonstr);
        wl.Send(jsonstr);

    }

    //���������¼�
    public void CreateRoom()
    {
        // �����������ÿ�
        createRoomPannel.SetActive(true);
    }

    //���뷿���¼�
    public void JoinRoom()
    {
        // TODO:����������Ϣ��ѯ���Ƿ����
        joinRoomPannel.SetActive(true);
        // TODO:�����������join room����
        // TODO:��������Ϣ
    }
    public void CloseJoinRoomPanel()
    {
        joinRoomPannel.SetActive(false);
    }
}
