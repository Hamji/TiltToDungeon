using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamjiPattern2 : MonoBehaviour {

    public Transform firePos1;
    public Transform firePos2;

    public GameObject bullet;

    bool direction1;
    bool direction2;

	// Use this for initialization
	void Start () {
        direction1 = false;
        direction2 = false;
        InvokeRepeating("Fire", 0, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        MoveFirePos1();
        MoveFirePos2();
    }

    public void MoveFirePos1()
    {
        if (! direction1)
        {
            firePos1.Translate(Vector3.right * 5 * Time.deltaTime);

            if (firePos1.localPosition.x < -9)
                direction1 = true;
        }
        else
        {
            firePos1.Translate(Vector3.right * -5 * Time.deltaTime);

            if (firePos1.localPosition.x > 9)
                direction1 = false;
        }
           
     //   Debug.Log(firePos1.localPosition);
       
    }
    public void MoveFirePos2()
    {
        if(! direction2)
        {
            firePos2.Translate(Vector3.left * 5 * Time.deltaTime);

            if (firePos2.localPosition.x > 9)
                direction2 = true;
        }
        else
        {
            firePos2.Translate(Vector3.left * -5 * Time.deltaTime);

            if (firePos2.localPosition.x < -9)
                direction2 = false;
        }
    }


    void Fire()
    {
        //Debug.Log("호출");
        StartCoroutine(this.CreateBullet());
    }


    IEnumerator CreateBullet()
    {

        Instantiate(bullet, firePos1.position, firePos1.rotation);
        Instantiate(bullet, firePos2.position, firePos2.rotation);

        yield return null;
    }
}
