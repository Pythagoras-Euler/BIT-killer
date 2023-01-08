using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Text;

// WebLink在切换场景时不销毁
// 打开StartScene，连接websocket
// 在WebLink的Update中处理发来和发出的消息
public class WebLink : MonoBehaviour
{
    public ClientWebSocket ws = new ClientWebSocket();
    public string uri = "ws://localhost:8080";
    private string receiveStr; // 获取到的str
    public string receiveJson; // 转换成json



    // private WebLink() { }

    // public string sendStr;
    // 在Start中获取消息
    public void Start()
    {
        //TODO 这里加一个转圈小动画，用于await加载
        OnConnectedToServer();
    }

    // 连接服务端
    public async Task OnConnectedToServer()
    {
        await ws.ConnectAsync(new Uri(uri), CancellationToken.None);
        while (true)
        {
            var reault = new byte[4096];
            //等待接收服务端发送的消息
            await ws.ReceiveAsync(new ArraySegment<byte>(reault), CancellationToken.None);
            UTF8Encoding m_utf8 = new UTF8Encoding(false);
            receiveStr = m_utf8.GetString(reault, 0, reault.Length);
            receiveJson = receiveStr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Send(string text)
    {
        UTF8Encoding m_utf8 = new UTF8Encoding(false);
        byte[] textbyte = m_utf8.GetBytes(text);
        ws.SendAsync(new ArraySegment<byte>(textbyte), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
