using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class resultController : MonoBehaviour
{
    public Text result, player1, player2, player3, player4, surviver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();
        result.text = timer.result;
        player1.text = timer.player1;
        player2.text = timer.player2;
        player3.text = timer.player3;
        player4.text = timer.player4;
        surviver.text = timer.surviver;
    }
}
