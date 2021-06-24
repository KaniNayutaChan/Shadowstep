using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Vector3 cameraDestination = new Vector3();
    public float yOffSet;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.transform.position.x > RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].maxX)
        {
            cameraDestination.Set(RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].maxX, Player.instance.transform.position.y + yOffSet, -1);
        }
        else if (Player.instance.transform.position.x < RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].minX)
        {
            cameraDestination.Set(RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].minX, Player.instance.transform.position.y + yOffSet, -1);
        }
        else
        {
            cameraDestination.Set(Player.instance.transform.position.x, Player.instance.transform.position.y + yOffSet, -1);
        }

        transform.position = Vector3.Lerp(transform.position, cameraDestination, speed);
    }
}
