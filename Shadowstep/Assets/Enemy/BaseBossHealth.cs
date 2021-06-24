using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossHealth : BaseEnemyHealth
{
    protected override void Die()
    {
        base.Die();

        RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].enemies[number].enemyNumber = 0;
    }
}
