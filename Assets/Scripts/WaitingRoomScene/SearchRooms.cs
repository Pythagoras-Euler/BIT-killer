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
        
    }

    //��ȡȫ�������б�
    public void GetAllRooms()
    {
        // ��������
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        //wl.Send(getRoomsJson);


        // TODO:���ݷ�����Ϣ���·����б�
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retallroom = JsonMapper.ToObject(json);
        if(retallroom["type"].ToString() == "get rooms")
        {
            if(retallroom["success"].ToString() == "True")
            {
                foreach(JsonData roomjson in retallroom["content"])
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
                    newroomitem.gaming = roomjson["gaming"].ToString()=="True"?true:false;
                    newroomitem.full = roomjson["full"].ToString() == "True" ? true : false;

                }
            }
            else
            {
                Debug.Log("��ȡȫ������ʧ��");
            }
        }
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
        //wl.Send(jsonstr);

        // ���շ���ֵ
        // ��Ȼ��BOM�����⣬�����ȷ�һ��jsontest.txt������
        //string retrandomjoinstr = wl.receiveJson;
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retrandomjoin = JsonMapper.ToObject(json);

        bool retSuc = retrandomjoin["success"].ToString()=="True"?true:false;

        if(retSuc == true)
        {
            JoinRoomPannel jrp = joinRoomPannel.GetComponent<JoinRoomPannel>();
            jrp.roomID = long.Parse(retrandomjoin["content"]["roomID"].ToString());
            jrp.roomName = retrandomjoin["content"]["roomName"].ToString();
            jrp.hasPassword = false;
            jrp.roomOwner = retrandomjoin["content"]["creator"].ToString();
            jrp.memberCount = int.Parse(retrandomjoin["content"]["playerCount"].ToString());
            jrp.roomMembers = new string[jrp.memberCount];
            for (int i = 0; i < jrp.memberCount; i++)
            {
                jrp.roomMembers[i] = retrandomjoin["content"]["players"][i].ToString();
            }
            jrp.canJoin = true;
            JoinRoom();//TODO:��ȡ������Ϣ�Ĳ�����û�ҵ����ģ��������Ǹ�searchroomô��
        }
        else
        {
            CreateRoom();//�Ҳ�������ʹ�������
        }
        
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
}
