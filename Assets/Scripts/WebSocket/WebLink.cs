using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Text;

// WebLink���л�����ʱ������
// ��StartScene������websocket
// ��WebLink��Update�д������ͷ�������Ϣ
public class WebLink : MonoBehaviour
{
    public ClientWebSocket ws = new ClientWebSocket();
    public string uri = "ws://localhost:8080";
    private string receiveStr; // ��ȡ����str
    public string receiveJson; // ת����json



    // private WebLink() { }

    // public string sendStr;
    // ��Start�л�ȡ��Ϣ
    public void Start()
    {
        OnConnectedToServer();
    }

    // ���ӷ����
    public async Task OnConnectedToServer()
    {
        await ws.ConnectAsync(new Uri(uri), CancellationToken.None);
        while (true)
        {
            var reault = new byte[4096];
            //�ȴ����շ���˷��͵���Ϣ
            await ws.ReceiveAsync(new ArraySegment<byte>(reault), CancellationToken.None);
            UTF8Encoding m_utf8 = new UTF8Encoding(false);
            receiveStr = m_utf8.GetString(reault, 0, reault.Length);
            // string[] strArray = receiveStr.Split(new string[] {"  ","  "}, StringSplitOptions.RemoveEmptyEntries);
            // receiveJson = strArray[0]; // TODO��strת��
            // Debug.Log(receiveJson.Length.ToString());
            // Debug.Log(receiveJson);
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
