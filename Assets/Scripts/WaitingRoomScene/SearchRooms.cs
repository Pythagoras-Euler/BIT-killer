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
        
        // TODO���ȴ�ʱ�䳬ʱ
        // if (waittime >= 1000)
        // { timeout }else
        
        // TODO:���Ҳ���ֱ�������ʾ��Ϣ�����ҵ��򵯴�
        if (retRoom.success)
        {
            // TODO:��������ֱ�������Ƿ����
            if(retRoom.content[0].password == "")
            {
                //����ȷ�ϣ������룩
            }
            // TODO:������������������������ȷֱ�Ӽ��룬������ʾ����
            else
            {
                //����ȷ�ϣ������룩
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
            JoinRoom();//TODO:��ȡ������Ϣ�Ĳ�����û�ҵ����ģ���
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
