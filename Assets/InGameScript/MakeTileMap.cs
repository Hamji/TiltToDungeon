using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// define
static class Constants
{
    public const int MAX = 25;

    public const int CENTER = 12;

    public const int MOVEX = 20;
    public const int MOVEY = 10;
}

public class MakeTileMap : MonoBehaviour
{

    // 맵 좌표 25 X 25
    
    public int max_of_map;
    public int many_of_map;

    // 칸
    public Queue<Room> room_queue;
    public List<Room> room_list;

    public GameObject room_form;
    public Transform room_pos;
    // 문
    public GameObject top_door;
    public GameObject left_door;
    public GameObject right_door;
    public GameObject bottom_door;
    
    // 방해물을 만들기 위하여 필요한 부분 
    // 방해물 List
    public Transform interrupts;
    public List<GameObject> inter_list;

    // 방해물
    public GameObject interrupt_0;
    public GameObject interrupt_1;
    public GameObject interrupt_2;


    // Method
    



    // Use this for initialization
    void Start()
    {
        InitElement();

        MakeMap();
        for(int i = 0; i < room_list.Count; i++)
        {
            MakeDoor(room_list[i]);
        }
    //    Debug.Log(room_queue);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void InitElement()
    {
        room_pos.position = new Vector3(0, 0, -3);
        room_queue = new Queue<Room>();
        room_list = new List<Room>();
        many_of_map = 0;

        // 맨 처음 방 생성
        MakeRoom(room_pos);
    }

    // 룸 삭제
    public void DeleteRoom(Room del)
    {
        Destroy(del.charge);
        del = null;
    }

    // 맵초기화 함수
    public void MakeRoom(Transform location)
    {
        Room temp = new Room();
        temp.CopyTransform(location);
   
        temp.charge = Instantiate(room_form, temp.location, location.rotation);
        room_queue.Enqueue(temp);
        room_list.Add(temp);
        //Debug.Log(room_queue.Dequeue()    );
    }

    // 전체 맵 생성 함수
    public Room MakeMap()
    {
        int door_many = Random.Range(1, 3);
        
        // Queue가 비었거나 맵의 갯수가 꽉차면 Stop 
        if (room_queue.Count == 0 || many_of_map == max_of_map)
            return null;
        many_of_map++;

        Room present_room = room_queue.Dequeue();
        
        // room의 위치 저장
        present_room.CopyTransform(room_pos);

        if (many_of_map != 0)
            MakeRoom(room_pos);

        SetDoor(present_room, door_many);

        return present_room;

    }

    public void SetDoor(Room room, int door_many)
    {
        int door_direction = Random.Range(0, 4);
        Room connected_room;

        for(int i = 0; i< door_many; i++)
        {
            room_pos.position = room.location;
            // 0북 1동 2남 3서
            switch (door_direction)
            {
                case 0:
                    room_pos.position = new Vector3(room.location.x,room.location.y + 10, -3);
                    connected_room = MakeMap();
                    if (connected_room != null)
                    {
                        room.door_top = true;
                        connected_room.door_bottom = true;
                    }
                    else
                        return;
                //    room.door_top = true;
                    break;
                case 1:
                    room_pos.position = new Vector3(room.location.x + 20, room.location.y, -3);
                    connected_room = MakeMap();
                    if (connected_room != null)
                    {
                        room.door_right = true;
                        connected_room.door_left = true;
                    }
                    else
                        return;
                    //    room.door_right = true;
                    break;
                case 2:
                    room_pos.position = new Vector3(room.location.x, room.location.y - 10, -3);
                    connected_room = MakeMap();
                    if (connected_room != null)
                    {
                        room.door_bottom = true;
                        connected_room.door_top = true;
                    }
                    else
                        return;
                    //    room.door_bottom = true;
                    break;
                case 3:
                    room_pos.position = new Vector3(room.location.x - 20, room.location.y, -3);
                    connected_room = MakeMap();
                    if (connected_room != null)
                    {
                        room.door_left = true;
                        connected_room.door_right = true;
                    }
                    else
                        return;
                    //    room.door_left = true;
                    break;
            }

            door_direction = (door_direction + 1) % 4;
        }

        
    }

    void MakeDoor(Room room)
    {
        if(room.door_top)
        {
            Vector3 temp = new Vector3(room.location.x, room.location.y + 4, -3);
            Instantiate(top_door, temp, room_pos.transform.rotation);
        }
        if(room.door_bottom)
        {
            Vector3 temp = new Vector3(room.location.x, room.location.y - 4, -3);
            Instantiate(bottom_door, temp, room_pos.transform.rotation);
        }
        if (room.door_left)
        {
            Vector3 temp = new Vector3(room.location.x - 8, room.location.y, -3);
            Instantiate(left_door, temp, room_pos.transform.rotation);
        }
        if (room.door_right)
        {
            Vector3 temp = new Vector3(room.location.x + 8, room.location.y, -3);
            Instantiate(right_door, temp, room_pos.transform.rotation);
        }
    }

    // 방해물을 특정 위치에 생성하며 inter_list의 child 로 만듬
    IEnumerator CreateInterrupt(int x, int y)
    {
    //    Debug.Log("호출");
        Vector3 temp = new Vector3(x, y, -3);
        // 종류
        int kind = Random.Range(0, 3);
        GameObject intemp;

        switch (kind)
        {
            case 0:
                intemp = Instantiate(interrupt_0, temp, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
                break;
            case 1:
                intemp = Instantiate(interrupt_1, temp, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
                break;
            case 2:
                intemp = Instantiate(interrupt_2, temp, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
                break;
            default:
                break;
        }
      
        yield return null;
    }

    // CreateInterrupt 호출
    public void Interrupting(int x, int y)
    {
      
    }

    public void InitInterrupt()
    {

    }

    
}
public class Room
{

    public Vector3 location;

    public GameObject charge;
    // 문
    public bool door_top;
    public bool door_bottom;
    public bool door_left;
    public bool door_right;

    // check 여부
    public bool check;
    // Use this for initialization
    public Room()
    {
        door_top = false;
        door_bottom = false;
        door_left = false;
        door_right = false;

        check = false;

    }

    public void CopyTransform(Transform location)
    {
        this.location = new Vector3(location.transform.position.x, location.transform.position.y,-3);
    }
}


