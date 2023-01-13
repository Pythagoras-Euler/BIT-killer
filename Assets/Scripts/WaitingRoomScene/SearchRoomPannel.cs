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

        // ��������Ϣ
        // ��һ��jsontest.txt����
        // StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        // string json = sr.ReadToEnd();
        // Debug.Log(json);
        JsonData retgetaroom = JsonMapper.ToObject(wl.receiveJson);
        // Debug.Log(retgetaroom["success"]);
        if (retgetaroom["success"].ToString() == "True") // ���ҳɹ�
        {
            //TODO:�ҵ�����ѯ���Ƿ����
            if (int.Parse(retgetaroom["content"]["playerCount"].ToString()) >= 7) //��������
            {
                joinRoomPannel.GetComponent<JoinRoomPannel>().retMsg.GetComponent<Text>().text = "��������";
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
            else // ����δ��
            {
                joinRoomPannel.GetComponent<JoinRoomPannel>().retMsg.GetComponent<Text>().text = "�ɼ���";
                if (retgetaroom["content"]["password"].ToString() == "")
                {
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = false;
                    //����ȷ�ϣ������룩
                    joinRoomPannel.GetComponent<JoinRoomPannel>().roomID = int.Parse(retgetaroom["content"]["roomID"].ToString());
                    joinRoomPannel.GetComponent<JoinRoomPannel>().hasPassword = false; // ������
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
                    //����ȷ�ϣ������룩
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
            RetMsg.GetComponent<Text>().text = "����ʧ��";
        }
    }
    
    // ��������
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
