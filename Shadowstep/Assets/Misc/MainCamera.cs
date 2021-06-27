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
            cameraDestination.x = RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].maxX;
        }
        else if (Player.instance.transform.position.x < RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].minX)
        {
            cameraDestination.x = RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].minX;
        }
        else
        {
            cameraDestination.x = Player.instance.transform.position.x;
        }

        if(Player.instance.transform.position.y > RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].maxY)
        {
            cameraDestination.y = RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].maxY;
        }
        else
        {
            cameraDestination.y = Player.instance.transform.position.x + yOffSet;
        }

        cameraDestination.z = -1;

        transform.position = Vector3.Lerp(transform.position, cameraDestination, speed);
    }
}
