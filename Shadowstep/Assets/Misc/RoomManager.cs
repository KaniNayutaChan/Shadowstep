using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [Space]
    public Transform mainCamera;

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
        public bool hasBeenVisited = false;
        public bool isSaveRoom = false;

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
