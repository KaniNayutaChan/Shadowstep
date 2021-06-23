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
        ChangeUIBar(health, Player.instance.currentHealth, Player.instance.maxHealth);
        ChangeUIBar(posture, Player.instance.currentPosture, Player.instance.maxPosture);
        ChangeUIBar(experience, Player.instance.currentExperience, Player.instance.RequiredExperienceToLevelUp());
    }

    void ChangeUIBar(Image UI, float current, float max)
    {
        if (UI.fillAmount < current / max)
        {
            if (UI.fillAmount + Time.deltaTime > current / max)
            {
                UI.fillAmount = current/max;
            }
            else
            {
                UI.fillAmount += Time.deltaTime;
            }
        }
        else if (UI.fillAmount > current / max)
        {
            if (UI.fillAmount - Time.deltaTime < current / max)
            {
                UI.fillAmount = current / max;
            }
            else
            {
                UI.fillAmount -= Time.deltaTime;
            }
        }
    }
}
