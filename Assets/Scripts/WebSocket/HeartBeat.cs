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

    void GetHeartBeat()
    {
        JsonData retuser = JsonMapper.ToObject(wl.receiveJson);
        if (retuser["type"].ToString() == "heartbeat") // 如果是登录 
        {
            //TODO 怎么读取time,没弄明白
        }
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
