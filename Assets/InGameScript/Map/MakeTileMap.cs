using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// define
static class Constants
{
    // 일반 방
    public const int GENERALROOM = 0;
    // 보스 방
    public const int BOSSROOM = 1;
    // 이벤트 방
    public const int EVENTROOM = 2;


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

    // 방 부모, 형태, 모습
    public Transform room_parent;
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

    // 적
    public GameObject enemy1;

    public GameObject Boss;
    public GameObject bossInfo;

    // enemy many per room
    public int max_enemy;

    // Method
    



    // Use this for initialization
    void Start()
    {
        /////////////////////방 생성////////////////////////
        while(many_of_map != max_of_map)
        {
            InitElement();
            MakeMap();
            if (max_of_map != many_of_map)
                for (int i = 0; i < room_list.Count; i++)
                    DeleteRoom(room_list[i]);
        }
        // 방에 이름 붙여주기
        SetRoomNumber();

        ////////////////////////////////////////////////////

        //////////////////////문 검사///////////////////////
        ExaminateDoor();
        ////////////////////////////////////////////////////

        //////////////////////문 생성///////////////////////
        for (int i = 0; i < room_list.Count; i++)
        {
            MakeDoor(room_list[i]);
        }
        /////////////////////////////////////////////////////
        
        ////////////////////장애물 생성//////////////////////
        for(int i = 1; i < room_list.Count-1; i++)
        {
            CreateInterrupt(room_list[i].location);
        }
        /////////////////////////////////////////////////////

        ////////////////////적 생성//////////////////////////

        MakeAllEnemy();
        MakeBoss();
        /////////////////////////////////////////////////////

        Debug.Log(many_of_map);
        
        //Debug.Log(room_list[room_list.Count - 1].room_number);
    }

    // 방번호 지정
    public void SetRoomNumber()
    {
        for(int i = 0; i < room_list.Count; i++)
        {
            room_list[i].charge.transform.name = i.ToString();
        }
    }

    // 초기화
    public void InitElement()
    {
        room_pos.position = new Vector3(0, 0, -3);
        room_queue = new Queue<Room>();
        room_list = new List<Room>();
        many_of_map = 0;

        // 맨 처음 방 생성
        MakeRoom(room_pos);

        room_list[0].clear = true;
    }

    // 룸 삭제
    public void DeleteRoom(Room del)
    {
        Destroy(del.charge);
        del = null;
    }

    // 룸생성
    public bool MakeRoom(Transform location)
    {
        // 그 자리에 이미 있으면 안 만듬
        if (!AlreadyRoom(location))
            return false;

        Room temp = new Room();
        temp.room_number = many_of_map;

        temp.CopyTransform(location);
        
        temp.charge = Instantiate(room_form, temp.location, location.rotation);

        room_queue.Enqueue(temp);

        room_list.Add(temp);

        temp.charge.transform.parent = room_parent;


        //many_of_map++;
        //Debug.Log(room_queue.Dequeue());
        return true;
    }

    // 전체 맵 생성 함수
    public Room MakeMap()
    {
        int door_many = Random.Range(1, 3);
        bool door_ok = true;
        Room present_room;
        

        // Queue가 비었거나 맵의 갯수가 꽉차면 Stop 
        if (many_of_map == max_of_map)
            return null;

        
        if (many_of_map > 0)
        {
            //Debug.Log("sec");
            door_ok = MakeRoom(room_pos);

            if (!door_ok)
                return null;

            present_room = room_queue.Dequeue();

            // room의 위치 저장
            present_room.CopyTransform(room_pos);
            
        }
        else
        {
            //Debug.Log("fir");
            present_room = room_queue.Dequeue();
        }

        many_of_map++;
        if (many_of_map == max_of_map)
        {
            present_room.room_type = Constants.BOSSROOM;
            Debug.Log(present_room.location);
        }

        if (door_ok)
        {
            //Debug.Log("ok");    
            SetDoor(present_room, door_many);
            NextRoom(present_room);
        }

        
        return null;
    }

