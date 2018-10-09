using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtr : MonoBehaviour {
    
    public GameObject bullet;

    public Transform firePos;

    public float bulletSpeed;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Fire", 0, bulletSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void Fire()
    {
        StartCoroutine(this.CreateBullet());
    }

    IEnumerator CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }
}
