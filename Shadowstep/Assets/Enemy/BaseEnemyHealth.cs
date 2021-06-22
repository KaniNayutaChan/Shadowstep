using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float maxPosture;
    public float currentPosture;
    public float experience;
    protected Animator animator;
    protected bool hasDied;
    [HideInInspector] public int number;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        hasDied = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentHealth <= 0 && !hasDied)
        {
            Die();
            hasDied = true;
        }
    }

    protected virtual void Die()
    {
        animator.Play("Death");

        Player.instance.currentExperience += experience;
        RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].aliveEnemies[number].enemyNumber = 0;
    }
}
