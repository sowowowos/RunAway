using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LeftPlayer : MonoBehaviourPunCallbacks
{
    public Text PlayerMembers;
    public int n=0;
    public int v_int = 0;
    public string v;
    // Start is called before the first frame update
    void Start()
    {
        v = PhotonNetwork.PlayerList.Length.ToString();
        v_int = int.Parse(v) -1;

        PlayerMembers.text = "Left Object : " + v_int;

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("ObjectPrefab");
        n = temp.Length;
        //n = GetComponent<ObjectController>().diemem;
        //v_int = int.Parse(v);

        //v_int -= n;

        PlayerMembers.text = "Left Object : " + n;

    }
}
