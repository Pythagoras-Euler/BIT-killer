using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    private const string IP = "127.0.0.1";
    private const int PORT = 8712;

    // 用户名输入
    public InputField unameInput;
    // 消息输入
    public InputField msgInput;
    // 登录按钮
    public Button loginBtn;
    // 发送按钮
    public Button sendBtn;
    // 连接状态文本
    public Text stateTxt;
    // 连接按钮文本
    public Text connectBtnText;
    // 聊天室聊天文本
    public Text chatMsgTxt;
    // 封装的ClientSocket对象
    private ClientSocket clientSocket = new ClientSocket();

    void Start()
    {
        chatMsgTxt.text = "";

        loginBtn.onClick.AddListener(() =>
        {
            if (clientSocket.connected)
            {
                // 断开
                clientSocket.CloseSocket();
                stateTxt.text = "已断开";
                connectBtnText.text = "连接";
                unameInput.enabled = true;
            }
            else
            {
                // 连接
                clientSocket.Connect(IP, PORT);
                stateTxt.text = clientSocket.connected ? "已连接" : "未连接";
                connectBtnText.text = clientSocket.connected ? "断开" : "连接";
                if (clientSocket.connected)
                    unameInput.enabled = false;
                // 登录
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
        // JSONObject转string
        string jsonStr = JSONConvert.SerializeObject(jsonObj);
        // string转byte[]
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        // 发送消息给服务端
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
