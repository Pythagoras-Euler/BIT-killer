using System;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;

public class ClientSocket
{
    private Socket init()
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // ���յ���Ϣ���ݰ���С����Ϊ 0x4000 byte, ��16KB
        m_recvBuff = new byte[0x4000];
        m_recvCb = new AsyncCallback(RecvCallBack);
        return clientSocket;
    }

    /// <summary>
    /// ���ӷ�����
    /// </summary>
    /// <param name="host">ip��ַ</param>
    /// <param name="port">�˿ں�</param>
    public void Connect(string host, int port)
    {
        if (m_socket == null)
            m_socket = init();
        try
        {
            Debug.Log("connect: " + host + ":" + port);
            m_socket.SendTimeout = 3;
            m_socket.Connect(host, port);
            connected = true;

        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void SendData(byte[] bytes)
    {
        NetworkStream netstream = new NetworkStream(m_socket);
        netstream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// ���Խ�����Ϣ��ÿ֡���ã�
    /// </summary>
    public void BeginReceive()
    {
        m_socket.BeginReceive(m_recvBuff, 0, m_recvBuff.Length, SocketFlags.None, m_recvCb, this);
    }

    /// <summary>
    /// ���յ�����������Ϣʱ��ص��������
    /// </summary>
    private void RecvCallBack(IAsyncResult ar)
    {
        var len = m_socket.EndReceive(ar);
        byte[] msg = new byte[len];
        Array.Copy(m_recvBuff, msg, len);
        var msgStr = System.Text.Encoding.UTF8.GetString(msg);
        // ����Ϣ���������
        m_msgQueue.Enqueue(msgStr);
        // ��buffer����
        for (int i = 0; i < m_recvBuff.Length; ++i)
        {
            m_recvBuff[i] = 0;
        }
    }

    /// <summary>
    /// ����Ϣ������ȡ����Ϣ
    /// </summary>
    /// <returns></returns>
    public string GetMsgFromQueue()
    {
        if (m_msgQueue.Count > 0)
            return m_msgQueue.Dequeue();
        return null;
    }

    /// <summary>
    /// �ر�Socket
    /// </summary>
    public void CloseSocket()
    {
        Debug.Log("close socket");
        try
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
        }
        finally
        {
            m_socket = null;
            connected = false;
        }
    }


    public bool connected = false;

    private byte[] m_recvBuff;
    private AsyncCallback m_recvCb;
    private Queue<string> m_msgQueue = new Queue<string>();
    private Socket m_socket;
}
