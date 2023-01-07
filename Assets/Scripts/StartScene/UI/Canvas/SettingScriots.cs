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

    //��������ҳ��
    public void SettingPanelAct()
    {
        SaveSettings();
        settingPanel.SetActive(true);
        mask.SetActive(true);
    }

    //������������p.s. ��λ�Ƿֱ�����ʱ�ᵼ������ָ�������ڣ� 
    public void SetMasterVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolMaster", volValue);
    }

    //���ñ�����������
    public void SetBGMVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolBGM", volValue);
    }

    //������Ч����������������Ϸ����Ч�ȣ�
    public void SetSEVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolSE", volValue);
    }

    //�����ϴ����ã����˳����õ�ʱ���ȡ��*�˺���Ӧ��ÿ�δ򿪴���ʱ����һ��*
    public void SaveSettings()
    {
        lastMasterVol = MasterVolSlider.value;
        lastBGMVol = BGMVolSlider.value;
        lastSEVol = SEVolSlider.value;
        lastClickSE = ClickSETog.isOn;
    }

    //���ȡ�����������������˳���
    public void SettingCancelButtonClicked()
    {
         MasterVolSlider.value = lastMasterVol;
        BGMVolSlider.value = lastBGMVol;
        SEVolSlider.value = lastSEVol ;
        ClickSETog.isOn = lastClickSE;

        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }

    //���ȷ�ϼ������������˳���
    public void SettingConfirmButtonClicked()
    {
        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }

}
