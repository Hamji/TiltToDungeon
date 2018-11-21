using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamjiPattern1 : MonoBehaviour {

    public GameObject bullet;

    public Transform axis;
    public Transform firePos;

    // false 면 시계방향 true 면 반시계
    public bool direction = false;

    public float bulletRepeat = 0.4f;


	// Use this for initialization
	void Start () {
        InvokeRepeating("Fire", 0, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        RotateAxis();
       
	}

    public void RotateAxis()
    {
        if (! direction)
        {
            if (axis.rotation.z < 0.5)
                axis.Rotate(Vector3.forward * 30 * Time.deltaTime);
            else
                direction = true;
        }
        else
        {
            if (axis.rotation.z > -0.5)
                axis.Rotate(Vector3.forward * -30 * Time.deltaTime);
            else
                direction = false;
        }


        //Debug.Log(axis.rotation.z);
        
    }

    void Fire()
    {
        //Debug.Log("호출");
        StartCoroutine(this.CreateBullet());
    }

    IEnumerator CreateBullet()
    {
        //Debug.Log("총알 생성");
        
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }
}
