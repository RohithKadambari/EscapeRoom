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

    public static Stamina Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentStamina = maxStamina;
        Staminabar.fillAmount = currentStamina / maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        StaminaDrain();
    }
    public void StaminaRegenerate()
    {
        Regenerating = true;
        if (currentStamina < maxStamina)
        {
            currentStamina += StaminaRechargeRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            Staminabar.fillAmount = currentStamina / maxStamina;
        }
        else
        {
            Regenerating = false;
        }

    }
    public void StaminaDrain()
    {
        if (Input.GetKey(sprintkey))
        {
            isSprinting = true;
            currentStamina -= sprintDrain * Time.deltaTime;
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
}
