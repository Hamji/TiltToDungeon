using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    // 스테이지 클리어 체크
    public bool stage_clear;

    // 현재 위치 행과 열
    public int current_r;
    public int current_c;
    
    // 맵 충돌 부분.
    
    public Player player;

  

    // Use this for initialization
    void Start()
    {
      
    }

    // 맵에서 충돌 났을때.
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    // 충돌벽 위치를 카메라 위치로 갱신
    public void ChangeCollider()
    {

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
        
        
    }

    
    public void WhatCollider(Collision2D collision)
    {
    //    Debug.Log(collision.transform.name);
        
        return;
    }

}