    // 다음 방 생성 함수
    public void NextRoom(Room room)
    {
        Vector3 temp = new Vector3(room.location.x, room.location.y, room.location.z);

        if (room.door_top)
        {
            temp.y += 10;
            ChangeRoomPosPosition(temp);
            MakeMap();
            ChangeRoomPosPosition(room.location);
        }
        if (room.door_bottom)
        {
            temp.y -= 10;
            ChangeRoomPosPosition(temp);
            MakeMap();
            ChangeRoomPosPosition(room.location);
        }
        if (room.door_left)
        {
            temp.x -= 20;
            ChangeRoomPosPosition(temp);
            MakeMap();
            ChangeRoomPosPosition(room.location);
        }
        if (room.door_right)
        {
            temp.x += 20;
            ChangeRoomPosPosition(temp);
            MakeMap();
            ChangeRoomPosPosition(room.location);
        }
    }

    // room의 location 을 복사
    public void ChangeRoomPosPosition(Vector3 loca)
    {
        room_pos.transform.position = new Vector3(loca.x, loca.y,loca.z);
    }

    // 그 장소에 방이 있는지 판단하는 함수 매개변수 Transform ver. 없으면 true 있으면 false
    public bool AlreadyRoom(Transform loca)
    {

        for (int i = 0; i < room_list.Count; i++)
            if (room_list[i].location.x == loca.position.x && room_list[i].location.y == loca.position.y)
                return false;

        return true;
    }

    // 그 장소에 방이 있는지 판단하는 함수 매개변수 Vector3 ver. 없으면 true 있으면 false
    public bool AlreadyRoom(Vector3 loca)
    {

        for (int i = 0; i < room_list.Count; i++)
            if (room_list[i].location.x == loca.x && room_list[i].location.y == loca.y)
                return false;

        return true;
    }

    // 문 설정
    public void SetDoor(Room room, int door_many)
    {
        int pre_dir = -1;
        int door_direction;

        for (int i = 0; i < door_many; i++)
        { 
            do
            {
                door_direction = Random.Range(0, 4);
            } while (pre_dir == door_direction);

            switch (door_direction)
            {
                case 0:
                    room.door_top = true;
                    break;
                case 1:
                    room.door_right = true;
                    break;
                case 2:
                    room.door_bottom = true;
                    break;
                case 3:
                    room.door_left = true;
                    break;
            }

            pre_dir = door_direction;
        }

    }

    // 문을생성
    void MakeDoor(Room room)
    {
       
        if(room.door_top)
        {
            Vector3 temp = new Vector3(room.location.x, room.location.y + 4, -3);
            GameObject door = Instantiate(top_door, temp, room_pos.transform.rotation);
            door.transform.name = "door_top";
            door.transform.parent = room.charge.transform;
        }
        if(room.door_bottom)
        {
            Vector3 temp = new Vector3(room.location.x, room.location.y - 4, -3);
            GameObject door = Instantiate(bottom_door, temp, room_pos.transform.rotation);
            door.transform.name = "door_bottom";
            door.transform.parent = room.charge.transform;
        }
        if (room.door_left)
        {
            Vector3 temp = new Vector3(room.location.x - 8, room.location.y, -3);
            GameObject door = Instantiate(left_door, temp, room_pos.transform.rotation);
            door.transform.name = "door_left";
            door.transform.parent = room.charge.transform;
        }
        if (room.door_right)
        {
            Vector3 temp = new Vector3(room.location.x + 8, room.location.y, -3);
            GameObject door = Instantiate(right_door, temp, room_pos.transform.rotation);
            door.transform.name = "door_right";
            door.transform.parent = room.charge.transform;
        }
        
    }


