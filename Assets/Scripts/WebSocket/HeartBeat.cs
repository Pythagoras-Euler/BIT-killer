using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

public class HeartBeat : MonoBehaviour
{

    public WebLink wl;
    public Text DelayDisplay;//�ӳٵ�Ӣ������ʵ��lag�����������ø���
    public Image NetStatusImage;

    public long localTime;
    private long serverTime;
    private long delayTime;

    // Start is called before the first frame update
    void Start()
    {
        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        DelayDisplay.text = "114514ms";
    }

    // Update is called once per frame
    void Update()
    {
        localTime = GetUtcNowTimeStamp();
        serverTime = GetHeartBeat();
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
        //localTime = GetTimeSet();
        localTime = GetUtcNowTimeStamp();
        delayTime = localTime - serverTime;

        DelayDisplay.text = delayTime.ToString() + "ms";
    }

    long GetTimeSet()
    {
        long ts = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
        return ts;
    }



    public static long GetUtcNowTimeStamp()
    {
        //DateTime.UtcNow��ȡ���������׼ʱ���ĵ�ǰʱ�䣨�ȱ���ʱ����8Сʱ��
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (long)ts.TotalMilliseconds;//��ȷ������
    }

}
