using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class LoginCfmBtn : MonoBehaviour
{
    //public GameObject mask;
    //public Text account;
    //public Text password;
    //public Text promptInfo;
    //public WebLink wl;

    //// Start is called before the first frame update
    //private void Start()
    //{
    //    wl = GameObject.Find("WebLink").GetComponent<WebLink>();
    //}

    //string Sha256(string plaintext)
    //{
    //    string cipertext;
    //    cipertext = plaintext;//TODO 密码加密

    //    return cipertext;
    //}

    //public void LoginCfmBtnClicked()
    //{

    //    var acc = account.GetComponent<Text>().text;
    //    var psw = password.GetComponent<Text>().text;
    //    if (psw.Length <= 6)
    //    {
    //        promptInfo.text = "请输入大于6位的密码";
    //    }
    //    else
    //    {

    //        string cipcher = Sha256(psw);

    //        Debug.Log($"标识\"login\",账号{acc}, 密码{cipcher}");

    //        //TODO:发送请求
    //        User user = new User("login", acc, cipcher);
    //        string userJson = JsonUtility.ToJson(user);
    //        wl.Send(userJson);

    //        // 处理返回值
    //        RetUser retuser = JsonUtility.FromJson<RetUser>(wl.receiveJson);
    //        if(retuser.type == "login") // 如果是登录 
    //        {

    //            string retType = retuser.type;//接收服务器返回值
    //            bool retSuccess = retuser.success;
    //            string retMes = retuser.message;


    //            if (retType != "login")//不知道什么时候会出现这种错误（大概是用户乱点？
    //            {
    //                promptInfo.text = "未知错误(请勿频繁操作）";
    //            }
    //            else if (retSuccess == false)//返回出现错误
    //            {
    //                if (retMes == "Unvalid Username")//登陆失败
    //                {
    //                    promptInfo.text = "该用户名未被注册";//用户名错误
    //                }
    //                else if (retMes == "Wrong Password")
    //                {
    //                    promptInfo.text = "密码错误";//密码错误

    //                }
    //                else if (retMes == "Login Failed")//登陆失败
    //                {
    //                    promptInfo.text = "登陆失败，请重试";//是不是应该有用户名不存在和密码错误两种检查方式
    //                }
    //                else//其他错误
    //                {
    //                    promptInfo.text = retMes;
    //                }
    //            }
    //            else if (retMes == "Login successful")//成功
    //            {

    //                promptInfo.text = "登录成功！";
    //                mask.SetActive(false);
    //                transform.parent.parent.gameObject.SetActive(false);
    //                //TODO close Panel
    //                //TODO set loginPanel true
    //                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//切换到下一个场景：菜单界面（主界面）
    //                                                                                     //（需要在 文件File->
    //                                                                                     //生成设置BuildSettings 里设置顺序）
    //                                                                                     //如果需要的话可以设置一个过渡动画


    //            }
    //        }
    //    }


    //}
    
}
