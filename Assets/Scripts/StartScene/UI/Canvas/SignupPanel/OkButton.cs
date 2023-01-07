using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace StartScene.UI.Canvas.SignupPanel
{
    public class OkButton: MonoBehaviour
    {
        private Button _okButton;

        public GameObject mask;

        public GameObject account;

        public GameObject password;

        public GameObject confirmPassword;

        public Text errorInfo;

        public Text loginAccount;

        public Text loginPassword;

        [SerializeField] WebLink wl;


        private void Start()
        {
            _okButton = transform.GetComponent<Button>();
            _okButton.onClick.AddListener(OkButtonClicked);
            errorInfo.text = "";
            wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
        }

        //private void OnDestroy()
        //{

        //}

        string Sha256(string plaintext)
        {
            //string cipertext;
            //cipertext = plaintext;//TODO 密码加密

            //return cipertext;

            //From https://github.com/jv-amorim/Unity-Helpers/blob/master/Scripts/HashingHelpers/HashGenerator.cs 
            //MIT Lisence Authorized;
            byte[] data = Encoding.ASCII.GetBytes(plaintext);
            data = new SHA256Managed().ComputeHash(data);
            string cipertext = Encoding.ASCII.GetString(data);
            return cipertext;
        }


        private void OkButtonClicked()
        {
            var acc = account.GetComponent<Text>().text;
            var psw = password.GetComponent<Text>().text;
            var cfmPsw = confirmPassword.GetComponent<Text>().text;
            if (psw.Length <= 6)//密码强度检查（只做了大于6位，这玩意没必要搞太复杂）（有时间可以搞一个低强度密码本匹配，在后端搞也行）
            {
                errorInfo.text = "请输入大于6位的密码";
            }
            else if (!psw.Equals(cfmPsw))
            {
                errorInfo.text = "两次密码输入不一致";
                //TODO:两次密码输入不一致
            }
            else 
            {

                string cipcher = Sha256(psw);
                Debug.Log($"标识\"register\",注册账号{acc}, 密码{cipcher}");

                // 发送请求
                User user = new User("register",acc,cipcher);
                string userJson = JsonMapper.ToJson(user);
                Debug.Log(userJson);
                wl.Send(userJson);

                // 接收服务器返回值
                // RetUser retuser = JsonUtility.FromJson<RetUser>(wl.receiveJson);
                Debug.Log(wl.receiveJson);
                RetUser retuser = JsonMapper.ToObject<RetUser>(wl.receiveJson);
                if (retuser.type == "register") // 如果是注册
                {

                    string retType = "register";
                    bool retSuccess = retuser.success;
                    string retMes = retuser.message;


                    if (retType != "register")//不知道什么时候会出现这种错误（大概是用户乱点？
                    {
                        errorInfo.text = "请勿频繁操作（未知错误）";
                    }
                    else if (retSuccess == false)//返回出现错误
                    {
                        if (retMes == "Duplicate username")//用户名重复错误
                        {
                            errorInfo.text = "该用户名已被注册";
                        }
                        else//其他错误
                        {
                            errorInfo.text = retMes;
                        }
                    }
                    else if (retMes == "Registration successful")//成功
                    {

                        errorInfo.text = "注册成功！";
                        mask.SetActive(false);
                        transform.parent.parent.gameObject.SetActive(false);
                        //TODO close Panel

                        //TODO set loginPanel true

                        loginAccount.text = acc;
                        loginPassword.text = psw;

                    }
                }
            }
        }
    }
}