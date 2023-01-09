using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class SearchRooms : MonoBehaviour
{
    public string roomName;
    public string creator;
    public int playerCount;
    public bool hasPassword;
    public bool gaming;
    public bool full;
    public int playerLimit = 7;

    public int roomID;
    public string roomPassword;

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



    //随机加入房间
    public void RandomJoin()
    {
        //发动随机加入请求

        //接收返回值
        bool retSuc = true;

        if(retSuc == true)
        {
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
