using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    PlayerAssignment 中负责从后端获取玩家的座位号、昵称、身份
 */
public class PlayerAssignment : MonoBehaviour
{
    [SerializeField] Room curroom;

    // 在夜晚显示Panel根据当前玩家身份决定，如果玩家是seer，所有其他玩家只显示BasicInfoPanel和seerPanel，seerPanel中是预言家可操控的功能组件
    //功能写到MainConlrol里了()                      --Liu
    public enum Character // 玩家身份
    {
        VILLAGE, WOLF, PROPHET, WITCH, UNDEF
    }
    public Character playerCharacter;
    public int seatNum;
    public string playerName;
    public bool playerState;//true == 活着?
    // Start is called before the first frame update
    void Start()
    {
        curroom = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<Room>();
        seatNum = curroom.playerCount;
        playerName = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>().username;
        playerState = true;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO:等待分配身份
    }
}
