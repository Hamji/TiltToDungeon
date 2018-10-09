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
    public Map[,] map;
    public int max_of_map;
    public int many_of_map;

    public Tilemap botmap;
    public Tilemap objmap;
    // 바닥
    public Tile floor;

    // 문
    public Tile top_door;
    public Tile left_door;
    public Tile right_door;
    public Tile bottom_door;

    public InGameManager manager;


    Vector3Int location;
    Vector3Int door_position;


    // 방해물을 만들기 위하여 필요한 부분 
    // 방해물 List
    public Transform interrupts;
    public List<GameObject> inter_list;

    // 방해물
    public GameObject interrupt_0;
    public GameObject interrupt_1;
    public GameObject interrupt_2;
  

    // Use this for initialization
    void Start()
    {
        Interrupting(0,0);
    //    InitInterrupt();
        
        // 맵 List 초기화 하며 생성
        map = new Map[Constants.MAX, Constants.MAX];
        for (int i = 0; i < Constants.MAX; i++)
            for (int j = 0; j < Constants.MAX; j++)
                map[i, j] = new Map();

        many_of_map = 0;


        while (many_of_map != max_of_map)
        {
            many_of_map = 0;

            botmap.ClearAllTiles();
            objmap.ClearAllTiles();

            MakeMap();
        }

        SetManagerMap();
    }
    
        public void SetManagerMap()
    {
        for (int i = 0; i < Constants.MAX; i++)
            for (int j = 0; j < Constants.MAX; j++)
                manager.maps[i, j] = map[i, j];
    }

    // 필드 그리기 함수
    public void DrawField(int r, int c)
    {
        int current_x = 0;
        int current_y = 0;

        int current_r = r;
        int current_c = c;

        current_x = (current_c - 12) * 20;
        current_y = -(current_r - 12) * 10;

        // 내 현재 위치 그림
        DrawMap(current_x, current_y, 0);

        DrawDoor(current_x, current_y, -2);

        
    }
 
    // 맵 그리기 함수
    public void DrawMap(int x, int y, int z)
    {
        location.Set(x, y, z);

        // 바닥이랑 벽 그림
        botmap.BoxFill(location, floor, x, y, x,y);
    }

    // 문 그리기 함수
    public void DrawDoor(int x, int y, int z)
    {

        int current_r, current_c;
        current_c = (x / 20) + 12;
        current_r = -(y / 10) + 12;

       
        if (map[current_r, current_c].door_top == true)
        {
            location.Set(x, y + 4, z);
            objmap.BoxFill(location, top_door, x, y + 4, x, y + 4);
//            objmap.RefreshTile(location);
        }
        if(map[current_r, current_c].door_bottom == true)
        {
            location.Set(x, y - 4, z);
            objmap.BoxFill(location, bottom_door, x, y - 4, x, y - 4);
//            objmap.RefreshTile(location);
        }
        if(map[current_r, current_c].door_right == true)
        {
            location.Set(x + 8, y, z);
            objmap.BoxFill(location, right_door, x + 8, y, x + 8, y);
//            objmap.RefreshTile(location);
        }
        if(map[current_r, current_c].door_left == true)
        {
            location.Set(x - 8, y, z);
            objmap.BoxFill(location, left_door, x - 8, y, x - 8, y);
//            objmap.RefreshTile(location);
        }
    }
        
    // Update is called once per frame
    void Update()
    {

    }

    // 맵 구조 생성 함수
    public void MakeMap()
    {
        InitMap();
        SetMap(Constants.CENTER, Constants.CENTER);
    }

    // 맵초기화 함수
    public void InitMap()
    {
        if (map != null)
            for (int i = 0; i < Constants.MAX; i++)
                for (int j = 0; j < Constants.MAX; j++)
                {
                    map[i, j].check = false;
                    map[i, j].door_bottom = false;
                    map[i, j].door_top = false;
                    map[i, j].door_right = false;
                    map[i, j].door_left = false;    
                }

        InitInterrupt();
    }

    // 맵 구조 설정 함수
    public  void SetMap(int r, int c)
    {
        int current_x = (c - 12) * 20;
        int current_y = -(r - 12) * 10;


        if (max_of_map <= many_of_map)
            return;

        if (map[r, c].check != false)
            return;
        


        map[r, c].check = true;
        many_of_map++;

        

        int try_number = 0;

        if( many_of_map == 1)
            try_number = Random.Range(1, 4);
        else
            try_number = Random.Range(0, 4);


        // 방향 설정
        for(int i = 0; i < try_number; i++)
        {
            int direction = Random.Range(0, 4);
            SetDirection(r, c, direction);
        }

        Interrupting(current_x, current_y);
//        DrawField(r, c);
        
        return;
    }

    //  방향 설정 함수1은 북  시계
    public void SetDirection(int r, int c, int direction)
    {
        switch (direction)
        {
            case 0:
                map[r, c].door_top = true;
                map[r - 1, c].door_bottom = true;
                SetMap(r - 1, c);
                break;
            case 1:
                map[r, c].door_right = true;
                map[r, c + 1].door_left = true;
                SetMap(r, c + 1);
                break;
            case 2:
                map[r, c].door_bottom = true;
                map[r + 1, c].door_top = true;
                SetMap(r + 1, c);
                break;
            case 3:
                map[r, c].door_left = true;
                map[r, c - 1].door_right = true;
                SetMap(r, c - 1);
                break;
            default:
                break;
        }

        TestMapDirection(r, c);
        DrawField(r, c);
    }

    // 맵 검사 
    public void TestMapDirection(int r, int c)
    {
        if (map[r, c].door_top == true)
        {
            if (map[r - 1, c].check == false)
            {
                map[r, c].door_top = false;
                return;
            }   
            map[r - 1, c].door_bottom = true;
        }
        else if (map[r - 1, c].door_bottom == true)
            map[r, c].door_top = true;

        if (map[r, c].door_bottom == true)
        {
            map[r + 1, c].door_top = true;
        }
        else if (map[r + 1, c].door_top == true)
            map[r, c].door_bottom = true;

        if (map[r, c].door_right == true)
        {
            map[r, c + 1].door_left = true;
        }
        else if (map[r, c + 1].door_left == true)
            map[r, c].door_right = true;

        if (map[r, c].door_left == true)
        {
            map[r, c - 1].door_right = true;
        }
        else if (map[r, c - 1].door_right == true)
            map[r, c].door_left = true;


 //       DrawField(r, c);
    }

    // 방해물을 특정 위치에 생성하며 inter_list의 child 로 만듬
    IEnumerator CreateInterrupt(int x, int y)
    {
    //    Debug.Log("호출");
        Vector3 temp = new Vector3(x+0.5f, y+0.5f, -3);
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
       
//        GameObject intemp = Instantiate(interrupt, temp, inter_list.rotation);
//        intemp.transform.parent = inter_list;
        yield return null;
    }

    // CreateInterrupt 호출
    public void Interrupting(int x, int y)
    {
        StartCoroutine(this.CreateInterrupt(x,y));
    }
    
    public void InitInterrupt()
    {
        int count = interrupts.transform.childCount;
        for(int i = inter_list.Count-1;i>-1;i--)
        {
            Destroy(inter_list[i]);
        }
    }
}

public class Map
{
    // 문
    public bool door_top;
    public bool door_bottom;
    public bool door_left;
    public bool door_right;
    

    // check 여부
    public bool check;

    public Map()
    {
        door_top = false;
        door_bottom = false;
        door_left = false;
        door_right = false;

        check = false;
    }
}