using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public enum GameState
    {
        START,      // 游戏开始
        KILL,       // 狼人刀人
        PROPHET,    // 预言家查验
        WITCH,      // 女巫操作
        ELECT,      // 选举警长
        DISCUSS,    // 交流
        VOTE,       // 投票环节
        WORDS,      // 遗言
        END,        // 游戏结束
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
