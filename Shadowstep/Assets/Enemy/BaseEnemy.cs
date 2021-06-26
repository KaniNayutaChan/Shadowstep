using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy: MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float maxPosture;
    public float currentPosture;
    public float experience;
    public float damageMultiplier = 3;
    public GameObject experienceOrb;
    protected Animator animator;
    protected bool hasDied;
    [HideInInspector] public int number;
    public float staggerTime;
    protected float currentStaggerTime;
    protected bool isStaggered;

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
        if(currentStaggerTime >= 0)
        {
            currentStaggerTime -= Time.deltaTime;
        }
        else if(isStaggered)
        {
            animator.Play("Idle");
            isStaggered = false;
        }
    }

    public void TakePosture(float posture)
    {
        currentPosture += posture;

        if (currentPosture > maxPosture)
        {
            animator.Play("Stagger");
            currentStaggerTime = staggerTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!hasDied)
        {
            if (currentPosture > maxPosture)
            {
                isStaggered = true;
                currentHealth = 0;
                animator.Play("Idle");
                currentHealth -= damage * damageMultiplier;
            }
            else
            {
                currentHealth -= damage;
            }

            if (currentHealth < 0)
            {
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        animator.Play("Death");
        hasDied = true;

        GameObject orb = Instantiate(experienceOrb, transform.position, transform.rotation, RoomManager.instance.currentRoom.transform);
        orb.GetComponent<ExperienceOrb>().experience = experience;
        RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].aliveEnemies[number].enemyNumber = 0;
    }
}
