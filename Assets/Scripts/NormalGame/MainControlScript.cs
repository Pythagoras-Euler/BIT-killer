using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControlScript : MonoBehaviour
{

    int PoliceId;
    [SerializeField] GameControl gameControl;
    public GameObject multyBtn;
    public GameObject doubtPan;
    public GameObject electPan;
    public GameObject statePan;
    public GameObject villagerPan;
    public GameObject wolfPan;
    public GameObject witchPan;
    public GameObject seerPan;
    public GameObject votePan;
    public GameObject idenIcon;
    [SerializeField] UserInfo userInfo;
    [SerializeField] PlayerAssignment playerAssignment;
    private int minWaitTime;
    private int maxWaitTime;
    private int maxPlayerNum;

    // Start is called before the first frame update
    void Start()
    {
        PoliceId = -1;
        minWaitTime = 0;//TODO һ�����ʵ�ֵ��ȡ���ڼ�ʱ��
        maxWaitTime = 9999;//ͬ��
        multyBtn.SetActive(false);

        playerAssignment = GameObject.Find("/PlayerAssignment").GetComponent<PlayerAssignment>();
    }

    // Update is called once per frame
    void Update()//�׶θ����շ��ŵ�GameControl��
    {
        switch(gameControl.gameState)//�ж����ڵĽ׶β�ִ����Ӧ����
        {
            case GameControl.GameState.WAIT:
                Waiting();
                break;
            case GameControl.GameState.START://�ַ������
                StartGame();
                break;
            case GameControl.GameState.KILL:
                WolfAct();
                break;
            case GameControl.GameState.PROPHET:
                ProphetExamine();
                break;
            case GameControl.GameState.WITCH:
                WitchAct();
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
            case GameControl.GameState.END://���㣬����ʤ��
                break;
        }
    }

    //�ȴ�ҳ�棬������icon����Ϊ������
    void Waiting()
    {
        SetDay();

        bool everyoneReady = true;


        if (everyoneReady)//������׼��
        {
            multyBtn.SetActive(true);
        }
    }

    //�ַ������,���Ե���һ�����ڣ�Ҳ��������������һ��ϵͳ��Ϣ
    void StartGame()
    {
        SetNight();
    }

    //
    void SetNight()
    {
        //�����ɰ����ɺ�ҹ
    }

    void SetDay()
    {
        //�����ɺ�ҹ��ɰ���
    }

    void WolfAct()//�����ǲ���Ӧ���ܿ�������Ͷ��˭
    {
        SetNight();
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WOLF)
        {
            if (playerAssignment.playerState == true)
            {
                wolfPan.SetActive(true);
                //TODO ��Ӧ����һ������ʱģ��
            }
            else//��ʾ��Ϣ���ǰ�ť��Ч
            {
                //TODO
            }
        }
    }

    //BTN��Ӧ�¼�
    void WolfVote()
    {
        wolfPan.SetActive(false);
        //TODO ����ʱ��ʱֹͣ������
        //TODO ����ͶƱ��Ϣ
    }

    void WitchAct()//��ֱ������/����ѡһ�ɣ�д������һЩ
    {
        if (playerAssignment.playerCharacter == PlayerAssignment.Character.WITCH)
        {
            if ( playerAssignment.playerState == true)
            {
                witchPan.SetActive(true);
                //TODO ��Ӧ����һ������ʱģ��
            }
            else
            {
                int waittime = Random.Range(minWaitTime, maxWaitTime);
                //TODO �������ʱ����װ��û��
            }
        }
    }    

    void WitchPison()
    {
        witchPan.SetActive(false);
        //TODO ������Ϣ
    }

    void WitchSave()
    {
        witchPan.SetActive(false);
        //TODO ������Ϣ
    }

    void ProphetAct()
    {
        seerPan.SetActive(true);
    }

    void ProphetExamine()
    {
        //string IdenRet = "";

        //return IdenRet;
        
        //TODO ���� ���� ����

        //
    }

    void DeadCheck()
    {

        GameObject targetOP;
        for(int i = 0; i < maxPlayerNum; i++ )
        {
            targetOP = GameObject.Find("PlayerBand" + maxPlayerNum);
            

            //string targetName = targetOP.


        }


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
        electPan.SetActive(true);
    }

    void VotePolice()
    {
        electPan.SetActive(false);
        //TODO ����ͶƱ��Ϣ
    }


    void VoteKill()
    {
        //send btn enable
        votePan.SetActive(true);
    }

    void Ending()
    {
        //��ʾһ��ʤ�߱�
    }

}
