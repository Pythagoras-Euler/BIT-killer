using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class SearchRooms : MonoBehaviour
{
    public WebLink wl;
    public GameObject searchRoomPannel;
    [SerializeField] Text searchIdText;
    public class GetRooms // 获取所有房间的请求参数
    {
        public string type { get; set; }
        public Dictionary<string, string> content;
        public GetRooms()
        {
            type = "get rooms";
            content = null;
        }
    }
    public class RetRooms // 获取所有房间的返回参数
    {
        public string type { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
 
        public List<Room> content;
        public RetRooms(bool s,string m,int ID,string creator,string roomName,string password,bool gameing,bool full)
        {
            type = "get rooms";
            success = s;
        }
    }
    public class Room
    {
        public int roomID;
        public string creator;
        public string roomName;
        public string password;
        public int playerCount;
        public string[] players;
        public bool gaming;
        public bool full;

        public Room(int id,string c,string rn,string psw,int plc,string[]pl,bool g,bool f)
        {
            roomID = id;
            creator = c;
            roomName = rn;
            password = psw;
            playerCount = plc;
            players = pl;
            gaming = g;
            full = f;
        }
    }
    public class GetTheRoom
    {
        public string type;
        public Dictionary<string, int> content;
        public GetTheRoom(int id)
        {
            type = "get a room";
            content = new Dictionary<string, int>()
            {
                {"roomID",id}
            };
        }
        public GetTheRoom() { }
    }
    public class RetTheRoom
    {
        public string type;
        public bool success;
        public string message;
        public List<Room> content;
    }
    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllRooms()
    {
        // 发送请求
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        wl.Send(getRoomsJson);
    }
    public void OpenSearchRoomPannel()
    {
        searchRoomPannel.SetActive(true);
    }
    public void CloseSearchRoomPannel()
    {
        searchRoomPannel.SetActive(false);
    }
    public void SearchRoom()
    {
        int id =int.Parse(searchIdText.text);
        GetTheRoom gettheRoom = new GetTheRoom(id);
        string gettheRoomJson = JsonMapper.ToJson(gettheRoom);
        Debug.Log(gettheRoomJson);
        wl.Send(gettheRoomJson);

        // TODO:接收返回json，根据返回json处理
        // TODO:查找不到直接输出提示消息，查找到则弹窗
        // TODO:有密码则直接提问是否加入
        // TODO:有密码则输入密码检查正误，正确直接加入，错误提示错误
        // 东西好多啊哼哼哼啊啊啊啊
    }
    public void CreateRoom()
    {
        // TODO:弹出房间设置框，确认即发出json
        // TODO:处理返回json
    }
    public void JoinRoom()
    {
        // TODO:弹出房间信息窗询问是否加入
        // TODO:如果加入则发送join room请求
        // TODO:处理返回消息
    }
}