    // 문 검사
    public void ExaminateDoor()
    {
        for(int i = 0; i < room_list.Count; i++)
        {
            Room exam = room_list[i];
            if(exam.door_top)
            {
                Vector3 temp = new Vector3(exam.location.x, exam.location.y + 10, exam.location.z);
                if (AlreadyRoom(temp))
                {
                    //Debug.Log("없네요");
                    exam.door_top = false;
                }
            }
            if (exam.door_bottom)
            {
                Vector3 temp = new Vector3(exam.location.x, exam.location.y - 10, exam.location.z);
                if (AlreadyRoom(temp))
                {
                    //Debug.Log("없네요");
                    exam.door_bottom = false;
                }
                    
            }
            if (exam.door_right)
            {
                Vector3 temp = new Vector3(exam.location.x + 20, exam.location.y , exam.location.z);
                if (AlreadyRoom(temp))
                {
                    //Debug.Log("없네요");
                    exam.door_right = false;
                }
                    
            }
            if (exam.door_left)
            {
                Vector3 temp = new Vector3(exam.location.x - 20, exam.location.y, exam.location.z);
                if (AlreadyRoom(temp))
                {
                    //Debug.Log("없네요");
                    exam.door_left = false;
                }
            }
        }

        for (int i = 0; i < room_list.Count; i++)
        {
            Room exam = room_list[i];
            Vector3 temp;

            temp = new Vector3(exam.location.x, exam.location.y + 10, exam.location.z);
            if (!AlreadyRoom(temp))
                exam.door_top = true;

            temp = new Vector3(exam.location.x, exam.location.y - 10, exam.location.z);
            if (!AlreadyRoom(temp))
                exam.door_bottom = true;

            temp = new Vector3(exam.location.x + 20, exam.location.y, exam.location.z);
            if (!AlreadyRoom(temp))
                exam.door_right = true;

            temp = new Vector3(exam.location.x - 20, exam.location.y, exam.location.z);
            if (!AlreadyRoom(temp))
                exam.door_left = true;
        }

    }

    // 방해물을 특정 위치에 생성하며 inter_list의 child 로 만듬
    public void CreateInterrupt(Vector3 loca)
    {
        //    Debug.Log("호출");
        
        // 종류
        int kind = Random.Range(0, 3);
        GameObject intemp;

        switch (kind)
        {
            case 0:
                Vector3 temp = new Vector3(loca.x + 0.35f, loca.y, loca.z);
                intemp = Instantiate(interrupt_0, temp, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
               
                break;
            case 1:
                intemp = Instantiate(interrupt_1, loca, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
                break;
            case 2:
                intemp = Instantiate(interrupt_2, loca, interrupts.rotation);
                intemp.transform.parent = interrupts;
                inter_list.Add(intemp);
                break;
            default:
                break;
        }
        
        return;
    }

   
    public void MakeEnemy(Room other)
    {
        GameObject room = other.charge;
        int max = Random.RandomRange(1, max_enemy);

        for(int i = 0; i < max; i++)
        {
            float x = Random.Range(room.transform.position.x - 5, room.transform.position.x + 5);
            float y = Random.Range(room.transform.position.y - 3, room.transform.position.y + 3);

            Vector3 temp = new Vector3(x, y, -3);

            GameObject tenmey = Instantiate(enemy1, temp, room.transform.rotation);
            tenmey.transform.name = "Fkiller" + i;
            tenmey.transform.parent = room.transform;
            tenmey.GetComponent<fkillerPattern>().roomNum = System.Convert.ToInt32(room.transform.name);
            other.enemy_list.Add(tenmey);
        }
    }

    public void MakeAllEnemy()
    {
        for(int i = 1; i < room_list.Count-1;i++)
        {
            MakeEnemy(room_list[i]);
        }
    }

    public void MakeBoss()
    {
        Transform temp = room_list[max_of_map - 1].charge.transform;
        Vector3 loca = new Vector3(temp.position.x, temp.position.y + 4f, -3);
        bossInfo = Instantiate(Boss, loca, temp.rotation);
        bossInfo.GetComponent<RamjiPattern3>().playerTrans = GetComponent<InGameManager>().player.transform;

    }
}

// class room

public class Room
{

    public Vector3 location;
    public List<GameObject> enemy_list;

    public GameObject charge;
    // 문
    public bool door_top;
    public bool door_bottom;
    public bool door_left;
    public bool door_right;

    // check 여부
    public bool clear;

    // 룸 타입
    public int room_type;

    // 방 번호
    public int room_number;

    // Use this for initialization
    public Room()
    {
        door_top = false;
        door_bottom = false;
        door_left = false;
        door_right = false;

        clear = false;

        room_number = 0;
        room_type = 0;

        enemy_list = new List<GameObject>();
    }

    public void CopyTransform(Transform location)
    {
        this.location = new Vector3(location.transform.position.x, location.transform.position.y,-3);
    }
}


