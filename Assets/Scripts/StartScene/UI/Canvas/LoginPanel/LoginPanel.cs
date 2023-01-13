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
        //cipertext = plaintext;//TODO �������

        //From https://github.com/jv-amorim/Unity-Helpers/blob/master/Scripts/HashingHelpers/HashGenerator.cs with MIT Lisence
        byte[] data = Encoding.ASCII.GetBytes(plaintext);
        data = new SHA256Managed().ComputeHash(data);
        //string cipertext = Encoding.ASCII.GetString(data);   

        //return cipertext;
        return data;
    }


    public static string ToHexStrFromByte(byte[] byteDatas)//�����ã�log�������16�����ַ���
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

    //TODO ��������
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
            promptInfo.text = "���������6λ������";
        }
        else
        {
            //TODO ���η��������
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
        //Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipchers}");
        //Debug.Log($"ff00:{ToHexStrFromByte(ax)}");
        //Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipchers}");
        Debug.Log($"salt:{salt},psw:{psw}");
        // ��������
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
        // ������ֵ
        //StreamReader sr = new StreamReader(Application.dataPath + "/jsontest.txt");
        //string json = sr.ReadToEnd().TrimEnd('\0');
        //Debug.Log(wl.receiveJson);
        JsonData retuser = JsonMapper.ToObject(wl.receiveJson);
        //Debug.Log("retuser");
        if (retuser["type"].ToString() == "login") // ����ǵ�¼ 
        {
            Debug.Log("retuser");
            string retType = retuser["type"].ToString();//���շ���������ֵ
            bool retSuccess = retuser["success"].ToString() == "True" ? true : false;
            string retMes = retuser["message"].ToString();


            if (retType != "login")//��֪��ʲôʱ���������ִ��󣨴�����û��ҵ㣿
            {
                promptInfo.text = "δ֪����(����Ƶ��������";
            }
            else if (retSuccess == false)//���س��ִ���
            {
                if (retMes == "Unvalid Username")//��½ʧ��
                {
                    promptInfo.text = "���û���δ��ע��";//�û�������
                }
                else if (retMes == "Wrong username")//Wrong Password
                {
                    promptInfo.text = "�������";//�������

                }
                else if (retMes == "Login Failed")//��½ʧ��
                {
                    promptInfo.text = "��½ʧ�ܣ�������";//�ǲ���Ӧ�����û��������ں�����������ּ�鷽ʽ
                }
                else//��������
                {
                    promptInfo.text = retMes;
                }
            }
            else if (retMes == "Loginsuccessful" || retMes == "Login successful")//�ɹ�
            {

                promptInfo.text = "��¼�ɹ���";
                mask.SetActive(false);
                this.gameObject.SetActive(false);
                userInfo.GetComponent<UserInfo>().username = acc;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//�л�����һ���������˵����棨�����棩
                                                                                     //����Ҫ�� �ļ�File->
                                                                                     //��������BuildSettings ������˳��
                                                                                     //�����Ҫ�Ļ���������һ�����ɶ���

            }
        }
        else if(retuser["type"].ToString() == "login salt")
        {
            string retType = retuser["type"].ToString();//���շ���������ֵ
            bool retSuccess = retuser["success"].ToString() == "True" ? true : false;
            string retMes = retuser["message"].ToString();
            string retContent = retuser["content"].ToString();

            if (retType != "login salt")//��֪��ʲôʱ���������ִ��󣨴�����û��ҵ㣿
            {
                promptInfo.text = "δ֪����(����Ƶ��������";
            }
            else if (retMes != "Salt")//���س��ִ���retSuccess == false
            {
                if (retMes == "Invalid Username")//��½ʧ��
                {
                    promptInfo.text = "���û���δ��ע��";//�û�������
                }
                else//��������
                {
                    promptInfo.text = retMes + "!!!";
                    Debug.Log(retMes);
                }
            }
            else if (retType == "login salt" && retMes == "Salt")//�ɹ�
            {
                //Debug.Log(retContent);
                salt = retContent;
                login();
            }    
        }
    }

}
