using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ObjectController : MonoBehaviourPun
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera, CurrentCam, firstCam;

    public Camera[] arrCam; //카메라 요소들을 추가한다.

    int nCamCount = 2;
    int nNowCam = 0;

    private Rigidbody myRigidbody;
    private RigidbodyConstraints originalConstraints;

    [SerializeField]
    private int ObjHP;

    public GameObject thisObj;

    public bool isDie, isJumping, isFreeze,isMoved_o;
    public bool isCamChange = false;

    private GameObject HPUI,destroyD;

    public Vector3 vec;//
    void Start()
    {
        if (!photonView.IsMine) 
        { 
            return; 
        }
        //particles = GetComponent<ParticleSystem>().Play();

        /*System.Random random = new System.Random();
        int rand_map = Random.Range(0, 2);
        if(rand_map == 0)
        {
            transform.position = new Vector3(1f, 1f, 25f);
        }
        else
        {
            transform.position = new Vector3(60f, 1f, 25f);
        }*/
        /*gameObject.transform.position = GameObject.Find("원하는오브젝트").transform.position;
            // 위 처럼 Object 의 이름 찾으셔서 해당 이름 오브젝트의 포지션으로 이동시키셔도 되고
            // 아니면 선언하셔서 gameObject.transform.position = 선언Object.transform.position
            // 처럼 하셔도 되고 방법은 많습니다. 이렇게 하시거나
            gameObject.transform.position = new Vector3(새로운, Vector3의, 좌표값으로);
            // 새로운 위치, 즉 정해준 위치로 이동시키려면 위처럼 하셔도 되구요..*/

        //thisObj.transform.position = GameObject.Find("Soldier_demo").transform.position + Vector3.one*5;

        /*Vector3 pos_s;
        //pos_s = GameObject.FindGameObjectsWithTag("soldier").transform.position;
        
        pos_s = GameObject.Find("Soldier_demo").GetComponent<SoidierController>().transform.position;
        Debug.Log(pos_s+"@@@@@@");
        //GameObject.Find("TimeLeft").GetComponent<TimerController>();*/
        //vec = GameObject.Find("Soldier_demo").GetComponent<SoidierController>().pos_;
        //transform.Translate(vec + Vector3.one, Space.World);
        //thisObj.transform.position = vec;

        /* SoidierController call = GameObject.Find("Soldier_demo").GetComponent<SoidierController>();
         isMoved_o = call.isMoved;*/
        SoidierController mov = GetComponent<SoidierController>();
        theCamera = arrCam[0];
        HPUI = GameObject.FindGameObjectWithTag("HPUI");
        destroyD = GameObject.FindGameObjectWithTag("destroyD");
        isDie = false;
        isJumping = false;
        isFreeze = false; //

        myRigidbody = GetComponent<Rigidbody>();

        CurrentCam = theCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && !isDie)
        {
            return;
        }

        HPUI.SetActive(false);
        destroyD.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();
        if (timer.time > 0)
        {
            
            Jump();
            Move();
            Stop_Move();
            //LastChance();
            //CameraChange();
            CameraRotation();
            CharacterRotation();
            Camera.main.GetComponent<CameraController>().target = CurrentCam.GetComponent<Transform>();
        
        }
        

    }

/*    private void CameraChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ++nNowCam;
            if (nNowCam >= nCamCount)
            {
                nNowCam = 0;
            }

            for (int i = 0; i < arrCam.Length; ++i)
            {
                arrCam[i].enabled = (i == nNowCam);
                theCamera = arrCam[i+1];
            }

        }
    }*/

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        
    }
    void LastChance()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        SoidierController mov = GetComponent<SoidierController>();
        if (mov.isMoved == true)
            thisObj.transform.position = new Vector3(-15f, 1f, 70f);
        //}
    }
    void Stop_Move()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isFreeze == false)
            {
                myRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                isFreeze = true;
            }
            else
            {
                myRigidbody.constraints = originalConstraints;
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                isFreeze = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            OnDamage();

        }
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(_characterRotationY));

    }

    private void CameraRotation()
    {
        
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        CurrentCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isCamChange == false)
            {
                CurrentCam = firstCam;
                isCamChange = true;
            }
            else
            {
                CurrentCam = theCamera;
                isCamChange = false;
            }
        }

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ++nNowCam;
            if (nNowCam >= nCamCount)
            {
                nNowCam = 0;
            }

            for (int i = 0; i < arrCam.Length; ++i)
            {
                arrCam[i].enabled = (i == nNowCam);
                CurrentCam = arrCam[i + 1];
            }

        }*/
    }
    public void OnDamage()
    {
        if (ObjHP < 0 || isDie)
            return;

        ObjHP -= 10;

        if (ObjHP <= 0 && isDie == false)
        {
            ObjHP = -100;
            isDie = true;
            
            TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();

            if (timer.player4 == "")
                timer.player4 = PhotonNetwork.PlayerList.Length - 1 + ". " + photonView.Owner.NickName;
            else if (timer.player3 == "" && timer.player4 != "")
                timer.player3 = PhotonNetwork.PlayerList.Length -2 + ". " + photonView.Owner.NickName;

            else if (timer.player2 == ""  && timer.player3 != "")
                timer.player2 = PhotonNetwork.PlayerList.Length - 3 + ". " + photonView.Owner.NickName;

            else if (timer.player1 == "" && timer.player3 != "" && timer.player2 != "")
                timer.player1 = PhotonNetwork.PlayerList.Length - 4 + ".  " + photonView.Owner.NickName;


            Destroy(this.gameObject);
        }
    }
}
