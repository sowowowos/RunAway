using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Bullet : MonoBehaviourPunCallbacks
{
    float bulletSpeed = 30;

    private RaycastHit hitInfo;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(this.gameObject, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);

    }



}
