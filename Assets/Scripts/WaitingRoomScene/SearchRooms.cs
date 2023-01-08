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

    //���������¼�
    public void SearchRoom()
    {
        int id =int.Parse(searchIdText.text);
        GetTheRoom gettheRoom = new GetTheRoom(id);
        string gettheRoomJson = JsonMapper.ToJson(gettheRoom);
        Debug.Log(gettheRoomJson);
        wl.Send(gettheRoomJson);

        // TODO:���շ���json�����ݷ���json����
        RetTheRoom retRoom = new RetTheRoom();

        //������init
        {
            retRoom.type = "get a room";
            retRoom.success = true;
            retRoom.message = "Get a room successfully";
            retRoom.content[0].roomID = 1;
            retRoom.content[0].creator = "lbwnb";
            retRoom.content[0].roomName = "test room";
            retRoom.content[0].password = "";
            retRoom.content[0].playerCount = 4;
            retRoom.content[0].players = new string[] { "lbwnb", "lcy", "lgy", "lx" };
            retRoom.content[0].gaming = false;
            retRoom.content[0].full = false;
        }
        
        // TODO���ȴ�ʱ�䳬ʱ
        // if (waittime >= 1000)
        // { timeout }else
        
        // TODO:���Ҳ���ֱ�������ʾ��Ϣ�����ҵ��򵯴�
        if (retRoom.success)
        {
            if(retRoom.content[0].playerCount >= playerLimit)//��������
            {
                retMsgDisplay.text = "����������";
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = roomID;
                joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = hasPassword;
                joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = false;
                joinRoomPannel.SetActive(true);
            }
            else 
            // TODO:��������ֱ�������Ƿ����
            if(retRoom.content[0].password == "")
            {
                hasPassword = false;
                //����ȷ�ϣ������룩
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = roomID;
                joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = hasPassword;
                joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = true;
                joinRoomPannel.SetActive(true);
            }
            // TODO:������������������������ȷֱ�Ӽ��룬������ʾ����
            else
            {
                //����ȷ�ϣ������룩
                hasPassword = true;
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = roomID;
                joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = hasPassword;
                joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = true;
                joinRoomPannel.SetActive(true);
            }
        }
        else //�Ҳ����÷���
        {
            //NotFound()
            retMsgDisplay.text = "�Ҳ����÷��� ����ID����";
        }

        // �����öడ�ߺߺ߰�������
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
