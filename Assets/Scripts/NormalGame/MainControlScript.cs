using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControlScript : MonoBehaviour
{
    enum Identity {viliager, wolf, witch, prophet };
    int PoliceId;


    // Start is called before the first frame update
    void Start()
    {
        PoliceId = -1;
    }

    // Update is called once per frame
    void Update()
    {
        switch(1)//判断现在的阶段并执行相应函数
        {

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

    void HasLastWords()
    {

    }

    bool CheckLive()
    {
        bool isLive = false;
        return isLive;
    }

    void VotePolice()
    {

    }


    void VoteKill()
    {

    }

}
