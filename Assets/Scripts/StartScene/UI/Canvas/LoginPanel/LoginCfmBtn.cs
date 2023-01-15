using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;



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
    //    wl = GameObject.FindGameObjectWithTag("WebLink").GetComponent<WebLink>();
    //}


    
    //string Sha256(string plaintext)
    //{
    //    string cipertext;
    //    cipertext = plaintext;//TODO �������

    //    return cipertext;
    //}

    //public void LoginCfmBtnClicked()
    //{

    //    var acc = account.GetComponent<Text>().text;
    //    var psw = password.GetComponent<Text>().text;
    //    if (psw.Length <= 6)
    //    {
    //        promptInfo.text = "���������6λ������";
    //    }
    //    else
    //    {

    //        string cipcher = Sha256(psw);

    //        // Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipcher}");

    //        //��������
    //        User user = new User("login", acc, cipcher);
    //        string userJson = JsonMapper.ToJson(user);
    //        wl.Send(userJson);

    //        // ��������ֵ
    //        RetUser retuser = JsonMapper.ToObject<RetUser>(wl.receiveJson);
    //        if(retuser.type == "login") // ����ǵ�¼ 
    //        {

    //            string retType = retuser.type;//���շ���������ֵ
    //            bool retSuccess = retuser.success;
    //            string retMes = retuser.message;


    //            if (retType != "login")//��֪��ʲôʱ���������ִ��󣨴�����û��ҵ㣿
    //            {
    //                promptInfo.text = "δ֪����(����Ƶ��������";
    //            }
    //            else if (retSuccess == false)//���س��ִ���
    //            {
    //                if (retMes == "Unvalid Username")//��½ʧ��
    //                {
    //                    promptInfo.text = "���û���δ��ע��";//�û�������
    //                }
    //                else if (retMes == "Wrong Password")
    //                {
    //                    promptInfo.text = "�������";//�������

    //                }
    //                else if (retMes == "Login Failed")//��½ʧ��
    //                {
    //                    promptInfo.text = "��½ʧ�ܣ�������";//�ǲ���Ӧ�����û��������ں�����������ּ�鷽ʽ
    //                }
    //                else//��������
    //                {
    //                    promptInfo.text = retMes;
    //                }
    //            }
    //            else if (retMes == "Login successful")//�ɹ�
    //            {

    //                promptInfo.text = "��¼�ɹ���";
    //                mask.SetActive(false);
    //                transform.parent.parent.gameObject.SetActive(false);
    //                //TODO close Panel
    //                //TODO set loginPanel true
    //                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//�л�����һ���������˵����棨�����棩
    //                                                                                     //����Ҫ�� �ļ�File->
    //                                                                                     //��������BuildSettings ������˳��
    //                                                                                     //�����Ҫ�Ļ���������һ�����ɶ���


    //            }
    //        }
    //    }


    //}
    
}
