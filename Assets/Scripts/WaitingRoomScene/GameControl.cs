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
    public bool hasDown;//hasDone，用于限制每回合操作次数（避免同时按俩按钮），至于拼写的问题我是文盲，不要在意这些细节，求轻喷（

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

    public string[] players;//玩家列表
    public Hashtable playerCharacterMap;//角色身份表
    public string captain;//房主
    public Hashtable playerStateMap;//角色状态（死亡）表，true代表活着

    public string[] seenPlayers;//（预言家的）视野
    public int[] canDo;//初步设想：用于限制女巫每局行动次数。以后也可以改为其他角色的行动次数
}
