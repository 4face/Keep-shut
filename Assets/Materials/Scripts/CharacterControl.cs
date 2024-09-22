using System;
using UnityEngine;

/// <summary>
/// Control of player in game
/// </summary>
public class CharacterControl : MonoBehaviour
{
    // Components
    private CharacterController characterController;
    private InputManager inputManager;
    private Transform cameraTransform;
    private DialogueController dialogueController;
    private StaminaBar staminaBar;

    // Settings for player movement
    [Header("Player Movement Settings")]
    [SerializeField] private float playerSpeed = 2.5f;
    private float currentSpeed;
    public float playerStatements;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaDrainRate = 20f;
    private Vector3 playerVelocity;
    private bool isGrounded;

    // For dialogues
    private static CharacterControl instance;
    private CharInfo currentChar;

    // Constants
    private const float interactionDistance = 3f;

    private void Awake()
    {
        // Singleton
        instance = this;
    }

    private void Start()
    {
        playerStatements = 0;
        inputManager = InputManager.Instance;
        staminaBar = StaminaBar.GetInstance();
        dialogueController = DialogueController.GetInstance();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        currentSpeed = playerSpeed;
    }

    private void Update()
    {
        // While player in dialogue no movement
        if (dialogueController.isPlaying) return;
        HandleMovement();
        HandleInteraction();
    }

    /// <summary>
    /// Handle of player movement
    /// </summary>
    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Getting direction of player movement
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        if (inputManager.PlayerRanThisFrame() && staminaBar.HaveEnoughStamina() && move.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed * 2, Time.deltaTime / 0.4f);
            staminaBar.DecreaseStamina();
            playerStatements = 2;
            if (!staminaBar.HaveEnoughStamina())
            {
                playerStatements = 1;
            }
        }
        else if(!inputManager.PlayerRanThisFrame() && move.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed, Time.deltaTime / 0.2f);
            playerStatements = 1;
            staminaBar.RegenerateStamina();
        }
        else if(!inputManager.PlayerRanThisFrame() && move.magnitude < 0.1f)
        {
            playerStatements = 0;
            staminaBar.RegenerateStamina();
        }

        // Transformation of movement in world coordinates with taking camera positions
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        characterController.Move(move * Time.deltaTime * currentSpeed);

        // Jump processing (if needed)
        /*
        if (inputManager.PlayerJumpedThisFrame() && isGrounded) 
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        */

        // Gravity handle
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Interaction Handle
    /// </summary>
    private void HandleInteraction()
    {
        if (inputManager.PlayerInteractedThisFrame())
        {
            RaycastFromCamera();
        }
    }

    /// <summary>
    /// Ray from camera to finding objects for interact.
    /// </summary>
    private void RaycastFromCamera()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            float distance = Vector3.Distance(transform.position, hitInfo.transform.position);

            if (distance < interactionDistance && hitInfo.collider.CompareTag("DialogueChar"))
            {
                CharInfo charInfo = hitInfo.collider.GetComponent<CharInfo>();

                if (charInfo.dialogueReady)
                {
                    StartDialogueWithCharacter(charInfo);
                }
            }
        }
    }

    /// <summary>
    /// Start of dialogue.
    /// </summary>
    /// <param name="charInfo">Information of character for dialogue.</param>
    private void StartDialogueWithCharacter(CharInfo charInfo)
    {
        charInfo.CharCamera.gameObject.SetActive(true);
        currentChar = charInfo;
        dialogueController.EnterDialogue(currentChar.inkJSON);
        Cursor.visible = true;
    }
    public static CharacterControl GetInstance()
    {
        return instance;
    }
}
