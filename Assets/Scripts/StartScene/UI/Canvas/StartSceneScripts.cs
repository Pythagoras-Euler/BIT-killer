using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StartSceneScripts : MonoBehaviour
{
    //public Button loginButton;

    //public Button signUpButton;

    //public Button quitButton;

    public GameObject loginPanel;

    public GameObject signUpPanel;

    public GameObject settingPanel;

    public AudioMixer mainAudioMixer;

    public GameObject mask;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void LogInPanelAct()
    {
        loginPanel.SetActive(true);
        mask.SetActive(true);
    }
    public void SignUpPanelAct()
    {
        signUpPanel.SetActive(true);
        mask.SetActive(true);
    }

    public void SettingPanelAct()
    {
        settingPanel.SetActive(true);
        mask.SetActive(true);
    }

    public void setMasterVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolMaster", volValue);
    }

    public void setBGMVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolBGM",volValue);
    }


    public void setSEVolume(float volValue)
    {
        mainAudioMixer.SetFloat("VolSE", volValue);
    }

    //public void Back2Start()
    //{
    //    mask.SetActive(false);
    //    transform.parent.parent.gameObject.SetActive(false);
    //}

    public void QuitGame()
    {
        Application.Quit();
    }

}
