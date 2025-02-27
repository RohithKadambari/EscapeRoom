using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    float maxStamina = 100f;
    float currentStamina;
    float StaminaRechargeRate = 5f;
    float sprintDrain = 10f;

    KeyCode sprintkey = KeyCode.LeftShift;

    public Image Staminabar;

    bool isSprinting;

    bool Regenerating;
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        Staminabar.fillAmount = currentStamina / maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(sprintkey))
        {
            isSprinting = true;
            currentStamina -= sprintDrain;
            Staminabar.fillAmount = currentStamina / maxStamina;
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            else
            {
                isSprinting = false;


            }
            if (!isSprinting && currentStamina < maxStamina)
            {
                StaminaRegenerate();
            }

        }
    }
    public void StaminaRegenerate()
    {
        Regenerating = true;
        if (currentStamina < maxStamina)
        {
            currentStamina += StaminaRechargeRate;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            Staminabar.fillAmount = currentStamina / maxStamina;
        }
        else
        {
            Regenerating = false;
        }

    }
}
