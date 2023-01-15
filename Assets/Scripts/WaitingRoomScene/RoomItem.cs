using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public string roomName;
    public string creator;
    public int playerCount;
    public bool hasPassword;
    public bool gaming;
    public bool full;

    public long roomID;
    public string roomPassword;

    public string[] players;

    [SerializeField] GameObject roomNameField;
    [SerializeField] GameObject creatorField;
    [SerializeField] GameObject playerCountField;
    [SerializeField] GameObject hasPasswordHint;
    [SerializeField] GameObject lockIcon;
    [SerializeField] GameObject gamingHint;
    [SerializeField] GameObject fullHint;

    [SerializeField] GameObject joinRoomPannel;
    [SerializeField] GameObject joinBtn;
    [SerializeField] WebLink wl;

    // Start is called before the first frame update
    void Start()
    {
        joinRoomPannel = GameObject.Find("Canvas").transform.Find("JoinRoomPannel").gameObject;
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        Debug.Log("find");
    }

    // Update is called once per frame
    void Update()
    {
        roomNameField.GetComponent<Text>().text = roomName;
        creatorField.GetComponent<Text>().text = creator;
        playerCountField.GetComponent<Text>().text = playerCount.ToString()+"/7";
        // TODO:有密码和无密码不同颜色显示or加图标
        if(hasPassword)
        {
            hasPasswordHint.GetComponent<Text>().text = "需要密码";
            lockIcon.SetActive(true);//加锁形图标

        }
        // TODO:是否正在游戏显示不同图标or加粗
        if (gaming)
        {
            gamingHint.GetComponent<Text>().text = "游戏中";
            joinBtn.SetActive(false);
        }
        // TODO:是否满员也采用不同图标
        if (full)
        {
            fullHint.GetComponent<Text>().text = "已满";
            joinBtn.SetActive(false);
        }
    }
    public void JoinRoom()
    {
        joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = roomID;
        joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = hasPassword;
        // 发送一个获取特定房间的请求
        JsonData data1 = new JsonData();
        data1["type"] = "get a room";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = roomID;
        string srJson = data1.ToJson();
        wl.Send(srJson);
        joinRoomPannel.SetActive(true);
    }
}
