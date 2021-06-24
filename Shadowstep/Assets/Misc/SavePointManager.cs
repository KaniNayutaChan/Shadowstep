using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    public void ResumeGame()
    {
        Player.instance.StartCoroutine(Player.instance.SwitchState(Player.State.Moving, 1));
        Destroy(gameObject);
    }
}
