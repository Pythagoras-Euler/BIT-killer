using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Text;
using System.IO;

// WebLink���л�����ʱ������
// ��StartScene������websocket
// ��WebLink��Update�д������ͷ�������Ϣ
public class WebLink : MonoBehaviour
{
    public ClientWebSocket ws = new ClientWebSocket();
    public string uri = "ws://localhost:8080";
    private string receiveStr; // ��ȡ����str
    public string receiveJson; // ת����json
    public byte[] reault;


    // private WebLink() { }

    // public string sendStr;
    // ��Start�л�ȡ��Ϣ
    public void Start()
    {
        //TODO �����һ��תȦС����������await����
        OnConnectedToServer();
    }

    // ���ӷ����
    public async Task OnConnectedToServer()
    {
        await ws.ConnectAsync(new Uri(uri), CancellationToken.None);
        while (true)
        {
            reault = new byte[40960];
            //�ȴ����շ���˷��͵���Ϣ
            await ws.ReceiveAsync(new ArraySegment<byte>(reault), CancellationToken.None);
            UTF8Encoding m_utf8 = new UTF8Encoding(false);
            receiveStr = m_utf8.GetString(reault, 0, reault.Length);
            File.WriteAllText(Application.dataPath + "/jsontest.txt", receiveStr, m_utf8);
            StreamReader sr = new StreamReader(Application.dataPath+"/jsontest.txt");
            receiveJson = sr.ReadLine();
            receiveJson =receiveJson.TrimEnd('\0');
        }
    }

    // server --> json --> local 
    // many json files   Queue  ���洢���files
    // ��һ����Ϣ������С�

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
