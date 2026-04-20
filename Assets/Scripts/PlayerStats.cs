using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;
    public Image healthBarFill;

    private float currentStamina;
    private float maxStamina;
    public Image staminaBarFill;

    private bool isExhausted;
    private float exhaustionTimer;
    private float exhaustionCooldown;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100f;
        maxStamina = 100f;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        isExhausted = false;
        exhaustionTimer = 0f;
        exhaustionCooldown = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isExhausted)
        {
            exhaustionTimer -= Time.deltaTime;
            if (exhaustionTimer <= 0f)
            {
                isExhausted = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            GameManager.GM.GameOver();
        } else
        {
            currentHealth -= damage;
        }
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void LoseStamina(float stamina)
    {
        if (currentStamina - stamina <= 0)
        {
            currentStamina = 0;
            isExhausted = true;
            exhaustionTimer = exhaustionCooldown;
        } else
        {
            currentStamina -= stamina;
        }
        staminaBarFill.fillAmount = currentStamina / maxStamina;
    }

    public void RegenStamina(float stamina)
    {
        if (!isExhausted)
        {
            if (currentStamina + stamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            else
            {
                currentStamina += stamina;
            }
            staminaBarFill.fillAmount = currentStamina / maxStamina;
        }
    }

    public bool IsExhausted()
    {
        return isExhausted;
    }
}
