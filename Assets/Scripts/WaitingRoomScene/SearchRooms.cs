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

    private Text retMsgDisplay;


    public class GetRooms // ��ȡ���з�����������
    {
        public string type { get; set; }
        public Dictionary<string, string> content;
        public GetRooms()
        {
            type = "get rooms";
            content = null;
        }
    }
    public class RetRooms // ��ȡ���з���ķ��ز���
    {
        public string type { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
 
        public List<Room> content;
        public RetRooms(bool s,string m,int ID,string creator,string roomName,string password,bool gameing,bool full)
        {
            type = "get rooms";
            success = s;
        }
    }
    public class Room
    {
        public int roomID;
        public string creator;
        public string roomName;
        public string password;
        public int playerCount;
        public string[] players;
        public bool gaming;
        public bool full;

        public Room(int id,string c,string rn,string psw,int plc,string[]pl,bool g,bool f)
        {
            roomID = id;
            creator = c;
            roomName = rn;
            password = psw;
            playerCount = plc;
            players = pl;
            gaming = g;
            full = f;
        }
    }
    public class GetTheRoom
    {
        public string type;
        public Dictionary<string, int> content;
        public GetTheRoom(int id)
        {
            type = "get a room";
            content = new Dictionary<string, int>()
            {
                {"roomID",id}
            };
        }
        public GetTheRoom() { }
    }
    public class RetTheRoom
    {
        public string type;
        public bool success;
        public string message;
        public List<Room> content;
    }
    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();

        retMsgDisplay = transform.Find("RetMsg").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllRooms()
    {
        // ��������
        GetRooms getRooms = new GetRooms();
        string getRoomsJson = JsonMapper.ToJson(getRooms);
        Debug.Log(getRoomsJson);
        wl.Send(getRoomsJson);
    }
    public void OpenSearchRoomPannel()
    {
        searchRoomPannel.SetActive(true);
    }
    public void CloseSearchRoomPannel()
    {
        searchRoomPannel.SetActive(false);
    }
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
    public void CreateRoom()
    {
        // TODO:�����������ÿ�ȷ�ϼ�����json
        // TODO:������json
    }
    public void JoinRoom()
    {
        // TODO:����������Ϣ��ѯ���Ƿ����
        // TODO:�����������join room����
        // TODO:��������Ϣ
    }
}
