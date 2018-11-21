using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
        
    public int hp;

    // 현재 플레이어가 있는 방 번호
    public int roomnumber;
    public InGameManager manager;


    // Use this for initialization
    void Start () {
        
        roomnumber = 0;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsEnemyBullet(collision))
            hp -= collision.transform.GetComponent<FkillerBulletCtr>().damage;
//        Vector2 pos = collision.contacts[0].point;
//        Debug.Log(pos);
//        Debug.Log(collision.gameObject.tag);
//        Debug.Log(collision.gameObject.name);
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        manager.WhatTrigger(other);
    }

    // Update is called once per frame
    void Update () {
        if (IsAlive())
            MoveCharactor();
        else
            Dead();
    }

    public void Dead()
    {
        this.GetComponent<FireCtr>().CancelInvoke();
    }
    
    public void MoveCharactor()
    {
        this.transform.Translate(Vector3.up * 5 * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            this.transform.Rotate(Vector3.back * 600 * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow))
            this.transform.Rotate(Vector3.forward * 600 * Time.deltaTime);
    }

    public bool IsEnemyBullet(Collision2D other)
    {
        if (other.transform.tag == "enemyBullet")
            return true;
        else
            return false;
    }

    public bool IsAlive()
    {
        if (hp > 0)
            return true;
        else
            return false;
    }
}
