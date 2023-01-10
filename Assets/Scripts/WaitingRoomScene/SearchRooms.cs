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

    //获取全部房间列表
    public void GetAllRooms()
    {
        // 发送请求
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        //wl.Send(getRoomsJson);


        // TODO:根据返回消息更新房间列表
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
                Debug.Log("获取全部房间失败");
            }
        }
    }

    //打开搜索房间面板
    public void OpenSearchRoomPannel()
    {
        searchRoomPannel.SetActive(true);
    }

    //关闭搜索房间面板
    public void CloseSearchRoomPannel()
    {
        searchRoomPannel.SetActive(false);
    }



    //随机加入房间
    public void RandomJoin()
    {
        // 发动随机加入请求

        JsonData randomjoin = new JsonData();
        randomjoin["type"] = "get a random room";
        randomjoin["content"] = null;
        string jsonstr = randomjoin.ToJson();
        //wl.Send(jsonstr);

        // 接收返回值
        // 依然有BOM的问题，这里先放一个jsontest.txt做测试
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
            JoinRoom();//TODO:获取房间信息的部分我没找到在哪（是上面那个searchroom么）
        }
        else
        {
            CreateRoom();//找不到房间就创建房间
        }
        
    }

    //创建房间事件
    public void CreateRoom()
    {
        // 弹出房间设置框
        createRoomPannel.SetActive(true);
    }

    //加入房间事件
    public void JoinRoom()
    {
        // TODO:弹出房间信息窗询问是否加入
        joinRoomPannel.SetActive(true);
        // TODO:如果加入则发送join room请求
        // TODO:处理返回消息
    }
}
