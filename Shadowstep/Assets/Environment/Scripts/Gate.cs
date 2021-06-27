using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator animator;
    public int number;
    public int[] requiredEnemies;

    public GateType gateType;
    public enum GateType
    {
        Lever,
        KillEnemies,
        Key,
        NightVision
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if(RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].gates[number])
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (gateType)
        {
            case GateType.KillEnemies:
                int counter = 0;
                for (int i = 0; i < requiredEnemies.Length; i++)
                {
                    if (RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].aliveEnemies[requiredEnemies[i]].enemyNumber == 0)
                    {
                        counter++;
                    }
                }
                if(counter == requiredEnemies.Length)
                {
                    Open();
                }
                break;
        }
    }

    public void Open()
    {
        animator.Play("Open");
        RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].gates[number] = true;
        Destroy(gameObject, 1);
    }
}
