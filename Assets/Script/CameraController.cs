using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class CameraController : MonoBehaviourPunCallbacks
{
    public Transform target;
    private void Start()
    {
        
    }
    void Update()
    {

        this.transform.position = target.position;
        this.transform.rotation = target.rotation;
        
    }


}
