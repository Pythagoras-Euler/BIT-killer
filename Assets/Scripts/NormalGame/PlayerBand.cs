using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBand : MonoBehaviour
{
    [SerializeField] UserInfo userInfo;// 存放玩家名
    [SerializeField] PlayerAssignment playerAssignment;// 存放玩家身份和座位号等信息
    [SerializeField] string userName;
    [SerializeField] int seatNum;
    // Start is called before the first frame update
    void Start()
    {
        userInfo = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>();
        userName = userInfo.username;
        playerAssignment = GameObject.Find("PlayerAssignment").GetComponent<PlayerAssignment>();
        seatNum = playerAssignment.seatNum;
    }

    // Update is called once per frame
    void Update()
    {
        // 根据游戏状态和当前客户端玩家身份决定显示哪个panel
    }
}
