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
        switch(gameControl.gameState)//�ж����ڵĽ׶β�ִ����Ӧ����
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
                WitchPison();//��Ҫ�޸�
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
        int winNum = 0;//0û��Ӯ��1����Ӯ��2����Ӯ

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
