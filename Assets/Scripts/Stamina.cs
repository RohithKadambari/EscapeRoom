using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    float maxStamina = 100f;
    float currentStamina;
    float StaminaRechargeRate = 5f;
    float sprintDrain = 10f;

    KeyCode sprintkey = KeyCode.LeftShift;

    bool isSprinting;
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(sprintkey))
        {
            isSprinting = true;
            currentStamina -= sprintDrain;
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            else
            {
                isSprinting = false;
                currentStamina += StaminaRechargeRate;
            }
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

        }
    }
}
