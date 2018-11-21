using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fkillerPattern : MonoBehaviour {

    public int HP;
    public int roomNum;

    // 방에 들어오면 활동시작
    public bool awake;
    public bool dead;
    


	// Use this for initialization
	void Start () {
        awake = true;
        dead = false;
	}
	
    public bool IsAwake()
    {
        if (awake)
            return true;
        else
            return false;
    }

	// Update is called once per frame
	void Update () {

        if(IsAwake())
        {
            if (!this.GetComponent<FkillerFireCtr>().enabled)
                this.GetComponent<FkillerFireCtr>().enabled = true;
            if (IsAlive())
                Rotate();
            else
                Dead();
        }
    }

    public void Dead()
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        this.GetComponent<FkillerFireCtr>().CancelInvoke();
        this.GetComponent<PolygonCollider2D>().enabled = false;
        
        dead = true;       
    }

    public void Rotate()
    {
        this.transform.Rotate(Vector3.back * 200 * Time.deltaTime);
    }

    public bool IsAlive()
    {
        if (HP > 0)
            return true;

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsBullet(collision))
        {
            HP = HP - collision.transform.GetComponent<BulletControl>().damage;
            //Debug.Log(HP);
        }
        
    }

    public bool IsBullet(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")
            return true;

        return false;
    }
}
