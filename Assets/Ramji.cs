using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramji : MonoBehaviour {

    public int hp;
    public int roomnumber;

    public bool dead;
    public bool awake;

    public Transform pattern1_trans;
    public Transform pattern2_trans;
    public Transform pattern3_trans;

    public GameObject player;


    // Use this for initialization
    void Start () {
        dead = false;
        awake = true ;
        hp = 500;

        GetComponent<RamjiPattern1>().enabled = false;
        GetComponent<RamjiPattern2>().enabled = false;
        GetComponent<RamjiPattern3>().enabled = false;


    }
	
    

	// Update is called once per frame
	void Update () {
       
        if (IsAwake())
        {
            if (IsAlive())
                PatternChange();
            else
                Dead();
        }
        

	}

    public void Dead()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;

        GetComponent<RamjiPattern1>().enabled = false;
        GetComponent<RamjiPattern1>().CancelInvoke();

        GetComponent<RamjiPattern2>().enabled = false;
        GetComponent<RamjiPattern1>().CancelInvoke();

        GetComponent<RamjiPattern3>().enabled = false;
        GetComponent<RamjiPattern1>().CancelInvoke();
    }

    public void PatternChange()
    {
        if (hp <= 200)
        {
            //패턴 3
            if (!IsPattern3())
            {
                GetComponent<RamjiPattern1>().enabled = false;
                GetComponent<RamjiPattern1>().CancelInvoke();

                GetComponent<RamjiPattern2>().enabled = false;
                GetComponent<RamjiPattern2>().CancelInvoke();

                GetComponent<RamjiPattern3>().enabled = true;
            }
        }
        else if (200 < hp && hp <= 300)
        {
            // 패턴 2
            if (!IsPattern2())
            {
                GetComponent<RamjiPattern1>().enabled = false;
                GetComponent<RamjiPattern1>().CancelInvoke();
                GetComponent<RamjiPattern2>().enabled = true;
            }
        }
        else
        {
            // 패턴 1
            if (!IsPattern1())
            {
                GetComponent<RamjiPattern1>().enabled = true;
            }
        }

    }

    public bool IsPattern1()
    {
        return GetComponent<RamjiPattern1>().enabled;
    }

    public bool IsPattern2()
    {
        return GetComponent<RamjiPattern2>().enabled;
    }

    public bool IsPattern3()
    {
        return GetComponent<RamjiPattern3>().enabled;
    }

    public bool IsBullet(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
            return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBullet(collision))
        {
            hp = hp - collision.transform.GetComponent<BulletControl>().damage;
            //Debug.Log(hp);
        }

    }

    public bool IsAlive()
    {
        if (hp > 0)
            return true;

        return false;
    }

    public bool IsAwake()
    {
        return awake;
    }
}
