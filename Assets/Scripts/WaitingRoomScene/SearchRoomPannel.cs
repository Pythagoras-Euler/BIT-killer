using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SearchRoomPannel : MonoBehaviour
{
    [SerializeField] GameObject searchFieldText;
    [SerializeField] GameObject RetMsg;
    [SerializeField] GameObject joinRoomPannel;
    public WebLink wl;

    public int searchID;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
    }

    // Update is called once per frame
    void Update()
    {

        // 处理返回消息
        // 放一个jsontest.txt测试
        // StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        // string json = sr.ReadToEnd();
        // Debug.Log(json);
        JsonData retgetaroom = JsonMapper.ToObject(wl.receiveJson);
        // Debug.Log(retgetaroom["success"]);
        if (retgetaroom["success"].ToString() == "True") // 查找成功
        {
            //TODO:找到房间询问是否加入
            if (int.Parse(retgetaroom["content"]["playerCount"].ToString()) >= 7) //房间已满
            {
                joinRoomPannel.GetComponent<JoinRoomPannel>().retMsg.GetComponent<Text>().text = "房间已满";
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = int.Parse(retgetaroom["content"]["roomID"].ToString());
                if (retgetaroom["content"]["password"].ToString() == "")
                {
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = false;
                }
                else
                {
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = true;
                }
                joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = false;
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomOwner = retgetaroom["content"]["creator"].ToString();
                joinRoomPannel.GetComponent<JoinRoomPannel>().memberCount = retgetaroom["content"]["players"].Count;
                joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers = new string[retgetaroom["content"]["players"].Count];
                for (int i = 0; i < retgetaroom["content"]["players"].Count; i++)
                {
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers[i] = retgetaroom["content"]["players"][i].ToString();

                }
                joinRoomPannel.SetActive(true);
            }
            else // 房间未满
            {
                joinRoomPannel.GetComponent<JoinRoomPannel>().retMsg.GetComponent<Text>().text = "可加入";
                if (retgetaroom["content"]["password"].ToString() == "")
                {
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = false;
                    //加入确认（无密码）
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = int.Parse(retgetaroom["content"]["roomID"].ToString());
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = false; // 无密码
                    joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = true;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomOwner = retgetaroom["content"]["creator"].ToString();
                    joinRoomPannel.GetComponent<JoinRoomPannel>().memberCount = retgetaroom["content"]["players"].Count;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers = new string[retgetaroom["content"]["players"].Count];
                    for (int i = 0; i < retgetaroom["content"]["players"].Count; i++)
                    {
                        joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers[i] = retgetaroom["content"]["players"][i].ToString();

                    }
                    joinRoomPannel.SetActive(true);
                }
                else
                {
                    //加入确认（有密码）
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = true;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = int.Parse(retgetaroom["content"]["roomID"].ToString());
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = true;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().canJoin = true;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomOwner = retgetaroom["content"]["creator"].ToString();
                    joinRoomPannel.GetComponent<JoinRoomPannel>().memberCount = retgetaroom["content"]["players"].Count;
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers = new string[retgetaroom["content"]["players"].Count];
                    for (int i = 0; i < retgetaroom["content"]["players"].Count; i++)
                    {
                        joinRoomPannel.GetComponent<JoinRoomPannel>().roomMembers[i] = retgetaroom["content"]["players"][i].ToString();

                    }
                    joinRoomPannel.SetActive(true);
                }
            }
        }
        else
        {
            RetMsg.GetComponent<Text>().text = "查找失败";
        }
    }
    
    // 搜索房间
    public void searchBtn() 
    {
        searchID =int.Parse (searchFieldText.GetComponent<Text>().text);
        JsonData data1 = new JsonData();
        data1["type"] = "get a room";
        data1["content"] = new JsonData();
        data1["content"]["roomID"] = searchID;
        string srJson = data1.ToJson();
        Debug.Log(srJson);
        wl.Send(srJson);

    }
}
