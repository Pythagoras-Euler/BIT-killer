using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//（1）、脚本里面定义了一个用来维护倒计时时间的类CountDown，这个类里面声明了倒计时的时分秒，还有总时间和剩余时间这些成员，
//          还有一个更新倒计时的函数UpdateTime。用的时候，只要将时分秒作为实参传进来，构造一个倒计时对象，调用其函数UpdateTime即可，
//          可以通过访问器CountDownTime 获取其剩余时间。
//（2）、进度条的值如何跟倒计时关联？其实就是将这两者做一个映射关系：进度条的值是0到1，倒计时为 0到总时间；
//          映射公式为：value/1.0f=倒计时剩余时间/总时间；即countDownSlider.value = countDown.CountDownTime / (countDown.TotalTime * 1.0f)。
//          这样就可以实现倒计时同步到进度条上显示的效果了。
//（3）、其他详见脚本注释。
//————————————————
//版权声明：本文为CSDN博主「周周的Unity小屋」的原创文章，遵循CC 4.0 BY - SA版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/qq_42437783/article/details/125635560


/// <summary>
/// 倒计时的类
/// </summary>
public class CountDown
{
    public int hour;
    public int minute;
    public int second;

    private int countDownTime;//倒计时剩余时间，秒为单位
    public int CountDownTime { get { return countDownTime; } }
    private int totalTime;//总时间，秒为单位
    public int TotalTime { get { return totalTime; } }

    public CountDown(int _hour, int _minute, int _second)
    {
        hour = _hour;
        minute = _minute;
        second = _second;
        totalTime = 3600 * hour + 60 * minute + second;//计算总时间
        countDownTime = totalTime;
    }

    private float time = 0;//用来控制时间间隔

    /// <summary>
    /// 更新剩余时间，间隔一秒，时间-1
    /// </summary>
    public void UpdateTime()
    {
        time += Time.deltaTime;
        if (time >= 1 && countDownTime != 0)
        {
            countDownTime--;
            time = 0;
        }
    }
}
//
public class CountDownSlider : MonoBehaviour
{
    public Text countDownText;//倒计时文本
    public Slider countDownSlider;//进度条显示
    private Image sliderImg;//进度条填充图
    private CountDown countDown = null;//声明倒计时对象
    public Button startCountDownBtn;//开始倒计时按钮

    void Start()
    {
        countDownSlider.value = 1;
        sliderImg = countDownSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        startCountDownBtn.onClick.AddListener(OnCountDownClick);
    }

    private void OnCountDownClick()//用法实例
    {
        countDown = new CountDown(0, 1, 0);//开始倒计时构造一个一分钟的倒计时对象
    }

    void Update()
    {
        if (countDown != null)
        {
            countDown.UpdateTime();//开启倒计时
            countDownText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", countDown.CountDownTime / 3600,
                countDown.CountDownTime / 60, countDown.CountDownTime % 60);//格式化输出倒计时时间
            countDownSlider.value = countDown.CountDownTime / (countDown.TotalTime * 1.0f);//将倒计时时间映射到进度条上
            //控制进度条和文本的显示颜色
            if (countDownSlider.value > 0.5f)
            {
                sliderImg.color = Color.green;
                countDownText.color = Color.green;
            }
            else if (countDownSlider.value > 0.2f)
            {
                sliderImg.color = Color.yellow;
                countDownText.color = Color.yellow;
            }
            else
            {
                sliderImg.color = Color.red;
                countDownText.color = Color.red;
                if (countDownSlider.value == 0)
                {
                    countDown = null;//倒计时结束
                }
            }
        }
    }
}