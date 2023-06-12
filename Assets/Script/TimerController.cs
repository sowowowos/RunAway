using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TimerController : MonoBehaviourPunCallbacks
{
   
    public float time, origin_time;
    public string player1, player2, player3, player4 , result, surviver;
    public Text TimeLeft;   //남은시간
    private AudioSource audio;
    private bool ON,bug = false;
    private int playernum;
    public GameObject endUI, leftplayernum;
    void Start()
    {
        //endUI = GameObject.FindGameObjectWithTag("END");  //지금 상태는 술래 시작하자마자 엔드유아이 보이는 것, 문제다 
        endUI.SetActive(false);
        
        //playernum = PhotonNetwork.PlayerList.Length - 1;
        player1 = "";
        player2 = "";
        player3 = "";
        player4 = "";
        result = "";
        surviver = "";

        origin_time = time;
        audio = GetComponent<AudioSource>();
    }
    IEnumerator EndDelay()

    {

        yield return new WaitForSeconds(3.0f);

        PhotonNetwork.Disconnect();
        Application.Quit();
        Debug.Log("종료 실행");
    }



    void Update()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("ObjectPrefab");
        playernum = temp.Length;

        if (playernum != 0 && bug == false)                                // 만약 이 줄이 있으면 시작하자마자 창 안뜨고 깔끔 하지만 무조건 술래승리!!!! 게다가 사물은 술래 WIN!! 고로 사물에겐 전달X
        {
            endUI.SetActive(false);
            bug = true;
        }
        if (playernum == 0)
        {
            result = "모든 탈영병을 잡았다!";
            endUI.SetActive(true);
            //StartCoroutine(EndDelay());
            audio.Stop();
        }
        //else if (playernum != 0) endUI.SetActive(false);    이 줄이 없으면 시작하자마자 솔져에서 UI창이 뜸 하지만 술래승리 사물승리는 잘뜸 시간 0되었을때
        
        
        if (time > 0 && playernum != 0)
        { 
            time -= Time.deltaTime; 

        }
        if (time < 20 && ON == false)
        {
            audio.Play();
            ON = true;
        }
/*        if (time > 0)
        {
            int SolHp =GameObject.Find("Soldier_demo").GetComponent<SoidierController>().Hp;
            if(SolHp == 0)
            {
                result = "사물승리";
            }
        }*/
        /*if(GameObject.Find("Soldier_demo").GetComponent<SoidierController>().isDie_s == true)
        {
            result = "사물 승리!!!!";
            endUI.SetActive(true);
        }*/
        if (time <= 0)
        {
            GameObject[] ob = GameObject.FindGameObjectsWithTag("ObjectPrefab");
            int num = ob.Length;
            surviver = " " +num +"명이 탈출에 성공했다!";
            
            if (playernum != 0) 
            {
                result = "도망쳤다..!"; 
            }
            //playernum = 0;
            endUI.SetActive(true);
            //StartCoroutine(EndDelay());
            
            audio.Stop();
        }
        if(time>20) TimeLeft.text = "LEFT TIME : " + Mathf.Ceil(time).ToString();
        else if(time<20)
        {
            string str = "<color=#ff0000>" + "LEFT TIME : " + Mathf.Ceil(time).ToString() + "</color>";
            TimeLeft.text = str;
        }
    }
}
