using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    // 스테이지 클리어 체크
    public bool stage_clear;

    // 현재 위치 행과 열
    public int current_r;
    public int current_c;
    public Map[,] maps;

    // 맵 충돌 부분.
    public GameObject map_collider;
    public Player player;


    // Use this for initialization
    void Start()
    {
        maps = new Map[25, 25];
        stage_clear = true;

        // 현재위치 초기화
        current_c = 12;
        current_r = 12;
    }

	// Update is called once per frame
	void Update () {
       
	}

    // 맵에서 충돌 났을때.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(map_collider.transform.position);
    }

    // 충돌벽 위치를 카메라 위치로 갱신
    public void ChangeCollider()
    {
        Vector2 cam_position = this.transform.position;
        cam_position.x -= 0.5f;
        cam_position.y -= 0.5f;
        
        map_collider.transform.position = cam_position;
    }

    // 스테이지클리어 값 return 
    public bool IsClear()
    {
        return stage_clear;
    }

    // 문인지 참 거짓
    public bool IsDoor(string tag)
    {
        if (tag == "door")
            return true;
        else 
            return false;
    }

    // 사용 가능한 문인지 check 
    public bool IsAvailableDoor(string name)
    {
        if (name == "top_door_collider" && maps[current_r, current_c].door_top == true)
            return true;
        if (name == "bottom_door_collider" && maps[current_r, current_c].door_bottom == true)
            return true;
        if (name == "right_door_collider" && maps[current_r, current_c].door_right == true)
            return true;
        if (name == "left_door_collider" && maps[current_r, current_c].door_left == true)
            return true;

        return false;
    }

    public int CalculateX(int c)
    {
        return (c - 12) * 20;
    }

    public int CalculateY(int r)
    {
        return -(r - 12) * 10;
    }


    public void HandlingDoor(Collision2D collision)
    {
        if (!IsClear())
            return;


        if (IsAvailableDoor(collision.transform.name))
            ChangeCurrentLocation(collision.transform.name);

        
    }

    // 방 위치를 바꾸는 함수
    public void ChangeCurrentLocation(string door)
    {
        
        if (door == "top_door_collider")
        {
            current_r--;
            player.transform.position = new Vector3(CalculateX(current_c) + 0.5f, CalculateY(current_r) - 2, -3);
            //Debug.Log("위");
        }
        else if (door == "bottom_door_collider")
        {
            current_r++;
            player.transform.position = new Vector3(CalculateX(current_c) + 0.5f, CalculateY(current_r) + 3, -3);
            //Debug.Log("아래");
        }
        else if (door == "left_door_collider")
        {
            current_c--;
            player.transform.position = new Vector3(CalculateX(current_c) + 5, CalculateY(current_r) + 0.5f, -3);
            //Debug.Log("좌");
        }
        else if (door == "right_door_collider")
        {
            current_c++;
            player.transform.position = new Vector3(CalculateX(current_c) - 6, CalculateY(current_r) + 0.5f, -3);
            //Debug.Log("우");
        }

        this.transform.position = new Vector3(CalculateX(current_c) + 0.5f, CalculateY(current_r)+0.5f, -10);
        
        ChangeCollider();
    }

    
    public void WhatCollider(Collision2D collision)
    {
        //Debug.Log(name);
        if (IsDoor(collision.transform.tag))
            HandlingDoor(collision);

        return;
    }

}
