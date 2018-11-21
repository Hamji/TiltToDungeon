using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamjiPattern3 : MonoBehaviour {

    public GameObject bullet;

    public Transform firePos;

    public Transform playerTrans;

    float bulletRepeat = 0.7f;

    // Use this for initialization
    void Start () {
        //bullet.transform.localScale += new Vector3(0.5f, 0.5f, 0);

        InvokeRepeating("Fire", 0, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Fire()
    {
        //Debug.Log("호출");
        StartCoroutine(this.CreateBullet());
    }

    IEnumerator CreateBullet()
    {
        //Debug.Log("총알 생성");
        Vector3 dirToTarget = this.playerTrans.position - firePos.position;
        
        firePos.forward = dirToTarget;

        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }

}
