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

    public RetRooms(bool s, string m, int ID, string creator, string roomName, string password, bool gameing, bool full)
    {
        type = "get rooms";
        success = s;
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
public class JoinARoom
{
    public string type;
    public class JoinContent {
        public string user;
        public int roomID;
        public string password;
    }
    public JoinARoom() { }
    public JoinARoom(string t,string u,int id,string p)
    {
        type = t;
        JoinContent content = new JoinContent();
        content.user = u;
        content.roomID = id;
        content.password = p;
    }
}