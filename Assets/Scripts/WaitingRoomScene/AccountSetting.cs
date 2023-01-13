using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountSetting : MonoBehaviour
{//设置昵称页面
    public string nickName;
    public string userID;
    //可以有一个头像

    [SerializeField] Text UserID;


    // Start is called before the first frame update
    void Start()
    {
        userID = GameObject.FindGameObjectWithTag("UserInfo").GetComponent<UserInfo>().username;
        UserID.text = userID;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
