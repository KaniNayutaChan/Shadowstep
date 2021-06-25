using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject savePointMenu;
    public GameObject restCanvas;
    bool hasSpawned;

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, Player.instance.transform.position) < 1f)
        {
            if(Player.instance.currentState == Player.State.Moving)
            {
                restCanvas.SetActive(true);

                if (Input.GetKeyDown(KeyCode.UpArrow) && Player.instance.currentState == Player.State.Moving)
                {
                    Player.instance.currentState = Player.State.Saving;
                    RoomManager.instance.lastSavedRoomNumber = RoomManager.instance.currentRoomNumber;
                    RoomManager.instance.RespawnEnemies();
                    Player.instance.HealToFull();
                    hasSpawned = false;
                    Debug.Log("Game saved");
                }
            }
        }
        else
        {
            restCanvas.SetActive(false);
        }

        if (Player.instance.transform.position == Vector3.zero && Player.instance.currentState == Player.State.Saving)
        {
            if (!hasSpawned)
            {
                hasSpawned = true;
                StartCoroutine(SetPauseMenuActive());
            }
        }
    }

    IEnumerator SetPauseMenuActive()
    {
        yield return new WaitForSeconds(1);
        Instantiate(savePointMenu);
    }
}
