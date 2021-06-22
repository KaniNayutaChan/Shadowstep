using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, Player.instance.transform.position) < 1f)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Player.instance.Save();
                RoomManager.instance.lastSavedRoomNumber = RoomManager.instance.currentRoomNumber;
                RoomManager.instance.respawnPos = transform.position;
            }
        }
    }
}
