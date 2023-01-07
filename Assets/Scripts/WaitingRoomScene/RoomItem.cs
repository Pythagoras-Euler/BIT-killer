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

    public int roomID;
    public string roomPassword;

    [SerializeField] GameObject roomNameField;
    [SerializeField] GameObject creatorField;
    [SerializeField] GameObject playerCountField;
    [SerializeField] GameObject hasPasswordHint;
    [SerializeField] GameObject gamingHint;
    [SerializeField] GameObject fullHint;

    [SerializeField] GameObject joinRoomPannel;
    [SerializeField] GameObject joinBtn;

    // Start is called before the first frame update
    void Start()
    {
        joinRoomPannel = GameObject.Find("Canvas").transform.Find("JoinRoomPannel").gameObject;
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
        }
        // TODO:是否正在游戏显示不同图标
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
        joinRoomPannel.SetActive(true);
    }
}
