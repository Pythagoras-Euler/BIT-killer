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
        
    }
    
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

        // TODO:处理返回消息
        // 放一个jsontest.txt测试
        StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        string json = sr.ReadToEnd();
        Debug.Log(json);
        JsonData retgetaroom = JsonMapper.ToObject(json);
        Debug.Log(retgetaroom["success"]);
        if (retgetaroom["success"].ToString() == "True")//TODO 梳理一下这个和Searhroom.cs.Srarch()是什么关系
        {
            //TODO:找到房间询问是否加入
        }
        else
        {
            //TODO:显示查找失败信息
        }
    }

}
