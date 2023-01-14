using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPan : MonoBehaviour
{
    public GameObject infoPan;
    public void onInfoBtnClicked()
    {
        infoPan.SetActive(true);
    }
    public void CloseInfoBtn()
    {
        infoPan.SetActive(false);
    }
}
