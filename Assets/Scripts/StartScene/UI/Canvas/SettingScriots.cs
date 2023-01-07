using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingScriots : MonoBehaviour
{
    public GameObject mask;
    public GameObject settingPanel;
    public Slider MasterVolSlider;
    public Slider BGMVolSlider;
    public Slider SEVolSlider;
    public Toggle ClickSETog;
    public AudioMixer mainAudioMixer;

    private float lastMasterVol;
    private float lastBGMVol;
    private float lastSEVol;
    private bool lastClickSE;


    // Start is called before the first frame update
    void Start()
    {
        SaveSettings();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    //开启设置页面
    public void SettingPanelAct()
    {
        SaveSettings();
        settingPanel.SetActive(true);
        mask.SetActive(true);
    }

    //设置总音量（p.s. 单位是分贝，暂时会导致音量指数级调节） 
    public void SetMasterVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolMaster", volValue);
    }

    //设置背景音乐音量
    public void SetBGMVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolBGM", volValue);
    }

    //设置音效音量（按键音，游戏内音效等）
    public void SetSEVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolSE", volValue);
    }

    //保存上次设置（供退出设置的时候调取）*此函数应在每次打开窗口时调用一次*
    public void SaveSettings()
    {
        lastMasterVol = MasterVolSlider.value;
        lastBGMVol = BGMVolSlider.value;
        lastSEVol = SEVolSlider.value;
        lastClickSE = ClickSETog.isOn;
    }

    //点击取消键（不保存设置退出）
    public void SettingCancelButtonClicked()
    {
         MasterVolSlider.value = lastMasterVol;
        BGMVolSlider.value = lastBGMVol;
        SEVolSlider.value = lastSEVol ;
        ClickSETog.isOn = lastClickSE;

        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }

    //点击确认键（保存设置退出）
    public void SettingConfirmButtonClicked()
    {
        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }

}
