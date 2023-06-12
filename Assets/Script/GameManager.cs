using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{

    public string[] objName = new string[15];
    public int rand, rand_map;

    void Start()
    {
        

/*        switch (rand_map)
        {

            case 0:
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Instantiate("Soldier_demo", new Vector3(1.5f, 1f, 14f), Quaternion.identity, 0);
                }
                else
                {
                    PhotonNetwork.Instantiate(objName[rand], new Vector3(1f, 1f, 25f), Quaternion.identity, 0);
                }
                break;
            case 1:
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Instantiate("Soldier_demo", new Vector3(55f, 1f, 25f), Quaternion.identity, 0);
                }
                else
                {
                    PhotonNetwork.Instantiate(objName[rand], new Vector3(60f, 1f, 25f), Quaternion.identity, 0);
                }
                break;

        }*/
        System.Random random = new System.Random();
        //rand_map = Random.Range(0, 2);
        if(/*(PhotonNetwork.PlayerList.Length * PhotonNetwork.PlayerList.Length)* */Time.deltaTime % 2 == 0)
        {
            rand_map = 0;
        }
        else
        {
            rand_map = 1;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (rand_map == 0)
            {
                PhotonNetwork.Instantiate("Soldier_demo", new Vector3(1.5f, 1f, 14f), Quaternion.identity, 0);
            }
            else if (rand_map == 1)
            {
                PhotonNetwork.Instantiate("Soldier_demo", new Vector3(45f, 1f, 25f), Quaternion.identity, 0);

            }
            //PhotonNetwork.Instantiate("Soldier_demo", new Vector3(a, 1f, b), Quaternion.identity, 0);
        }
        else
        {
            rand = Random.Range(0, 14);
            if (rand_map == 0)
            {
                PhotonNetwork.Instantiate(objName[rand], new Vector3(1f, 1f, 25f), Quaternion.identity, 0);
            }
            else if (rand_map == 1)
            {
                PhotonNetwork.Instantiate(objName[12], new Vector3(60f, 1f, 25f), Quaternion.identity, 0);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*    public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
    */


    /*
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }*//*

    public void LeaveRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;

        PhotonNetwork.LoadLevel("Lobby");
    }
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        Debug.Log(PhotonNetwork.NickName + "접속");
        PhotonNetwork.LoadLevel("DemoAsteroids - GameScene");
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadArena();
        }
    }*/


}
