using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class HeartBeat : MonoBehaviour
{

    public WebLink wl;
    public Text DelayDisplay;
    public Image NetStatusImage;

    private long localTime;
    private long serverTime;
    private long delayTime;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        DelayDisplay.text = "9999ms";
    }

    // Update is called once per frame
    void Update()
    {
        GetHeartBeat();
        SpeedCheck();
    }

    long GetHeartBeat()
    {
        JsonData retMsg = JsonMapper.ToObject(wl.receiveJson);
        long heartBeat = 0;
        if (retMsg["type"].ToString() == "heartbeat") // �����������
        {
            //TODO ��ô��ȡtime,ûŪ����
            if(retMsg["success"].ToString()=="True")
            {
                heartBeat = long.Parse(retMsg["content"]["time"].ToString());
            }
        }
        return heartBeat;//����������������ȡʧ��
        }

    void SpeedCheck()
    {
        localTime = GetTimeSet();
        delayTime = localTime - serverTime;

        DelayDisplay.text = delayTime.ToString();
    }

    long GetTimeSet()
    {
        long ts = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        return ts;
    }

}
