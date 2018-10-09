using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
        
    public int hp;
    public InGameManager manager;

    // Use this for initialization
    void Start () {
        hp = 0;

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        manager.WhatCollider(collision);
//        Vector2 pos = collision.contacts[0].point;
//        Debug.Log(pos);
//        Debug.Log(collision.gameObject.tag);
//        Debug.Log(collision.gameObject.name);
       
    }

    // Update is called once per frame
    void Update () {
        MoveCharactor();
    }

    public void MoveCharactor()
    {
        this.transform.Translate(Vector3.up * 5 * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            this.transform.Rotate(Vector3.back * 600 * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow))
            this.transform.Rotate(Vector3.forward * 600 * Time.deltaTime);
      
        
    }
}
