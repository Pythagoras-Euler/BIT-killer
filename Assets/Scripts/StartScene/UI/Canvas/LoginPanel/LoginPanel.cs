using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;

public class LoginPanel : MonoBehaviour
{
    //public Text promptInfo;
    //public GameObject mask;

    public GameObject mask;
    public Text account;
    public Text password;
    public Text promptInfo;
    public WebLink wl;
    public InputField pswInputField;
    public Toggle pswDisplayTog;
    public GameObject userInfo;

    byte[] cipcher;
    string cipchers;
    private  string salt;
    // Start is called before the first frame update



    void Start()
    {
        promptInfo.text = "";

        wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        userInfo = GameObject.FindGameObjectWithTag("UserInfo");
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

    public void PswDisplay()
    {
        pswInputField.contentType = pswDisplayTog.isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
        pswInputField.Select();
    }


    // Start is called before the first frame update
    //private void Start()
    //{
    //    wl = GameObject.Find("WebLink").GetComponent<WebLink>();
    //}

    byte[] Sha256(string plaintext)
    {
        //string cipertext;
        //cipertext = plaintext;//TODO 密码加密

        //From https://github.com/jv-amorim/Unity-Helpers/blob/master/Scripts/HashingHelpers/HashGenerator.cs with MIT Lisence
        byte[] data = Encoding.ASCII.GetBytes(plaintext);
        data = new SHA256Managed().ComputeHash(data);
        //string cipertext = Encoding.ASCII.GetString(data);   

        //return cipertext;
        return data;
    }


    public static string ToHexStrFromByte(byte[] byteDatas)//调试用，log密码输出16进制字符串
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < byteDatas.Length; i++)
        {
            builder.Append(string.Format("{0:X2} ", byteDatas[i]));
        }
        return builder.ToString().Trim();
    }
    public static string ToHexString(string plainString, Encoding encode)
    {
        byte[] byteDatas = encode.GetBytes(plainString);
        return ToHexStrFromByte(byteDatas);
    }

    //TODO 发送请求
    private void loginRequest(string username)
    {
        //string type = "login salt";

        //send "type":"login salt",
        //send "content":"lbwnb"

        //await json type = "login salt"
        //string salt = "qwertyuiopasdfghjklz";//getsalt
        //return salt;

        JsonData userJson = new JsonData();
        userJson["type"] = "login salt";
        userJson["content"] = new JsonData();
        userJson["content"] = acc;
        string userJsonStr = userJson.ToJson();
        Debug.Log(userJsonStr);
        wl.Send(userJsonStr);

    }

    string acc;
    string psw;
    public void LoginCfmBtnClicked()
    {

        acc = account.GetComponent<Text>().text;
        psw = pswInputField.GetComponent<InputField>().text;
        if (psw.Length <= 6)
        {
            promptInfo.text = "请输入大于6位的密码";
        }
        else
        {
            //TODO 两次发送与接受
            loginRequest(acc);



        }
    }

    private void login()
    {
        //string cipcher = Sha256(psw);
        cipcher = Sha256(salt + psw);
        cipchers = ToHexStrFromByte(cipcher);
        //string cipcher = Encoding.UTF8.GetString(cipchers);
        //string cipcher = ToHexStrFromByte(cipchers);

        byte[] ax = new byte[2] { 0xFF, 0xF0 };

        //string opPsw = ToHexString(cipcher, Encoding.ASCII);
        //Debug.Log($"标识\"login\",账号{acc}, 密码{cipchers}");
        //Debug.Log($"ff00:{ToHexStrFromByte(ax)}");
        //Debug.Log($"标识\"login\",账号{acc}, 密码{cipchers}");
        Debug.Log($"salt:{salt},psw:{psw}");
        // 发送请求
        JsonData userJson = new JsonData();
        userJson["type"] = "login";
        userJson["content"] = new JsonData();
        userJson["content"]["username"] = acc;
        userJson["content"]["password"] = cipchers;
        userJson["content"]["salt"] = "";
        string userJsonStr = userJson.ToJson();
        //Debug.Log(userJsonStr);
        wl.Send(userJsonStr);
    }

    private void Update()
    {
        // 处理返回值
        //StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        //string json = sr.ReadToEnd().TrimEnd('\0');
        //Debug.Log(wl.receiveJson);
        JsonData retuser = JsonMapper.ToObject(wl.receiveJson);
        //Debug.Log("retuser");
        if (retuser["type"].ToString() == "login") // 如果是登录 
        {
            Debug.Log("retuser");
            string retType = retuser["type"].ToString();//接收服务器返回值
            bool retSuccess = retuser["success"].ToString() == "True" ? true : false;
            string retMes = retuser["message"].ToString();


            if (retType != "login")//不知道什么时候会出现这种错误（大概是用户乱点？
            {
                promptInfo.text = "未知错误(请勿频繁操作）";
            }
            else if (retSuccess == false)//返回出现错误
            {
                if (retMes == "Unvalid Username")//登陆失败
                {
                    promptInfo.text = "该用户名未被注册";//用户名错误
                }
                else if (retMes == "Wrong username")//Wrong Password
                {
                    promptInfo.text = "密码错误";//密码错误

                }
                else if (retMes == "Login Failed")//登陆失败
                {
                    promptInfo.text = "登陆失败，请重试";//是不是应该有用户名不存在和密码错误两种检查方式
                }
                else//其他错误
                {
                    promptInfo.text = retMes;
                }
            }
            else if (retMes == "Loginsuccessful" || retMes == "Login successful")//成功
            {

                promptInfo.text = "登录成功！";
                mask.SetActive(false);
                this.gameObject.SetActive(false);
                userInfo.GetComponent<UserInfo>().username = acc;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//切换到下一个场景：菜单界面（主界面）
                                                                                     //（需要在 文件File->
                                                                                     //生成设置BuildSettings 里设置顺序）
                                                                                     //如果需要的话可以设置一个过渡动画

            }
        }
        else if(retuser["type"].ToString() == "login salt")
        {
            string retType = retuser["type"].ToString();//接收服务器返回值
            bool retSuccess = retuser["success"].ToString() == "True" ? true : false;
            string retMes = retuser["message"].ToString();
            string retContent = retuser["content"].ToString();

            if (retType != "login salt")//不知道什么时候会出现这种错误（大概是用户乱点？
            {
                promptInfo.text = "未知错误(请勿频繁操作）";
            }
            else if (retMes != "Salt")//返回出现错误retSuccess == false
            {
                if (retMes == "Invalid Username")//登陆失败
                {
                    promptInfo.text = "该用户名未被注册";//用户名错误
                }
                else//其他错误
                {
                    promptInfo.text = retMes + "!!!";
                    Debug.Log(retMes);
                }
            }
            else if (retType == "login salt" && retMes == "Salt")//成功
            {
                //Debug.Log(retContent);
                salt = retContent;
                login();
            }    
        }
    }

}
