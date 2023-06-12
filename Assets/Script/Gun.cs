using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private int bulletsPerMag; //탄창 당 탄수
    private int bulletsTotal; //전체 가지고 있는 총탄수
    [SerializeField]
    private int currentBullets; //현재 탄창에 남아있는 총탄 수
    
    private Animation animator;
    
    
    private float range = 15f;        //사정거리
    [SerializeField]
    private float fireRate;     //연사속도
    private float damage;       //데미지
    //[SerializeField]
    //private float fireTimer;    //총탄과 총탄 사이의 발사 간격 설정

    //public Transform shootPoint;   //총탄이 실제 발사되는 지점

    [SerializeField]
    private Camera theCam;  //카메라 시점에서 정 중앙에 발사할거여서

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = bulletsPerMag;
        animator = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentBullets > 0)
            {
                Fire();
                //animator.SetTrigger("IsShoot");
            }
/*            if(fireTimer< fireRate)
            {
                fireTimer += Time.deltaTime;
            }*/
        }
      
    }
    private void Fire()
    {
        //if (fireTimer < fireRate) return;
        RaycastHit hit;
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hit, range))
        {
            Debug.Log("ㄸㅐ림");
            Debug.LogWarning(hit.transform.name);  //맞은 물체의 이름을 표시 >>>>맞은 물체가 Destroy(hit.transform.name)
        }
    }

}
