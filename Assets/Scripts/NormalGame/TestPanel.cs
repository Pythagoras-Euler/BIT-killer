using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    private const string IP = "127.0.0.1";
    private const int PORT = 8712;

    // �û�������
    public InputField unameInput;
    // ��Ϣ����
    public InputField msgInput;
    // ��¼��ť
    public Button loginBtn;
    // ���Ͱ�ť
    public Button sendBtn;
    // ����״̬�ı�
    public Text stateTxt;
    // ���Ӱ�ť�ı�
    public Text connectBtnText;
    // �����������ı�
    public Text chatMsgTxt;
    // ��װ��ClientSocket����
    private ClientSocket clientSocket = new ClientSocket();

    void Start()
    {
        chatMsgTxt.text = "";

        loginBtn.onClick.AddListener(() =>
        {
            if (clientSocket.connected)
            {
                // �Ͽ�
                clientSocket.CloseSocket();
                stateTxt.text = "�ѶϿ�";
                connectBtnText.text = "����";
                unameInput.enabled = true;
            }
            else
            {
                // ����
                clientSocket.Connect(IP, PORT);
                stateTxt.text = clientSocket.connected ? "������" : "δ����";
                connectBtnText.text = clientSocket.connected ? "�Ͽ�" : "����";
                if (clientSocket.connected)
                    unameInput.enabled = false;
                // ��¼
                Send("login");
            }
        });

        sendBtn.onClick.AddListener(() =>
        {
            Send("chat", msgInput.text);
        });
    }

    private void Update()
    {
        if (clientSocket.connected)
        {
            clientSocket.BeginReceive();
        }
        var msg = clientSocket.GetMsgFromQueue();
        if (!string.IsNullOrEmpty(msg))
        {
            chatMsgTxt.SetAllDirty();
            chatMsgTxt.text += msg + "\n";

            Debug.Log("RecvCallBack: " + msg);
        }
    }

    private void Send(string protocol, string msg = "")
    {
        JSONObject jsonObj = new JSONObject();
        jsonObj["protocol"] = protocol;
        jsonObj["uname"] = unameInput.text;
        jsonObj["msg"] = msg;
        // JSONObjectתstring
        string jsonStr = JSONConvert.SerializeObject(jsonObj);
        // stringתbyte[]
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        // ������Ϣ�������
        clientSocket.SendData(data);
    }

    private void OnApplicationQuit()
    {
        if (clientSocket.connected)
        {
            clientSocket.CloseSocket();
        }
    }
}
