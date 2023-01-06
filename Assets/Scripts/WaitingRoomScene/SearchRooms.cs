using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SearchRooms : MonoBehaviour
{
    public WebLink wl;
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
            Dictionary<string, int> content = new Dictionary<string, int>()
            {
                { "roomID",id}
            };
        }
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
        while(true)
        {

        }
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
}
