using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

public class HeartBeat : MonoBehaviour
{

    public WebLink wl;
    public Text DelayDisplay;//延迟的英文名其实是lag，但是我懒得改了
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
        if (retMsg["type"].ToString() == "heartbeat") // 如果是心跳包
        {
            //TODO 怎么读取time,没弄明白
            if(retMsg["success"].ToString()=="True")
            {
                heartBeat = long.Parse(retMsg["content"]["time"].ToString());
            }
        }
        return heartBeat;//如果不是心跳包或读取失败
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
        //DateTime.UtcNow获取的是世界标准时区的当前时间（比北京时间少8小时）
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (long)ts.TotalMilliseconds;//精确到毫秒
    }

}
