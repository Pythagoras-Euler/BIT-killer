using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public enum GameState
    {
        START,      // ��Ϸ��ʼ
        KILL,       // ���˵���
        PROPHET,    // Ԥ�ԼҲ���
        WITCH,      // Ů�ײ���
        ELECT,      // ѡ�پ���
        DISCUSS,    // ����
        VOTE,       // ͶƱ����
        WORDS,      // ����
        END,        // ��Ϸ����
    }
    public GameState gameState;
    public int DayCount;
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
    }
    public DayEvent dayEvent;
}
