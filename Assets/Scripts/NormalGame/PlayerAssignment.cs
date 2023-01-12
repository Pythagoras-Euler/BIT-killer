using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    PlayerAssignment �и���Ӻ�˻�ȡ��ҵ���λ�š��ǳơ����
 */
public class PlayerAssignment : MonoBehaviour
{
    [SerializeField] Room curroom;

    // ��ҹ����ʾPanel���ݵ�ǰ�����ݾ�������������seer�������������ֻ��ʾBasicInfoPanel��seerPanel��seerPanel����Ԥ�ԼҿɲٿصĹ������
    //����д��MainConlrol����()                      --Liu
    public enum Character // ������
    {
        VILLAGE, WOLF, PROPHET, WITCH, UNDEF
    }
    public Character playerCharacter;
    public int seatNum;
    public string playerName;
    public bool playerState;//true == ����?
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
        // TODO:�ȴ��������
    }
}
