using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector] public Rigidbody2D playerRB;
    [HideInInspector] public bool isDead = false;
  
    [HideInInspector] public bool canMove = true;
    Vector3 moveInputVector = new Vector3();
    float moveInput;

    [Space]
    public float movementSpeed;

    [Space]
    public float maxHealth;
    float currentHealth;

    [Space]
    [HideInInspector] public float currentExperience;
    public float requiredExperience;
    public float maxLevel;
    public float currentLevel;

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
        CheckForMove();
        CheckForDie();
        CheckForLevelUp();
    }

    void CheckForMove()
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
        if (!isDead && canMove)
        {
            moveInputVector.Set(moveInput, 0, 0);
            transform.position += moveInputVector * movementSpeed * Time.deltaTime;
        }
    }

    void CheckForDie()
    {
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                canMove = false;
                playerRB.velocity = Vector2.zero;
                StartCoroutine(Die());
            }
        }
    }

    void CheckForLevelUp()
    {
        if (currentExperience >= currentLevel * 10)
        {
            LevelUp((currentLevel * 10) - currentExperience);
        }
    }

    public void LevelUp(float experienceOverflow)
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
        isDead = false;
        currentExperience /= 2;
        HealToFull();
    }

    public void Save()
    {
        HealToFull();
        RoomManager.instance.RespawnEnemies();
        Debug.Log("Game saved");
    }

    void HealToFull()
    {
        currentHealth = maxHealth;
    }
}
