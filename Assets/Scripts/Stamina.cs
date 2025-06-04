using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public static Stamina Instance;

    public Image Staminabar;
    private float currentStamina;

    private bool isSprinting;
    private readonly float maxStamina = 100f;

    private bool Regenerating;
    private readonly float sprintDrain = 10f;

    private readonly KeyCode sprintkey = KeyCode.LeftShift;
    private readonly float StaminaRechargeRate = 5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentStamina = maxStamina;
        Staminabar.fillAmount = currentStamina / maxStamina;
    }

    // Update is called once per frame
    private void Update()
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
                currentStamina = 0;
            else
                isSprinting = false;
            if (!isSprinting && currentStamina < maxStamina) StaminaRegenerate();
        }
    }
}