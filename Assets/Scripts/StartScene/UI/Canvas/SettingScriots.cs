using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScriots : MonoBehaviour
{
    public GameObject mask;
    public GameObject settingPanel;
    public Slider MasterVolSlider;
    public Slider BGMVolSlider;
    public Slider SEVolSlider;
    public Toggle ClickSETog;

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

    public void SaveSettings()
    {
        lastMasterVol = MasterVolSlider.value;
        lastBGMVol = BGMVolSlider.value;
        lastSEVol = SEVolSlider.value;
        lastClickSE = ClickSETog.isOn;
    }

    public void settingCancelButtonClicked()
    {
         MasterVolSlider.value = lastMasterVol;
        BGMVolSlider.value = lastBGMVol;
        SEVolSlider.value = lastSEVol ;
        ClickSETog.isOn = lastClickSE;

        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }
    public void settingConfirmButtonClicked()
    {
        transform.gameObject.SetActive(false);
        mask.SetActive(false);
    }

}
