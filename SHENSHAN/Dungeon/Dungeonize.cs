using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using Unity.Mathematics;
using UnityEngine.AI;


using Random = Unity.Mathematics.Random;


//房间尺寸,当两个房间连接时,过道(corner)包含补给物品
public class Room
{
    public int x;
    public int y;
    //
    public int w;
    public int h;
    //
    public Room connectTo = null;
    public int branch = 0;
    //
    public string relative_pos = "x";
    
    //
    public bool dead_end = false;
    //
    public int room_id = 0;

}


//生成位置
public class SpawnList
{
    public int x;
    public int y;
    public bool byWall;
    public string wallLoc;
    public bool inTheMid;
    public bool byCollidor;
    
    //
    public int asDoor = 0;
    public Room room = null;
    public bool spawnObject;
    
}

//内容块，生成主要以下内容
//1.商人图块
//2.对战模块
//3.事件推进模块
//4.地形材质结构
//p-p tile->door--corner--door<-tile
[System.Serializable]
public class CustomRoom
{
    public int roomid = 1;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject cornerPrefab;
    public GameObject doorPrefab;
}

//生成参数，根据配置文件生成地形大小和类型
[System.Serializable]
public class SpawnOption
{
    public int minCount;
    public int maxCount;
    public bool spawnByWall;
    public bool spawnIntheMid;
    public bool spawnRotated;
    //
    public float heightFix = 0;
    public GameObject obj;
    public int spawnRoom = 0;

}


//
public class MapTile
{
    public int type = 0;
    public int oridentation = 0;
    public Room room = null;
}


/// <summary>
/// 地形生成，根据关卡设置进行随机生成，需要配置文件设定参数(怪物参数,TileBG,任务参数)
/// 玩家初始位置根据初始房间生成
/// NPC prefab 根据地点产生，
/// </summary>
///

public class Dungeonize : MonoBehaviour
{
    public GameObject startPrefab;
    public GameObject endPrefab;
    
   [Header("Content Here To Config")]
    public List<SpawnList> spawnLoc= new List<SpawnList>();
    public List<CustomRoom>roomList=new List<CustomRoom>();
    public List<SpawnOption> spawnOptions=new List<SpawnOption>();
  

    [Header("Common")]
    public GameObject floorprefab;
    public GameObject doorPrefab;
    public GameObject wallPrefab;
    public GameObject cornerPrefab;

    
    public int roomMargin = 3;
    public bool is3D = false;    //default 2D for miniMap,3d in the world
    public bool generate_onLoad = true;    //runtime

    public int minRoomSize = 5;

    public int maxRoomSize = 10;
    public float tileScaling = 1f;
    
    
    //Common rnd
    public static Random rnd= new Random();
    //Dungeo Config detail
    class Dungeon
    {
        //dungeon size
        public static int map_size;
        public static int map_size_x;
        public static int map_size_y;
        public int minSize=3;
        public int maxSize=5;
        
        //
        public static MapTile[,] MapTile;
        
        public static List<Room> rooms =new List<Room>();

        public static Room goalRoom;
        public static Room spawnRoom; //initPlayer

        public int maxminiumRoomCount;
        public int roomMargin;    //房间邻接点
        public int roomMarginTmp;
        
        //tile type for ease
        public static List<int> roomsandfloors = new List<int> {1, 3};
        public static List<int> corners = new List<int> { 4,5,6,7};
        public static List<int> walls = new List<int> {8, 9, 10, 11};
        //save Loc
        public static List<string> directionLoc = new List<string>() {"x", "-x", "y", "-y"};

