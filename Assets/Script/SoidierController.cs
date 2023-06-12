using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoidierController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    public int Hp;

    [SerializeField]
    private float cameraRotationLimit, timeshow;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private GameObject particle,thisSol;
    //private GameObject thisSol;////////

    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigidbody;
    private Animator animator;
    public bool isRunCheck;

    public bool CursorBool, isDie_s, isMoved;

    public AudioSource[] Sound = new AudioSource[2];
    public AudioClip[] Clips = new AudioClip[2];
    private GameObject HPUI,destroyD,hidden;
    public Image HPimage;
    public Text Hptext;

    [SerializeField]
    public Transform target;
    public GameObject endUI;
    public GameObject obj_move;


    private int playernum;
    //public Vector3 pos_;///////////
    void Start()
    {

        if (photonView.IsMine)
        {
            //endUI = GameObject.FindGameObjectWithTag("END");
            //endUI.SetActive(false);
            
            //pos_ = thisSol.GameObject.transform.position;  //pos_에다가 위치값 저장////////

            hidden = GameObject.FindGameObjectWithTag("hidden");
            HPUI = GameObject.FindGameObjectWithTag("HPUI");
            destroyD = GameObject.FindGameObjectWithTag("destroyD");
            destroyD.SetActive(true);

            HPimage = GameObject.FindGameObjectWithTag("Hpimage").GetComponent<Image>();
            Hptext = GameObject.FindGameObjectWithTag("Hptext").GetComponent<Text>();
            Camera.main.GetComponent<CameraController>().target = theCamera.transform;
            Hp = 100;
            Cursor.lockState = CursorLockMode.Locked;
            isDie_s = false;
            CursorBool = false;
            isMoved = false; //마지막 이동
            isRunCheck = true;
            animator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody>();

            for(int i =0;i<2; i++)
            {
                Sound[i] = this.gameObject.AddComponent<AudioSource>() as AudioSource;
                Sound[i].Stop();
            }
            Clips[0] = Resources.Load("Steyraug-fire", typeof(AudioClip)) as AudioClip;
            Clips[1] = Resources.Load("walksound", typeof(AudioClip)) as AudioClip;
            for (int j = 0; j < 2; j++)
            {
                Sound[j].clip = Clips[j];
                Sound[j].loop = false;
                Sound[j].playOnAwake = false;
            }
            Sound[1].loop = true;
        }
        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HPUI.SetActive(true);
            
            
            CursorVisible(); 
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

            TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();
            GameObject[] temp = GameObject.FindGameObjectsWithTag("ObjectPrefab");
            int playernum = temp.Length;
            if (timer.time > 0 && playernum !=0 && Hp >0)
            {
                Fire();
                Move();
                CameraRotation();
                CharacterRotation();
                Item_F();


            }
            if(Hp <= 0)
            {
                isDie_s = true;
            }
            Destroy(destroyD,5f);

        }

    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "hidden") // 해당 조건은 태그로 하셔도되고 원하시는 대로
        {
            //gameObject.transform.position = new Vector3(0, 0, 0);
            /*AsyncOperation async;
            async = SceneManager.LoadSceneAsync("New");*/
            gameObject.transform.position = new Vector3(-5f, 1f, 70f);
            isMoved = true;
            //GameObject[] objects = GameObject.FindGameObjectsWithTag("ObjectPrefab");
            /*GameObject new_o =GameObject.Find(objects[1]).name;
            var goName : String = gameObject.Find(objects[1]).name;
            goName.transform.position = new Vector3(-15f, 1f, 70f);*/

            //GameObject.Find("Soldier_demo").GetComponent<SoidierController>().transform.position;
/*
            GameObject go = GameObject.FindGameObjectsWithTag("ObjectPrefab");
            go[1].transform.position = new Vector3(-15f, 1f, 70f);

            GameObject[] obj_all=GameObject.FindGameObjectsWithTag("ObjectPrefab");
            int num_all = obj_all.Length;
            int i = 0;
            for(i=0;i< num_all; i++){
                obj_move = GameObject.FindGameObjectsWithTag("ObjectPrefab");
                obj_move.transform.position = new Vector3(-15f, 1f, 70f);
            }*/

            //obj_all = new Vector3(-15f, 1f, 70f);

            /*gameObject.transform.position = GameObject.Find("원하는오브젝트").transform.position;
            // 위 처럼 Object 의 이름 찾으셔서 해당 이름 오브젝트의 포지션으로 이동시키셔도 되고
            // 아니면 선언하셔서 gameObject.transform.position = 선언Object.transform.position
            // 처럼 하셔도 되고 방법은 많습니다. 이렇게 하시거나
            gameObject.transform.position = new Vector3(새로운, Vector3의, 좌표값으로);
            // 새로운 위치, 즉 정해준 위치로 이동시키려면 위처럼 하셔도 되구요..*/

        }
    }
    private void CursorVisible()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            CursorBool = !CursorBool;
            if (CursorBool)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsRun", true);
            isRunCheck = true;
            Sound[1].Play();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("IsRun", false);
            isRunCheck = false;
            Sound[1].Stop();
        }


        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    private void Item_F()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Hp > 0)
            {
                //TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();

                RaycastHit hit;
                

/*
                PhotonNetwork.Instantiate("bullet", theCamera.transform.position,
                    Quaternion.LookRotation(theCamera.transform.forward + new Vector3(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f), 0)));*/

                if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, 1000f))
                {
                    if (hit.collider.tag != "ObjectPrefab")
                    {
                        //Hp += 10;
                        GameObject clone = Instantiate(obj_move, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(clone, 5f);
                        //hit.collider.gameObject.GetComponent<ObjectController>().OnDamage();

                    }
                }



            }
        }
    }
    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (Hp > 0)
            {
                Sound[0].Play();
                TimerController timer = GameObject.Find("TimeLeft").GetComponent<TimerController>();
                
                RaycastHit hit;
                if(timer.time > 20.0f)
                {   Hp -= 10;
                    if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, 1000f))
                    {
                        if (hit.collider.tag == "ObjectPrefab")
                        {
                            
                            Hp += 10;
                            //hit.collider.gameObject.GetComponent<ObjectController>().OnDamage();

                        }
                    }

                    

                    HPimage.fillAmount = Hp / 100f;
                    Hptext.text = Hp + " ";
                }

                animator.SetTrigger("IsShoot");
                
                PhotonNetwork.Instantiate("bullet", theCamera.transform.position, 
                    Quaternion.LookRotation(theCamera.transform.forward + new Vector3(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f), 0)));

                if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, 1000f))
                {
                    if(hit.collider.tag == "ObjectPrefab")
                    {
                        //Hp += 10;
                        GameObject clone = Instantiate(particle, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(clone, 2f);
                        hit.collider.gameObject.GetComponent<ObjectController>().OnDamage(); 

                    }
                }
            }
        }
    }

    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
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

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
