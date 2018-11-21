using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FkillerBulletCtr : MonoBehaviour {

    // 데미지
    public int damage = 1;

    // 총알속도
    public float speed = 2000.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(Vector2.up * 10 * Time.deltaTime);
        
    }

    public void HandlingCollision(Collision2D collision)
    {
        if (collision.transform.tag != "Player" && collision.transform.tag != "Bullet")
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlingCollision(collision);
         //   Debug.Log("총알 충돌");
    }
}