        /// <summary>
        /// 生成随机地下城，根据以下原则
        /// 1.方位确认，房间相邻不超过4个(limit of cube)，
        /// 2.房间生成无重叠邻间生成过道
        /// 3.碰撞体
        /// 4.墙体生成
        /// 5。过道生成,在2个房间之间,楼梯(x,y+10,z)产生位置向y轴产生
        /// maptile:0default,1room floor,2wall,corridor floor 3,room corner 4,5,6,7
        /// </summary>
        public void Generate()
        {
            int room_count = this.maxminiumRoomCount;
            int min_size = this.minSize;
            int max_size = this.maxSize;
            //房间邻接点
            if (roomMargin < 2)
            {
                map_size = room_count * max_size * 2;
            }
            else
            {
                map_size = (room_count * (max_size + (roomMargin * 2))) + (room_count * room_count * 2);
            }
            //
            MapTile=new MapTile[map_size,map_size];
            //generate tile in world[x,y]
            for(var x=0;x<map_size;x++)
            {
                for (var y = 0; y < map_size; y++)
                {
                    MapTile=new MapTile[x,y];
                    MapTile[x, y].type = 0; //type equals 
                }
            }
            rooms = new List<Room>();
            
            //room direction
            int collision_count = 0;
            string direction = "set";
            string oldDirection = "set";
            Room lastRoom;
            
            //Set Room Direction
            for (var rc = 0; rc < room_count; rc++)
            {
                Room room=new Room();
                Random r=new Random();
                if (rooms.Count == 0)
                {
                    Random rs = new Random();
                    //generate first room then 
                    //set first dire
                    room.x = (int) math.floor(map_size / 2);
                    room.y = (int) math.floor(map_size / 2);
                    //Rnd size for [w,h]
                    room.w =UnityEngine.Random.Range(min_size, max_size);
                    if (room.w % 2 == 0) room.w += 1;
                    room.h = UnityEngine.Random.Range(min_size, max_size);
                    if (room.h % 2 == 0) room.h += 1;
                    room.branch = 0;
                    lastRoom = room;
                }
                else
                {
                    //
                    int branch = 0;
                    if (collision_count == 0)
                    {
                        branch =UnityEngine.Random.Range(5, 20);

                    }

                    room.branch = branch;

                    //get all room in list
                    lastRoom = rooms[rooms.Count - 1];

                    int lri = 1;
                    if (lastRoom.dead_end)
                    {
                        lastRoom = rooms[rooms.Count - lri++];
                    }

                    #region ROOMDIRECTION

                    //find new dire set then add
                    if (direction == "set")
                    {
                        //rnd directions
                        string newRndDirection = directionLoc[UnityEngine.Random.Range(0, directionLoc.Count)];
                        direction = newRndDirection;
                        while (direction == oldDirection)
                        {
                            newRndDirection = directionLoc[UnityEngine.Random.Range(0, directionLoc.Count)];
                            direction = newRndDirection;
                        }
                    }

                    //tmp add margin(tmp ist all margin)
                    this.roomMarginTmp = r.NextInt(0, this.roomMargin - 1);
                    //according to x,y,-x,-y to set directions
                    if (direction == "y")
                    {
                        room.x = lastRoom.x + lastRoom.w + r.NextInt(3, 5) + this.roomMarginTmp;
                        room.y = lastRoom.y;
                    }
                    else if (direction == "-y")
                    {
                        room.x = lastRoom.x - lastRoom.w + r.NextInt(3, 5) - this.roomMarginTmp;
                        room.y = lastRoom.y;
                    }
                    else if (direction == "x")
                    {
                        room.y = lastRoom.y + lastRoom.h + r.NextInt(3, 5) + this.roomMarginTmp;
                        room.x = lastRoom.x;
                    }
                    else if (direction == "-x")
                    {
                        room.y = lastRoom.y - lastRoom.h + r.NextInt(3, 5) + this.roomMarginTmp;
                        room.x = lastRoom.x;
                    }

                    //direction by tile[w,h]
                    room.w = r.NextInt(min_size, max_size);
                    if (room.w % 2 == 0) room.w += 1;
                    room.h = r.NextInt(min_size, max_size);
                    if (room.h % 2 == 0) room.h += 1;
                    room.connectTo = lastRoom;

                    //check corridor
                    bool isCorridor = this.DoesCorride(room, 0);
                    if (isCorridor)
                    {
                        rc--;
                        collision_count += 1;
                        if (collision_count > 3)
                        {
                            lastRoom.branch = 1;
                            lastRoom.dead_end = true;
                            collision_count = 0;
                        }
                        else
                        {
                            oldDirection = direction;
                            direction = "set";
                        }
                    }
                    else
                    {
                        room.room_id = rc;
                        rooms.Add(room);
                        oldDirection = direction;
                        direction = "set";
                    }
                }

                #endregion

                 

                    for (int k = 0; k < rooms.Count; k++)
                    {
                        Room mr = rooms[k];
                        for (int m=room.x; m < room.x + room.w; m++)
                        {
                            for (int n = 0; n < room.y + room.h; n++)
                            {
                                MapTile[m, n].type = 1;
                                MapTile[m, n].room = room;
                            }
                        }
                    }
                    
                    //Collidor corridor between a und b
                    // A----->corner<-----B
                    for (int crc = 0; crc < rooms.Count; crc++)
                    {
                        //
                        Room rA = rooms[crc];
                        Room rB = rooms[crc].connectTo;
                        
                        //
                        if (rB != null)
                        {
                            var pa = new Room();
                            var pb = new Room();
                            pa.x = rA.x + (int) Mathf.Floor(rA.w / 2);
                            pb.x = rB.x + (int) Mathf.Floor(rB.w / 2);
                            //
                            pa.y = rA.y + (int)Mathf.Floor(rA.h / 2);
                            pb.y = rB.y + (int) Mathf.Floor(rB.h / 2);
                            
                            //
                            if (math.abs(pa.x - pb.x) > math.abs(pa.y - pb.y))
                            {
                                //y axis
                                if (rA.h > rB.h)
                                {
                                    pa.y = pb.y;
                                }
                                else
                                {
                                    pb.y = pa.y;
                                }
                            }
                            else
                            {
                                //
                                if (rA.w < rB.w)
                                {
                                    pa.x = pb.x;
                                }
                                else
                                {
                                    pb.x = pa.x;
                                }
                                
                            }
                            
                            //碰撞体是否相等
                            while ((pb.x != pa.x) || (pb.y != pa.y))
                            {
                                if (pb.x != pa.x)
                                {
                                    if (pb.x > pa.x)
                                    {
                                        pb.x--;
                                    }
                                    else
                                    {
                                        pb.x++;
                                    }
                                }
                                else if(pb.y!= pa.y)
                                {
                                    if (pb.y > pa.y)
                                    {
                                        if (pb.x > pa.x)
                                        {
                                            pb.y--;
                                        }
                                        else
                                        {
                                            pb.y++;
                                        }
                                    }
                                    
                                    //
                                    if (MapTile[pb.x, pb.y].room == null)
                                    {
                                        MapTile[pb.x, pb.y].type = 3;
                                    }
                                    
                                }
                            }

                        }
                        

                    }
                    
                    //Push tile to bottom of edge from mid
                    //x crops
                    int row = 1;
                    int min_crop_x = map_size;
                    for (int mx = 1; mx < map_size_x - 1; mx++)
                    {
                        bool x_empty = true;
                        for (int my = 1; my < map_size_y - 1; my++)
                        {
                            if (MapTile[mx, my].type != 0)
                            {
                                x_empty = false;
                                if (mx < min_crop_x)
                                {
                                    min_crop_x = mx;
                                }

                                break;
                            }
                        }
                        //
                        if (!x_empty)
                        {
                            for (int mys = 0; mys < map_size - 1; mys++)
                            {
                                //Generate by maptile[x,nys]
                                MapTile[row, mys] = MapTile[mx, mys];
                                MapTile[mx,mys]=new MapTile();
                            }
                            //
                            row += 1;
                        }
                    }
                    
                    //y crops y轴
                     row = 1;
                    int min_crop_y = map_size;
                    for (int y = 0; y < map_size_x - 1; y++)
                    {
                        bool y_empty = false;
                        for (int mxs=0;mxs <map_size_y-1;mxs++)
                        {
                            if (MapTile[mxs, y].type != 0)
                            {
                                y_empty = false;
                                if (y < min_crop_y)
                                {
                                    min_crop_y = y;
                                }

                                break;
                            }    
                        }
                        //generate new y tile to bottom of edges
                        if (!y_empty)    
                        {
                            for (int xs = 0; xs < map_size_x - 1; xs++)
                            {
                                MapTile[xs, row] = MapTile[xs, y];
                                MapTile[xs,row]=new MapTile();
                            }

                            row += 1;

                        }
                    }
                    //
                    foreach (Room rs in rooms)
                    {
                        rs.x -= min_crop_x;
                        rs.y -= min_crop_y;
                    }
                    
                    //testing ms
                    int final_map_size_x = 0;
                    int final_map_size_y = 0;
                    for (int y = 0; y < map_size_x - 1; y++)
                    {
                        for (int x = 0; x < map_size_y - 1; x++)
                        {
                            if (MapTile[x, y].type != 0)
                            {
                                final_map_size_y += 1;
                                break;
                            }
                        }
                    }
                    //
                    for (int x = 0; x < map_size_x - 1; x++)
                    {
                        for (int y = 0; y < map_size_y - 1; y++)
                        {
                            if (MapTile[x, y].type != 0)
                            {
                                final_map_size_x += 1;
                                break;
                            }
                        }
                    }
                    //
                    final_map_size_x += 5;
                    final_map_size_y += 5;
                    
                    //
                    MapTile[,]new_map = new MapTile[final_map_size_x+1,final_map_size_y+1];
                    for (int x = 0; x < final_map_size_x; x++)
                    {
                        for (int y = 0; y < final_map_size_y; y++)
                        {
                            new_map[x, y] = MapTile[x, y];
                        }
                    }

                    MapTile = new_map;
                    map_size_x = final_map_size_x;
                    map_size_y = final_map_size_y;
                    
                    
                    //WALL
                    for (int x = 0; x < map_size_x-1; x++)
                    {
                        for (int y = 0; y < map_size_y-1; y++)
                        {
                            if (MapTile[x, y].type == 0)
                            {
                                if (MapTile[x + 1, y].type == 1 || MapTile[x + 1, y].type == 3)            //west
                                {
                                    MapTile[x, y].type = 11;
                                    MapTile[x, y].room = MapTile[x + 1, y].room;
                                }

                                if (x > 0)
                                {
                                    if (MapTile[x - 1, y].type == 1 || MapTile[x + 1, y].type == 3)        //east
                                    {
                                        MapTile[x, y].type = 9;
                                        MapTile[x, y].room = MapTile[x-1, y].room;
                                    }
                                }

                                if (MapTile[x, y + 1].type == 1 || MapTile[x, y + 1].type == 1)            //north
                                {
                                    MapTile[x, y + 1].type = 10;
                                    MapTile[x, y].room = MapTile[x, y + 1].room;
                                }

                                if (y > 0)
                                {
                                    if (MapTile[x, y + 1].type == 1 || MapTile[x, y + 1].type == 3)
                                    {
                                        MapTile[x, y].type = 8;
                                        MapTile[x, y].room = MapTile[x, y - 1].room;
                                    }
                                }
                            }
                        }
                    }
                    
                    //corner
                    for (int x = 0; x < map_size_x - 1; x++)
                    {
                        for (int y = 0; y < map_size_y - 1; y++)
                        {
                            if (walls.Contains(MapTile[x, y + 1].type) && walls.Contains(MapTile[x + 1, y].type) &&
                                roomsandfloors.Contains(MapTile[x + 1, y + 1].type))
                            {
                                //north
                                MapTile[x, y].type = 1;
                                MapTile[x, y].room = MapTile[x + 1, y + 1].room;
                            }

                            if (y > 0)
                            {
                                //north
                                if (walls.Contains(MapTile[x + 1, y].type) && walls.Contains(MapTile[x, y - 1].type) &&
                                    roomsandfloors.Contains(MapTile[x + 1, y - 1].type))
                                {
                                    MapTile[x, y].type = 5;
                                    MapTile[x, y].room = MapTile[x + 1, y - 1].room;
                                }
                            }

                            if (x > 0)
                            {
                                if (walls.Contains(MapTile[x - 1, y].type) && walls.Contains(MapTile[x, y + 1].type) &&
                                    roomsandfloors.Contains(MapTile[x - 1, y + 1].type))
                                {
                                    MapTile[x, y].type = 7;
                                    MapTile[x, y].room = MapTile[x - 1, y + 1].room;
                                }
                            }

                            if (x > 0 && y > 0)
                            {
                                if (walls.Contains(MapTile[x - 1, y].type) && walls.Contains(MapTile[x, y - 1].type) &&
                                    roomsandfloors.Contains(MapTile[x - 1, y - 1].type))
                                {
                                    MapTile[x, y].type = 6;
                                    MapTile[x, y].room = MapTile[x - 1, y - 1].room;
                                }
                            }
                            //door corner
                            if (MapTile[x, y].type == 3)
                            {
                                if (MapTile[x+1, y].type == 1)
                                {
                                    MapTile[x,y+1].type = 11;
                                    MapTile[x, y-1].type = 11;
                                }else if (Dungeon.MapTile[x - 1, y].type == 1)
                                {
                                    MapTile[x, y + 1].type = 9;
                                    MapTile[x, y - 1].type = 9;
                                }
                            }
                        }
                    }
                    //
                    for (int x = 0; x < map_size_x - 1; x++)
                    {
                        for (int y = 0; y < map_size_y - 1; y++)
                        {
                            if (MapTile[x, y].type == 3)
                            {
                                bool cw = MapTile[x, y + 1].type == 3;
                                bool ce = MapTile[x, y - 1].type == 3;
                                bool cn = MapTile[x + 1, y].type == 3;
                                bool cs = MapTile[x - 1, y].type == 3;
                                if (cw || ce)
                                {
                                    MapTile[x, y].oridentation = 1;
                                }
                                else if (cn || cs)
                                {
                                    MapTile[x, y].oridentation = 2;
                                }
                            }
                        }
                    }
                    
                    //goal
                    goalRoom = rooms[rooms.Count - 1];
                    if (goalRoom != null)
                    {
                        goalRoom.x = goalRoom.x + (goalRoom.w / 2);
                        goalRoom.y = goalRoom.y + (goalRoom.h / 2);
                        
                    }
                    //first room
                    spawnRoom = rooms[0];
                    spawnRoom.x = spawnRoom.x + (spawnRoom.w / 2);
                    spawnRoom.y = spawnRoom.y + (spawnRoom.h / 2);
                    

            }
                
        }
             
        
        public bool DoesCorride(Room room, int count)
        {
            int rnd_blankliness = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                var check = rooms[i];
                if(!((room.x + room.w + rnd_blankliness <check.x) || 
                     (room.x >check.x + check.w +rnd_blankliness ) ||
                     (room.y+room.h + rnd_blankliness <check.y) ||
                    (room.y>check.y+check.h+rnd_blankliness)))
                {
                    return true;
                }
            }
            return false;
        }

