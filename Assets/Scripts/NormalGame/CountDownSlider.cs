using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��1�����ű����涨����һ������ά������ʱʱ�����CountDown����������������˵���ʱ��ʱ���룬������ʱ���ʣ��ʱ����Щ��Ա��
//          ����һ�����µ���ʱ�ĺ���UpdateTime���õ�ʱ��ֻҪ��ʱ������Ϊʵ�δ�����������һ������ʱ���󣬵����亯��UpdateTime���ɣ�
//          ����ͨ��������CountDownTime ��ȡ��ʣ��ʱ�䡣
//��2������������ֵ��θ�����ʱ��������ʵ���ǽ���������һ��ӳ���ϵ����������ֵ��0��1������ʱΪ 0����ʱ�䣻
//          ӳ�乫ʽΪ��value/1.0f=����ʱʣ��ʱ��/��ʱ�䣻��countDownSlider.value = countDown.CountDownTime / (countDown.TotalTime * 1.0f)��
//          �����Ϳ���ʵ�ֵ���ʱͬ��������������ʾ��Ч���ˡ�
//��3������������ű�ע�͡�
//��������������������������������
//��Ȩ����������ΪCSDN���������ܵ�UnityС�ݡ���ԭ�����£���ѭCC 4.0 BY - SA��ȨЭ�飬ת���븽��ԭ�ĳ������Ӽ���������
//ԭ�����ӣ�https://blog.csdn.net/qq_42437783/article/details/125635560


/// <summary>
/// ����ʱ����
/// </summary>
public class CountDown
{
    public int hour;
    public int minute;
    public int second;

    private int countDownTime;//����ʱʣ��ʱ�䣬��Ϊ��λ
    public int CountDownTime { get { return countDownTime; } }
    private int totalTime;//��ʱ�䣬��Ϊ��λ
    public int TotalTime { get { return totalTime; } }

    public CountDown(int _hour, int _minute, int _second)
    {
        hour = _hour;
        minute = _minute;
        second = _second;
        totalTime = 3600 * hour + 60 * minute + second;//������ʱ��
        countDownTime = totalTime;
    }

    private float time = 0;//��������ʱ����

    /// <summary>
    /// ����ʣ��ʱ�䣬���һ�룬ʱ��-1
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
    public Text countDownText;//����ʱ�ı�
    public Slider countDownSlider;//��������ʾ
    private Image sliderImg;//���������ͼ
    private CountDown countDown = null;//��������ʱ����
    public Button startCountDownBtn;//��ʼ����ʱ��ť

    void Start()
    {
        countDownSlider.value = 1;
        sliderImg = countDownSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        startCountDownBtn.onClick.AddListener(OnCountDownClick);
    }

    private void OnCountDownClick()//�÷�ʵ��
    {
        countDown = new CountDown(0, 1, 0);//��ʼ����ʱ����һ��һ���ӵĵ���ʱ����
    }

    void Update()
    {
        if (countDown != null)
        {
            countDown.UpdateTime();//��������ʱ
            countDownText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", countDown.CountDownTime / 3600,
                countDown.CountDownTime / 60, countDown.CountDownTime % 60);//��ʽ���������ʱʱ��
            countDownSlider.value = countDown.CountDownTime / (countDown.TotalTime * 1.0f);//������ʱʱ��ӳ�䵽��������
            //���ƽ��������ı�����ʾ��ɫ
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
                    countDown = null;//����ʱ����
                }
            }
        }
    }
}