using Cinemachine;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private InputManager inputManager;
    private Transform cameraTransform;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField]private float jumpHeight = 1f;
    [SerializeField]private float gravityValue = -9.81f;
    [SerializeField] private DialogueController dialogueController;
    private CinemachineVirtualCamera activeCamera;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        if (!dialogueController.InProgress)
        {
            isGrounded = characterController.isGrounded;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0;
            }
            Vector2 movement = inputManager.GetPlayerMovement();
            Vector3 move = new Vector3(movement.x, 0f, movement.y);
            move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
            move.y = 0f;
            characterController.Move(move * Time.deltaTime * playerSpeed);
            //if(inputManager.PlayerJumpedThisFrame() && isGrounded) 
            //{
            //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            //}
            //playerVelocity.y += gravityValue * Time.deltaTime;
            //characterController.Move(playerVelocity * Time.deltaTime);
            if (inputManager.PlayerIntectedThisFrame())
            {
                RaycastFromCamera();
            }
            if (activeCamera != null && !dialogueController.InProgress)
            {
                activeCamera.gameObject.SetActive(false);
                activeCamera = null;
                Cursor.visible = false;
            }
        }
    }
    private void RaycastFromCamera()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.collider.gameObject.GetComponent<CharInfo>().dialogueTriggers.PlayerInRange) 
            {
                    if (hitInfo.collider.gameObject.CompareTag("DialogueChar"))
                    {
                    CharInfo charInfo = hitInfo.collider.gameObject.GetComponent<CharInfo>();
                    CinemachineVirtualCamera camera = charInfo.Camera;
                    camera.gameObject.SetActive(true);
                    activeCamera = camera;
                    Debug.Log(charInfo.inkJSON.text);
                    //dialogueController.lines = charInfo.Dialogues;
                    //dialogueController.questions = charInfo.Questions;
                    //dialogueController.gameObject.SetActive(true);
                    //dialogueController.textComp.text = string.Empty;
                    //dialogueController.StartDialogue();
                    Cursor.visible = true;
                    }
            }
        }
    }
}