        public float LineDistance(Room pa, Room pb)
        {
            var xs = 0;
            var ys = 0;

            xs = pb.x - pa.x;
            ys = xs * xs;
            //
            ys = pb.y - pa.y;
            ys = ys * ys;

            return math.sqrt(xs + ys);
        }



            
        }


    public void ClearOldDungeon(bool immediate = false)
    {
        int c = transform.childCount;
        for (var i = c - 1; i >= 0; i--)
        {
            if (immediate)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            else
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
    
    


    void Start()
    {
        
        //
        Utils.InvokeMany(typeof(Dungeonize),this,"OnStart_");
    }


//    public override void OnStartClient()
//    {
//        base.OnStartClient();
//        
//        Utils.InvokeMany(typeof(Dungeonize),this,"OnStartClient_");
//    }
//
//    public override void OnStartServer()
//    {
//        base.OnStartServer();
//        
//        Utils.InvokeMany(typeof(Dungeonize),this,"OnStartServer_");
//    }

    /// <summary>
    /// Generate Dungeon at world
    /// </summary>
    public void Generate()
    {
        Dungeon dungeon =new Dungeon();
        dungeon.minSize = minRoomSize;
        dungeon.maxSize = maxRoomSize;
        dungeon.maxminiumRoomCount = maxRoomSize;
        dungeon.roomMargin = roomMargin;
        
        //Core
        dungeon.Generate();
        
        //according dungeon to gnerate details
        for (var y = 0; y < Dungeon.map_size_y; y++)
        {
            for (var x = 0; x < Dungeon.map_size_x; x++)
            {
                int tile = Dungeon.MapTile[x, y].type;
                int orientation = Dungeon.MapTile[x, y].oridentation;
                GameObject created_tile;
                Vector3 tile_loc;
                if (!is3D)
                {
                    //2D
                    tile_loc =new Vector3(x*tileScaling,y*tileScaling,0);
                    
                }
                else
                {
                    tile_loc=new Vector3(x*tileScaling,0,y*tileScaling);
                }
                //
                created_tile = null;
                if (tile == 1)
                {
                    //generate floor
                    GameObject floorPrefabToUse = floorprefab;
                    Room room = Dungeon.MapTile[x, y].room;
                    if (room != null)
                    {
                        foreach (CustomRoom cr in roomList)
                        {
                            if (cr.roomid == room.room_id)
                            {
                                floorPrefabToUse = cr.floorPrefab;
                                break;
                            }
                        }
                    }
                    //
                    created_tile = Instantiate(floorPrefabToUse,tile_loc,Quaternion.identity)as GameObject;
                    
                    
                }
                //generate wall
                if (Dungeon.walls.Contains(tile))
                {
                    GameObject wallPrefabToUse = wallPrefab;
                    Room room = Dungeon.MapTile[x, y].room;
                    if (room != null)
                    {
                        foreach (CustomRoom cr in roomList)
                        {
                            if (cr.roomid == room.room_id)
                            {
                                wallPrefabToUse = cr.wallPrefab;
                                break;
                            }
                        }
                    }
                    //Generat Wall
                    created_tile = Instantiate(wallPrefabToUse,tile_loc,Quaternion.identity)as GameObject;
                    //
                    if (!is3D)
                    {
                        created_tile.transform.Rotate(Vector3.forward*(-90*(tile-4)));
                    }
                    else
                    {
                        created_tile.transform.Rotate(Vector3.up*(-90 *(tile-4)));
                    }
                    
                }
                
                //corner
                if (Dungeon.corners.Contains(tile))
                {
                    GameObject cornerPrefabToUse = cornerPrefab;
                    Room room = Dungeon.MapTile[x, y].room;
                    if (room != null)
                    {
                        foreach (CustomRoom cr in roomList)
                        {
                            if (cr.roomid == room.room_id)
                            {
                                cornerPrefabToUse = cr.cornerPrefab;
                                break;
                            }
                        }
                    }

                    if (cornerPrefabToUse)
                    {
                        //Generat Wall
                        created_tile = Instantiate(cornerPrefabToUse, tile_loc, Quaternion.identity) as GameObject;
                        //
                        if (!is3D)
                        {
                            created_tile.transform.Rotate(Vector3.forward * (-90 * (tile - 4)));
                        }
                        else
                        {
                            created_tile.transform.Rotate(Vector3.up * (-90 * (tile - 4)));
                        }

                    }
                    else
                    {
                        created_tile=Instantiate(cornerPrefabToUse,tile_loc,Quaternion.identity)as GameObject;
                        
                    }

                }

                if (created_tile)
                {
                    created_tile.transform.parent = transform;
                }
                

            }
        }
        
        
        //point
        GameObject startPoint;
        GameObject endPoint;
        if (!is3D)
        {
            endPoint=Instantiate(endPrefab,new Vector3(Dungeon.goalRoom.x *tileScaling,Dungeon.goalRoom.y*tileScaling,0),Quaternion.identity)as GameObject;
            startPoint=Instantiate(startPrefab,new Vector3(Dungeon.spawnRoom.x,Dungeon.spawnRoom.y,0),Quaternion.identity)as GameObject;
            
        }
        else
        {
            endPoint=Instantiate(endPrefab,new Vector3(Dungeon.goalRoom.x *tileScaling,0,Dungeon.goalRoom.y*tileScaling),Quaternion.identity)as GameObject;
            startPoint=Instantiate(startPrefab,new Vector3(Dungeon.spawnRoom.x,0,Dungeon.spawnRoom.y),Quaternion.identity)as GameObject;
        }
        //
        endPoint.transform.parent = transform;
        startPoint.transform.parent = transform;
        
        //Generate SpawnList
        List<SpawnList> spawnListLoc = new List<SpawnList>();

        for (int x = 0; x < Dungeon.map_size_x; x++)
        {
            for (int y = 0; y < Dungeon.map_size_y; y++)
            {
                if (Dungeon.MapTile[x, y].type == 1 &&
                    ((Dungeon.spawnRoom != Dungeon.MapTile[x, y].room &&
                      Dungeon.goalRoom != Dungeon.MapTile[x, y].room) || maxRoomSize <= 3))
                {
                    var loc = new SpawnList();
                    loc.x = x;
                    loc.y = y;
                    //Check corrdince
                    if (Dungeon.walls.Contains(Dungeon.MapTile[x + 1, y].type))
                    {
                        //S
                        loc.byWall = true;
                        loc.wallLoc = "S";
                    }else if (Dungeon.walls.Contains(Dungeon.MapTile[x - 1, y].type))
                    {
                        //
                        loc.byWall = true;
                        loc.wallLoc = "N";
                    }else if (Dungeon.walls.Contains(Dungeon.MapTile[x, y+1].type))
                    {
                        //
                        loc.byWall = true;
                        loc.wallLoc = "W";
                    }else if (Dungeon.walls.Contains(Dungeon.MapTile[x , y-1].type))
                    {
                        //
                        loc.byWall = true;
                        loc.wallLoc = "E";
                    }

                    if (Dungeon.MapTile[x + 1, y].type == 3 || Dungeon.MapTile[x - 1, y].type == 3 ||
                        Dungeon.MapTile[x, y + 1].type == 3 || Dungeon.MapTile[x, y - 1].type == 3)
                    {
                        loc.byCollidor = true;
                    }
                    if (Dungeon.MapTile[x + 1, y+1].type == 3 || Dungeon.MapTile[x - 1, y-1].type == 3 ||
                        Dungeon.MapTile[x-1, y + 1].type == 3 || Dungeon.MapTile[x+1, y - 1].type == 3)
                    {
                        loc.byCollidor = true;
                    }

                    loc.room = Dungeon.MapTile[x, y].room;
                    
                    //
                    int roomCX = (int) math.floor(loc.room.w / 2) + loc.room.x;
                    int roomCY = (int) math.floor(loc.room.h / 2) + loc.room.y;
                    
                    //
                    if (x == roomCX + 1 && y == roomCY + 1)
                    {
                        loc.inTheMid = true;
                    }

                    spawnListLoc.Add(loc);

                }else if (Dungeon.MapTile[x, y].type == 3)
                {
                    var loc =new SpawnList();
                    loc.x = x;
                    loc.y = y;
                    //
                    if (Dungeon.MapTile[x+1, y].type == 1)
                    {
                        loc.byCollidor = true;
                        loc.asDoor = 4;
                        loc.room = Dungeon.MapTile[x + 1, y].room;
                        
                        spawnListLoc.Add(loc);
                    }
                    else if (Dungeon.MapTile[x-1, y].type == 1)
                    {
                        loc.byCollidor = true;
                        loc.asDoor = 2;
                        loc.room = Dungeon.MapTile[x - 1, y].room;
                        spawnListLoc.Add(loc);
                    }
                   else  if (Dungeon.MapTile[x, y+1].type == 1)
                    {
                        loc.byCollidor = true;
                        loc.asDoor = 1;
                        loc.room = Dungeon.MapTile[x , y+1].room;
                        spawnListLoc.Add(loc);
                    }
                   else if (Dungeon.MapTile[x, y-1].type == 1)
                    {
                        loc.byCollidor = true;
                        loc.asDoor = 3;
                        loc.room = Dungeon.MapTile[x , y-1].room;
                        spawnListLoc.Add(loc);
                    }
                }
            }
        }
        //
        for (int i = 0; i < spawnListLoc.Count; i++)
        {
            SpawnList tmp = spawnListLoc[i];
            int rndIndex = rnd.NextInt(i, spawnListLoc.Count);
            spawnListLoc[i] = spawnListLoc[rndIndex];
            spawnListLoc[rndIndex] = tmp;
        }
        //
        int objectToSpwan = 0;
        

        //door
        if (doorPrefab)
        {
            for (int i = 0; i < spawnListLoc.Count; i++)
            {
                if (spawnListLoc[i].asDoor > 0)
                {
                    GameObject newObj;
                    SpawnList sploc = spawnListLoc[i];
                    //
                    GameObject doorPreabTouse = doorPrefab;
                    Room room = sploc.room;
                    
                    //
                    if (room != null)
                    {
                        foreach (CustomRoom cr in roomList)
                        {
                            if (cr.roomid == room.room_id)
                            {
                                doorPreabTouse = doorPrefab;
                                break;
                            }
                        }
                    }
                    //
                    if (!is3D)
                    {
                        newObj =Instantiate(doorPreabTouse,new Vector3(sploc.x*tileScaling,sploc.y*tileScaling,0),Quaternion.identity)as GameObject;
                        
                    }
                    else
                    {
                        newObj =Instantiate(doorPreabTouse,new Vector3(sploc.x*tileScaling,0,sploc.y*tileScaling),Quaternion.identity)as GameObject;

                    }
                    //
                    if (!is3D)
                    {
                        newObj.transform.Rotate(Vector3.forward*(-90*(spawnListLoc[i].asDoor-1)));
                    }
                    else
                    {
                        newObj.transform.Rotate(Vector3.up*(-90*(spawnListLoc[i].asDoor-1)));
                    }
                    //
                    newObj.transform.parent = transform;
                    spawnListLoc[i].spawnObject = newObj;

                }
            }
        }
        
        //others
        foreach (SpawnOption ots in spawnOptions)
        {
            objectToSpwan = rnd.NextInt(ots.minCount, ots.maxCount);
            while (objectToSpwan > 0)
            {
                bool created = false;
                //
                for (int i = 0; i < spawnListLoc.Count; i++)
                {
                  bool  createHere = false;
                  //
                  if (!spawnListLoc[i].spawnObject && !spawnListLoc[i].byCollidor)
                  {
                      if (ots.spawnRoom > maxRoomSize)
                      {
                          ots.spawnRoom = 0;
                      }
                      //
                      if (ots.spawnRoom == 0)
                      {
                          if (ots.spawnByWall)
                          {
                              if (spawnListLoc[i].byWall)
                              {
                                  created = true;
                              }
                          }else if (ots.spawnIntheMid)
                          {
                              if (spawnListLoc[i].inTheMid)
                              {
                                  created = true;
                              }
                          }
                          else
                          {
                              created = true;
                          }
                      }
                      else
                      {
                          if (spawnListLoc[i].room.room_id == ots.spawnRoom)
                          {
                              if (ots.spawnByWall)
                              {
                                  if(spawnListLoc[i].byWall)
                                  {
                                      createHere = true;
                                  }
                              }
                              else
                              {
                                  createHere = true;
                              }
                          }
                      }
                  } 
                  //find suitable place
                                   if (createHere)
                                   {
                                       SpawnList spawnLoc = spawnListLoc[i];
                                       GameObject obj;
                                       Quaternion spawnRotation = Quaternion.identity;
                                       //
                                       if (!is3D)
                                       {
                                           obj =Instantiate(ots.obj,new Vector3(spawnLoc.x*tileScaling,spawnLoc.y*tileScaling,0),spawnRotation)as GameObject;
                                           
                                       }
                                       else
                                       {
                                           if (spawnLoc.byWall)
                                           {
                                               if (spawnLoc.wallLoc == "S")
                                               {
                                                   spawnRotation=Quaternion.Euler(new Vector3(0,270,0));
                                               }else if (spawnLoc.wallLoc == "N")
                                               {
                                                   spawnRotation=Quaternion.Euler(new Vector3(0,90,0));
                                               }else if (spawnLoc.wallLoc == "W")
                                               {
                                                   spawnRotation=Quaternion.Euler(new Vector3(0,100,0));
                                               }else if (spawnLoc.wallLoc == "E")
                                               {
                                                   spawnRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                               }
                                           }
                                           else
                                           {
                                               if (ots.spawnRotated)
                                               {
                                                   spawnRotation=Quaternion.Euler(new Vector3(0,rnd.NextInt(0,360),0));
                                               }
                                               else
                                               {
                                                   spawnRotation =
                                                       Quaternion.Euler(new Vector3(0, rnd.NextInt(0, 2) * 90, 0));
                                               }
                                           }
                                           //
                                           obj =Instantiate(ots.obj,new Vector3(spawnLoc.x *tileScaling,0+ots.heightFix,spawnLoc.y*tileScaling),spawnRotation)as GameObject;
                                           
                                       }
                                       //
                                       obj.transform.parent = transform;
                                       spawnListLoc[i].spawnObject = obj;
                                       objectToSpwan--;
                                       created = true;
                                       break;


                                   }
                                   
                }

                if (!created)
                {
                    objectToSpwan--;
                }
               
            }
            
        
            
        }
        
    }
    
}
