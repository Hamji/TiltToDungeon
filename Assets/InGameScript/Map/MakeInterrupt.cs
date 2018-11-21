using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInterrupt : MonoBehaviour {

    public Transform interPos;

    public GameObject interrupt;
    
	// Use this for initialization
	void Start () {
        Interrupting();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interrupting()
    {
        StartCoroutine(this.CreateInterrupt());
    }

    IEnumerator CreateInterrupt()
    {
        GameObject intemp = Instantiate(interrupt,interPos.position,interPos.rotation );
        intemp.transform.parent = interPos;
        yield return null;
    }
}
