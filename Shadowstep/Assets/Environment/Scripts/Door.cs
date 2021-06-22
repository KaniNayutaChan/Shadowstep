using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int connectedRoom;
    public Vector3 spawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            RoomManager.instance.SpawnRoom(connectedRoom, spawnPos);
            Instantiate(RoomManager.instance.transition);
            Player.instance.canMove = false;
            Destroy(gameObject);
        }
    }
}
