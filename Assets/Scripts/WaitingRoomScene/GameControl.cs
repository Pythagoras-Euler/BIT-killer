using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public enum GameState
    {
        WAIT,       // 等待开始
        START,      // 游戏开始，分发身份牌阶段
        KILL,       // 狼人刀人
        PROPHET,    // 预言家查验
        WITCH,      // 女巫操作
        ELECT,      // 选举警长
        DISCUSS,    // 交流
        VOTE,       // 投票环节
        WORDS,      // 遗言
        END,        // 游戏结束，宣布胜利方阶段
    }
    public GameState gameState;
    public int DayCount;
    public bool hasDown;

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

    public string[] players;
    public HashMap<string,string> playerCharacterMap;
    public string captain;
    public HashMap<string,bool> playerStateMap;

}
