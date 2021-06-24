using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector] public State currentState;
    public enum State
    {
        Moving,
        Attacking,
        Blocking,
        Healing,
        Dashing,
        Grappling,
        Saving,
        Dead
    }

    [HideInInspector] public Rigidbody2D playerRB;
  
    [HideInInspector] public bool canMove = true;
    Vector3 moveInputVector = new Vector3();
    float moveInput;

    [Space]
    public float movementSpeed;

    [Space]
    public float maxHealth;
    public float currentHealth;

    [Space]
    public float maxPosture;
    public float currentPosture;

    [Space]
    public float maxLevel;
    public float currentLevel;
    public float currentExperience;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerRB = GetComponent<Rigidbody2D>();
        HealToFull();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Moving:
                Move();
                break;

            case State.Saving:
                Save();
                break;
        }

        CheckForDie();
        CheckForLevelUp();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            moveInput = 0;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.Moving)
        {
            moveInputVector.Set(moveInput, 0, 0);
            transform.position += moveInputVector * movementSpeed * Time.deltaTime;
        }
    }

    void CheckForDie()
    {
        if (currentHealth <= 0)
        {
            if (currentState != State.Dead)
            {
                currentState = State.Dead;
                canMove = false;
                playerRB.velocity = Vector2.zero;
                StartCoroutine(Die());
            }
        }
    }

    public float RequiredExperienceToLevelUp()
    {
        //Function of required experience based on level
        float requiredExperience = currentLevel * 10;
        return requiredExperience;
    }

    void CheckForLevelUp()
    {
        if (currentExperience >= RequiredExperienceToLevelUp())
        {
            LevelUp(RequiredExperienceToLevelUp() - currentExperience);
        }
    }

    void LevelUp(float experienceOverflow)
    {
        if (currentLevel < maxLevel)
        {
            currentLevel += 1;
            currentExperience = experienceOverflow;
            HealToFull();
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        Instantiate(RoomManager.instance.transition);
        RoomManager.instance.SpawnRoom(RoomManager.instance.lastSavedRoomNumber, RoomManager.instance.respawnPos);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.3f);
        currentState = State.Moving;
        currentExperience /= 2;
        HealToFull();
    }

    public void Save()
    {
        if (transform.position != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, movementSpeed * Time.deltaTime);
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
    }

    public IEnumerator SwitchState(State state, float time)
    {
        yield return new WaitForSeconds(time);
        currentState = state;
    }
}
