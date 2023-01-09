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

    //��ȡȫ�������б�
    public void GetAllRooms()
    {
        // ��������
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        wl.Send(getRoomsJson);
        // TODO:���ݷ�����Ϣ���·����б�
    }

    //�������������
    public void OpenSearchRoomPannel()
    {
        searchRoomPannel.SetActive(true);
    }

    //�ر������������
    public void CloseSearchRoomPannel()
    {
        searchRoomPannel.SetActive(false);
    }



    //������뷿��
    public void RandomJoin()
    {
        //���������������

        //���շ���ֵ
        bool retSuc = true;

        if(retSuc == true)
        {
            JoinRoom();//TODO:��ȡ������Ϣ�Ĳ�����û�ҵ����ģ��������Ǹ�searchroomô��
        }
        else
        {
            CreateRoom();//�Ҳ�������ʹ�������
        }
        
    }

    //���������¼�
    public void CreateRoom()
    {
        // �����������ÿ�
        createRoomPannel.SetActive(true);
    }

    //���뷿���¼�
    public void JoinRoom()
    {
        // TODO:����������Ϣ��ѯ���Ƿ����
        joinRoomPannel.SetActive(true);
        // TODO:�����������join room����
        // TODO:��������Ϣ
    }
}
