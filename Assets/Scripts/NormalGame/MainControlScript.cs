using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControlScript : MonoBehaviour
{
    enum Identity {viliager, wolf, witch, prophet };
    int PoliceId;
    [SerializeField] GameControl gameControl;


    // Start is called before the first frame update
    void Start()
    {
        PoliceId = -1;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameControl.gameState)//判断现在的阶段并执行相应函数
        {
            case GameControl.GameState.WAIT:
                Waiting();
                break;
            case GameControl.GameState.START:
                StartGame();
                break;
            case GameControl.GameState.KILL:
                WolfVote();
                break;
            case GameControl.GameState.PROPHET:
                ProphetExamine();
                break;
            case GameControl.GameState.WITCH:
                WitchPison();//需要修改
                WitchSave();
                break;
            case GameControl.GameState.DISCUSS:
                break;
            case GameControl.GameState.ELECT:
                ElectPolice();
                break;
            case GameControl.GameState.VOTE:
                VoteKill();
                break;
            case GameControl.GameState.WORDS:
                LastWords();
                break;
            case GameControl.GameState.END:
                break;
        }
    }

    void Waiting()
    {

    }

    void StartGame()
    {
        
    }

    void SetNight()
    {

    }

    void SetDay()
    {

    }

    void WolfVote()
    {

    }


    void WitchPison()
    {

    }

    void WitchSave()
    {

    }

    string ProphetExamine()
    {
        string IdenRet = "";

        return IdenRet;
    }

    void DeadCheck()
    {

    }

    int HasVictory()
    {
        int winNum = 0;//0没人赢，1好人赢，2狼人赢

        return winNum;
    }

    void LastWords()
    {

    }

    bool CheckLive()
    {
        bool isLive = false;
        return isLive;
    }

    void ElectPolice()
    {

    }


    void VoteKill()
    {
        //send btn

    }

    void Ending()
    {

    }

}
