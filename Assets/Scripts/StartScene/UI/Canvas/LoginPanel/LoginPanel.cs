using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public Text promptInfo;
    public GameObject mask;
    // Start is called before the first frame update
    void Start()
    {
        promptInfo.text = "";
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void BackToMainmenu()
    {
        gameObject.SetActive(false);
        mask.SetActive(false);
    }

}
