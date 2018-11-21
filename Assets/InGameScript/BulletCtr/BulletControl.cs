using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    // 데미지
    public int damage;

    // 총알속도
    public float speed = 2000.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlingCollision(collision);
    //    Debug.Log("총알 충돌");
    }
    
    // Use this for initialization
    void Start () {
        //GetComponent<Rigidbody2D>().AddForce(transform.up * 700.0f);  
        
    }
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(Vector2.up * 20 * Time.deltaTime);
	}

    public void HandlingCollision(Collision2D collision)
    {
        if(collision.transform.tag != "Player" && collision.transform.tag != "Bullet")
            Destroy(this.gameObject);
    }

}
