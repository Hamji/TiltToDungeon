using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class InGameManager : MonoBehaviour {

    // 스테이지 클리어 체크
    public bool stage_clear;

    // 맵 충돌 부분.
    
    public Player player;
    public GameObject camera;
    public GameObject FadeInOut;



    // Use this for initialization
    void Start()
    {
        //stage_clear = true;
    }

    
   
    // 스테이지클리어 값 return 
    public bool IsClear()
    {
        List<Room> tList = GetComponent<MakeTileMap>().room_list;

        if (IsRoomClear(tList[player.roomnumber]))
            return true;
        else
            return false;
    }

    public bool IsRoomClear(Room other)
    {
        for(int i = 0; i < other.enemy_list.Count; i++)
        {
            if (other.enemy_list[i].GetComponent<fkillerPattern>().dead == false)
                return false;
        }
        return true;
    }

    // 문인지 참 거짓
    public bool IsDoor(string tag)
    {
        if (tag == "door")
            return true;
        else 
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


        
    }

    public void WhereIAm()
    {
        List<Room> roomList = GetComponent<MakeTileMap>().room_list;

        Vector3 position = player.transform.position;

        for(int i = 0; i < roomList.Count; i++)
        {
            float room_x = roomList[i].charge.transform.position.x;
            float room_y = roomList[i].charge.transform.position.y;

            if (room_x - 9 < position.x && position.x < room_x + 9)
                if (room_y - 5 < position.y && position.y < room_y + 5)
                {
                    player.roomnumber = i;
                    return;
                }
        }
    }

    // 어떤 방향 문인지
    public int KindOfDoor(string name)
    {
        if (name == "door_top")
            return 0;
        if (name == "door_right")
            return 1;
        if (name == "door_bottom")
            return 2;
        if (name == "door_left")
            return 3;

        return -1;
    }   

    // 방 위치를 바꾸는 함수
    public void ChangeCurrentLocation(Collider2D other)
    {
        int direction = KindOfDoor(other.transform.name);
        
        switch(direction)
        {
            case 0:
                FadeInOut.GetComponent<Animator>().SetTrigger("MoveMap");
                
                player.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 3, -3);
                RefreshCameraPosition(other.transform.parent.transform.position.x, other.transform.parent.transform.position.y + 10);
                break;
            case 1:
                FadeInOut.GetComponent<Animator>().SetTrigger("MoveMap");
                
                player.transform.position = new Vector3(other.transform.position.x + 6, other.transform.position.y, -3);
                RefreshCameraPosition(other.transform.parent.transform.position.x + 20, other.transform.parent.transform.position.y);
                break;
            case 2:
                FadeInOut.GetComponent<Animator>().SetTrigger("MoveMap");
                
                player.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 3, -3);
                RefreshCameraPosition(other.transform.parent.transform.position.x, other.transform.parent.transform.position.y - 10);
                break;
            case 3:
                FadeInOut.GetComponent<Animator>().SetTrigger("MoveMap");
                
                player.transform.position = new Vector3(other.transform.position.x - 6, other.transform.position.y, -3);
                RefreshCameraPosition(other.transform.parent.transform.position.x - 20, other.transform.parent.transform.position.y);
                break;
            default:
                break;
        }
        FadeInOut.GetComponent<Animator>().SetTrigger("GoIdle");

        

        WhereIAm();

        Room temp = GetComponent<MakeTileMap>().room_list[player.roomnumber];
        EnemyAwake(temp);
        BossAwake();
        //Debug.Log(player.roomnumber);

    }

    public void EnemyAwake(Room refer)
    {
        if (refer.clear)
            return;
        else
        {
            for (int i = 0; i < refer.enemy_list.Count; i++)
                refer.enemy_list[i].GetComponent<fkillerPattern>().awake = true;
        }
    }

    public void BossAwake()
    {
        int bossRoomNum = this.GetComponent<MakeTileMap>().bossInfo.GetComponent<Ramji>().roomnumber;

        if(bossRoomNum == player.roomnumber)
        {
            this.GetComponent<MakeTileMap>().bossInfo.GetComponent<Ramji>().awake = true;
        }
    }

    public void Waiting()
    {
        // 그냥 지연시간을 위한 함수일뿐.
    }

    public void RefreshCameraPosition(float x, float y)
    {
        camera.transform.position = new Vector3(x, y, camera.transform.position.z);
    }

    public void WhatTrigger(Collider2D other)
    {
        if (IsDoor(other.transform.tag))
            if(IsClear())
                ChangeCurrentLocation(other);

            
    }
}
