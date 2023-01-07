using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public RetRooms(bool s, string m, int ID, string creator, string roomName, string password, bool gameing, bool full)
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

    public Room(int id, string c, string rn, string psw, int plc, string[] pl, bool g, bool f)
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
public class CreateARoom
{
    public string type;
    public Dictionary<string, string> content;
    public CreateARoom() { }
    public CreateARoom(string t, string c, string p, string rn)
    {
        type = t;
        content = new Dictionary<string, string>()
            {
                { "creator", c },
                { "password", p },
                { "roomName", rn }
            };
    }
}
