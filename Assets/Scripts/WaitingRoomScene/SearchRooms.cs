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
        // TODO:根据返回消息更新房间列表
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
                Debug.Log("获取全部房间失败");
            }
        }
        else if (retmsg["type"].ToString() == "get a random room") // 验证消息类型
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
                    JoinRoom();//TODO:获取房间信息的部分我没找到在哪（是上面那个searchroom么）
                    wl.receiveJson = "";
                }
            }
            else
            {
                if (createRoomPannel.activeInHierarchy == false)
                    CreateRoom();//找不到房间就创建房间
            }
        }


    }

    //获取全部房间列表
    public void GetAllRooms()
    {
        // 发送请求
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        wl.Send(getRoomsJson);
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
        Debug.Log(jsonstr);
        wl.Send(jsonstr);

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
    public void CloseJoinRoomPanel()
    {
        joinRoomPannel.SetActive(false);
    }
}
