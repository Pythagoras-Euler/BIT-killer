using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBand : MonoBehaviour
{
    [SerializeField] UserInfo userInfo;// ��������
    [SerializeField] PlayerAssignment playerAssignment;// ��������ݺ���λ�ŵ���Ϣ
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
        // ������Ϸ״̬�͵�ǰ�ͻ��������ݾ�����ʾ�ĸ�panel
    }
}
