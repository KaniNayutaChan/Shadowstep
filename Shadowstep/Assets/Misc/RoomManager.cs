using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [Space]
    public int debugSpawnRoomNumber;
    public Vector2 debugSpawnPosition;

    [Space]
    public GameObject transition;

    [HideInInspector] public int lastSavedRoomNumber;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public int currentRoomNumber;

    public EnemyList[] enemyList;
    [System.Serializable]
    public class EnemyList
    {
        public GameObject[] variantList;
    }

    [System.Serializable]
    public class Enemy
    {
        public int enemyNumber;
        public Vector3 position;
        [HideInInspector] public int variantNumber;

        public Enemy Clone()
        {
            Enemy clone = new Enemy();
            clone.enemyNumber = enemyNumber;
            clone.position = position;
            clone.variantNumber = variantNumber;
            return clone;
        }
    }

    public enum AreaType
    {
        Gatehouse,
        EastWatchtower,
        WestWatchtower,
        Church,
        ThroneRoom,
        Keep,
        Dungeon,
        Laboratory,
        Prison,
        Library,
        GreatHalls,
        TrainingGround,
        BedChambers
    }

    public Dictionary<AreaType, bool> hasAreaBeenVisited = new Dictionary<AreaType, bool>()
    {
        { AreaType.Gatehouse, false },
        { AreaType.EastWatchtower, false},
        { AreaType.WestWatchtower, false},
        { AreaType.Church, false},
        { AreaType.ThroneRoom, false},
        { AreaType.Keep, false},
        { AreaType.Dungeon, false},
        { AreaType.Laboratory, false},
        { AreaType.Prison, false},
        { AreaType.Library, false},
        { AreaType.GreatHalls, false},
        { AreaType.TrainingGround, false},
        { AreaType.BedChambers, false},
    };

    public GameObject[] areaCanvas;

    public RoomList[] rooms;
    [System.Serializable]
    public class RoomList
    {
        [Space]
        public GameObject room;

        [Space]
        public float minX;
        public float maxX;
        public float maxY;

        [Space]
        public bool isSaveRoom = false;
        [HideInInspector] public bool hasBeenVisited = false;

        [Space]
        public AreaType areaType;

        [Space]
        public Enemy[] enemies;
        [HideInInspector] public Enemy[] aliveEnemies;

        [Space]
        [HideInInspector] public bool[] gates = new bool[10];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(this);
        }

        if (currentRoom == null)
        {
#if DEBUG
            SpawnRoom(debugSpawnRoomNumber, debugSpawnPosition);

#else
            SpawnRoom(lastSavedRoomNumber, Vector3.zero);
#endif
        }

        RespawnEnemies();
    }

    public void RespawnEnemies()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            for (int j = 0; j < rooms[i].enemies.Length; j++)
            {
                rooms[i].enemies[j].variantNumber = Random.Range(0, enemyList[rooms[i].enemies[j].enemyNumber].variantList.Length);
            }
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].aliveEnemies = new Enemy[rooms[i].enemies.Length];

            for (int j = 0; j < rooms[i].enemies.Length; j++)
            {
                rooms[i].aliveEnemies[j] = rooms[i].enemies[j].Clone();
            }
        }
    }

    public void SpawnRoom(int room, Vector3 spawnPos)
    {
        StartCoroutine(InstantiateRoom(room, spawnPos));
    }

    IEnumerator InstantiateRoom(int room, Vector3 spawnPos)
    {
        yield return new WaitForSeconds(1.5f);

        //destroy current room
        Destroy(currentRoom);
        //set new room to has been visited for map
        rooms[currentRoomNumber].hasBeenVisited = true;
        //store the current room number
        currentRoomNumber = room;
        //instantiate current room
        currentRoom = Instantiate(rooms[room].room);
        //spawn player at correct position
        Player.instance.transform.position = spawnPos;
        //change player state back to moving
        Player.instance.currentState = Player.State.Moving;

        //check if the area has been visited before
        if(!hasAreaBeenVisited[rooms[room].areaType])
        {
            hasAreaBeenVisited[rooms[room].areaType] = true;

            Instantiate(areaCanvas[(int)rooms[room].areaType], currentRoom.transform);
        }

        //spawn each enemy
        for (int i = 0; i < rooms[room].aliveEnemies.Length; i++)
        {
            if (rooms[room].aliveEnemies[i].enemyNumber != 0)
            {
                GameObject enemy = Instantiate(enemyList[rooms[room].aliveEnemies[i].enemyNumber].variantList[rooms[room].aliveEnemies[i].variantNumber], rooms[room].aliveEnemies[i].position, transform.rotation, currentRoom.transform);
                enemy.GetComponent<BaseEnemy>().number = i;
            }
        }
    }
}
