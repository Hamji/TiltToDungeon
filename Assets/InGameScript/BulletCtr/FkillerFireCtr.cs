using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FkillerFireCtr : MonoBehaviour {

    public GameObject bullet;

    public Transform firePos;

    float bulletRepeat = 0.4f;
    
    // Use this for initialization
    void Start () {
		InvokeRepeating("Fire", 0, bulletRepeat);
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
        
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }
}
