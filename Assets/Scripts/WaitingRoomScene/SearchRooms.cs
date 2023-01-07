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
    [SerializeField] GameObject createRoomPannel;
    [SerializeField] GameObject joinRoomPannel;

    private Text retMsgDisplay;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();

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
        wl.Send(getRoomsJson);
        // TODO:根据返回消息更新房间列表
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

    //搜索房间事件
    public void SearchRoom()
    {
        int id =int.Parse(searchIdText.text);
        GetTheRoom gettheRoom = new GetTheRoom(id);
        string gettheRoomJson = JsonMapper.ToJson(gettheRoom);
        Debug.Log(gettheRoomJson);
        wl.Send(gettheRoomJson);

        // TODO:接收返回json，根据返回json处理
        RetTheRoom retRoom = new RetTheRoom();

        //测试用init
        {
            retRoom.type = "join room";
            retRoom.success = true;
            retRoom.message = "Join the room successfully";
            retRoom.content[0].roomID = 1;
            retRoom.content[0].creator = "lbwnb";
            retRoom.content[0].roomName = "test room";
            retRoom.content[0].password = "";
            retRoom.content[0].playerCount = 1;
            retRoom.content[0].players = new string[] { "lbwnb", "lcy", "lgy", "lx" };
            retRoom.content[0].gaming = false;
            retRoom.content[0].full = false;
        }
        
        // TODO：等待时间超时
        // if (waittime >= 1000)
        // { timeout }else
        
        // TODO:查找不到直接输出提示消息，查找到则弹窗
        if (retRoom.success)
        {
            // TODO:无密码则直接提问是否加入
            if(retRoom.content[0].password == "")
            {
                //加入确认（无密码）
            }
            // TODO:有密码则输入密码检查正误，正确直接加入，错误提示错误
            else
            {
                //加入确认（有密码）
            }
        }
        else //找不到该房间
        {
            //NotFound()
            retMsgDisplay.text = "找不到该房间 请检查ID输入";
        }

        // 东西好多啊哼哼哼啊啊啊啊
    }

    //随机加入房间
    public void RandomJoin()
    {
        //发动随机加入请求

        //接收返回值
        bool retSuc = true;

        if(retSuc == true)
        {
            JoinRoom();//TODO:获取房间信息的部分我没找到在哪（）
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
