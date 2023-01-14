using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public enum GameState
    {
        WAIT,       // �ȴ���ʼ
        START,      // ��Ϸ��ʼ���ַ�����ƽ׶�
        KILL,       // ���˵���
        PROPHET,    // Ԥ�ԼҲ���
        WITCH,      // Ů�ײ���
        ELECT,      // ѡ�پ���
        DISCUSS,    // ����
        VOTE,       // ͶƱ����
        WORDS,      // ����
        END,        // ��Ϸ����������ʤ�����׶�
    }
    public GameState gameState;
    public int DayCount;
    public bool hasDown;//hasDone����������ÿ�غϲ�������������ͬʱ������ť��������ƴд������������ä����Ҫ������Щϸ�ڣ������磨

    [System.Serializable]
    public struct DayEvent
    {
        public string killed;
        public string saved;
        public string poisoned;
        public string inspected;
        public string checkIden;
        public string voted;
        public bool isnew;
        public string nowPolice;
    }
    public DayEvent dayEvent;
    ArrayList DayEvenList = new ArrayList();

    public string[] players;//����б�
    public Hashtable playerCharacterMap;//��ɫ��ݱ�
    public string captain;//����
    public Hashtable playerStateMap;//��ɫ״̬����������true�������

    public string[] seenPlayers;//��Ԥ�Լҵģ���Ұ
    public int[] canDo;//�������룺��������Ů��ÿ���ж��������Ժ�Ҳ���Ը�Ϊ������ɫ���ж�����
}
