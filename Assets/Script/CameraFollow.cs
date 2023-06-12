using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviourPun
{
    public GameObject target;
    public float smoothing = 5.0f;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        offset = transform.position - target.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Vector3 newCamPos = target.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
    }
}
