using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image health;
    public Image posture;
    public Image experience;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = Player.instance.currentHealth / Player.instance.maxHealth;
        posture.fillAmount = Player.instance.currentPosture / Player.instance.maxPosture;
        experience.fillAmount = Player.instance.currentExperience / Player.instance.RequiredExperienceToLevelUp();
    }
}
