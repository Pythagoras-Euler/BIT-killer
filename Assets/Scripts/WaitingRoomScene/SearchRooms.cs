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
        // TODO:���Ҳ���ֱ�������ʾ��Ϣ�����ҵ��򵯴�
        // TODO:��������ֱ�������Ƿ����
        // TODO:������������������������ȷֱ�Ӽ��룬������ʾ����
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
