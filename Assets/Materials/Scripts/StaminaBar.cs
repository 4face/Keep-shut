using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public sealed class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image staminaBarImage; 
    [SerializeField] private float maxStamina = 100f; 
    private float currentStamina; 

    [SerializeField] private float staminaRegenRate = 10f; 
    [SerializeField] private float staminaDrainRate = 20f;

    private static StaminaBar instance;
    public bool StartDecrease;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // Inicialization current stamina on max value
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    // Metor for decrease stamina
    public void DecreaseStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaBar();
    }
    public void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateStaminaBar();
        }
    }
    private void UpdateStaminaBar()
    {
        staminaBarImage.fillAmount = currentStamina / maxStamina;
    }
    public static StaminaBar GetInstance() { return instance; }
    public bool HaveEnoughStamina() 
    { 
     return currentStamina > 0;
    }
}